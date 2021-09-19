using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

using gid32_t = System.UInt32;
using gid64_t = System.UInt64;
using gsize32_t = System.UInt32;
using gsize64_t = System.UInt64;
using System;
using System.Threading;
using System.Net;

namespace Net
{
    // c++ 처럼 공용으로 사용하질 못하네?
    using packetSize_t = gid32_t;
    using packetId_t = gid32_t;

    public class NetSession
    {
        const string SERVER_IP = "127.0.0.1";
        const int SERVER_PORT = 9000;

        internal TcpClient m_client;
        internal NetworkStream m_netstream;

        private byte[] m_recvstream;
        private byte[] m_sendstream;

        private packetId_t m_newSendID;
        private packetId_t m_newRecvID;
        Queue<SendPacket> m_sendpacket_queue;

        internal bool IsSignedIn;

        INetStateBase m_current_state;
        internal SignState m_sign_state;
        internal CharacterSelectState m_charselect_state;

        public NetSession()
        {

        }

        public void __Initialize()
        {
            __Initialize_Vars();
            __Initialize_State();
        }

        #region Init functions
        private void __Initialize_Vars()
        {
            m_newSendID = 1;
            m_newRecvID = 1;

            IsSignedIn = false;

            m_client = new TcpClient();
            IPEndPoint serverAddr = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);
            m_client.Connect(serverAddr);
            m_netstream = m_client.GetStream();
            m_recvstream = new byte[PacketBase.stream_capacity];
            m_sendstream = new byte[PacketBase.stream_capacity];
            m_sendpacket_queue = new Queue<SendPacket>();
        }
        private void __Initialize_State()
        {
            m_sign_state = new SignState(this);
            m_charselect_state = new CharacterSelectState(this);

            m_current_state = m_sign_state;
        }
        #endregion


        public void __Finalize()
        {
            m_netstream.Close();
            m_client.Close();
        }

        public void ChangeState(INetStateBase next_stage)
        {
            m_current_state = next_stage;
            DebugConsoleGUIConstants.ShowMsg_Req("Change State!!");
        }



        public void Recv()
        {        

            // size recv
            byte[] size_bytes = new byte[sizeof(int)];
            m_netstream.Read(size_bytes, 0, sizeof(int));
            int packet_size = BitConverter.ToInt32(size_bytes,0);

            // packet id recv
            byte[] id_bytes = new byte[sizeof(int)];          
            m_netstream.Read(id_bytes, 0, sizeof(int));
            int packet_id = BitConverter.ToInt32(id_bytes, 0);

            // duplicate packet id (past recved)
            if (packet_id < m_newRecvID)
            {
                return;
            }
            ++m_newRecvID;

            int data_size = packet_size - sizeof(int)/*id size*/;

            // copy 'data' from m_recvstream to recvpacket's stream
            RecvPacket recvPacket = new RecvPacket();
            recvPacket.__Initialize();
            recvPacket.GetDataFromNetStream(m_netstream, data_size);
                        
            recvPacket.UnPacking();

            OnRecvComplete(recvPacket);
        }
        public void OnRecvComplete(RecvPacket recvPacket)
        {
            m_current_state.OnRecvComplete(recvPacket);
        }

        public void SendReq(SendPacket sendPacket)
        {
            m_sendpacket_queue.Enqueue(sendPacket);
        }
        public void SendQueueProcess()
        {
            while (m_sendpacket_queue.Count > 0)
            {
                var sendPacket = m_sendpacket_queue.Dequeue();
                Send(sendPacket);
            }
        }

        private void Send(SendPacket sendPacket)
        {
            int send_size = sendPacket.Packing(m_sendstream, m_newSendID++);           

            // send sendstream
            m_netstream.Write(m_sendstream, 0, send_size);

            OnSendComplete();
        }


        public void OnSendComplete()
        {
            m_current_state.OnSendComplete();
        }
    }
}

