using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

using gid32_t = System.Int32;
using gid64_t = System.Int64;
using gsize32_t = System.UInt32;
using gsize64_t = System.UInt64;
using System;
using System.Threading;

namespace Net
{
    using packetSize_t = gid32_t;
    using packetId_t = gid32_t;

    public class NetSession
    {
        const string SERVER_IP = "127.0.0.1";
        const int SERVER_PORT = 9000;

        private TcpClient m_client;
        private NetworkStream m_netstream;
        private byte[] m_recvstream;
        private byte[] m_sendstream;

        private packetId_t m_newSendID;
        private packetId_t m_newRecvID;

        NetStateBase m_current_state;
        SignState m_sign_state;

        public NetSession()
        {

        }

        public void __Initialize()
        {
            m_newSendID = 1;
            m_newRecvID = 1;

            m_client = new TcpClient(SERVER_IP, SERVER_PORT);
            m_netstream = m_client.GetStream();
            m_recvstream = new byte[PacketBase.stream_capacity];
            m_sendstream = new byte[PacketBase.stream_capacity];
        }

        public void __Finalize()
        {
            m_netstream.Close();
            m_client.Close();
        }

        public void Recv(out RecvPacket recvPacket)
        {
            recvPacket = new RecvPacket();
            recvPacket.__Initialize();

            // size recv
            int packet_size = 0;
            byte[] size_byte = BitConverter.GetBytes(packet_size);
            m_netstream.Read(size_byte, 0, sizeof(int));
            packet_size = BitConverter.ToInt32(size_byte, 0);

            // data recv
            m_netstream.Read(m_recvstream, 0, packet_size);

            // get packet 'id' from m_recvstream
            byte[] id_bytes = new byte[sizeof(packetId_t)];
            Array.ConstrainedCopy(m_recvstream, 0, id_bytes, 0, sizeof(packetId_t));
            int packet_id = BitConverter.ToInt32(id_bytes, 0);

            // duplicate packet id (past recved)
            if (packet_id < m_newRecvID)
            {
                recvPacket = null;
                return;
            }
            ++m_newRecvID;

            int data_size = packet_size - sizeof(packetId_t);
            // copy 'data' from m_recvstream to recvpacket's stream
            Array.ConstrainedCopy(m_recvstream,
                                    sizeof(packetId_t),
                                    recvPacket.m_stream,
                                    0,
                                    data_size);

            // decryption
            CipherManager.Instance.Decryption(recvPacket.m_stream, data_size);

            OnRecvComplete(recvPacket);
        }
        public void Send(SendPacket sendPacket)
        {
            // encryption
            packetSize_t encrypted_packetsize = 0;
            encrypted_packetsize = CipherManager.Instance.Encryption(sendPacket.m_stream, sendPacket.GetLength());


            /// set sendstream for send
            int sendstream_head = 0;

            // set 'total size' to m_sendstream
            packetSize_t total_size = 0;
            total_size = sizeof(packetSize_t) + sizeof(packetId_t) + encrypted_packetsize;
            byte[] total_size_bytes = BitConverter.GetBytes(total_size);
            Array.Copy(total_size_bytes, 0, m_sendstream, sendstream_head, sizeof(packetSize_t));

            sendstream_head += sizeof(packetSize_t);

            // set 'id' to m_sendstream
            packetId_t id = m_newSendID++;
            byte[] id_bytes = BitConverter.GetBytes(id);
            Array.Copy(id_bytes, 0, m_sendstream, sendstream_head, sizeof(packetId_t));

            sendstream_head += sizeof(packetId_t);

            // set 'Encrypted data' to m_sendstream
            Array.Copy(sendPacket.m_stream, 0, m_sendstream, sendstream_head, encrypted_packetsize);
            sendstream_head += encrypted_packetsize;


            // send sendstream
            m_netstream.Write(m_sendstream, 0, total_size);

            OnSendComplete();
        }

        public void OnRecvComplete(RecvPacket recvPacket)
        {
            object signal = m_current_state.OnRecvComplete(recvPacket);

            switch (m_current_state.SessionState)
            {
                case NetStateBase.State.None:
                    break;
                case NetStateBase.State.Sign:

                    switch ((SignState.Result)signal)
                    {
                        case SignState.Result.Success_SingIn:
                            // TODO : state change
                            break;
                        case SignState.Result.Success_SignOut:
                            break;
                        case SignState.Result.Success_DeleteAccount:
                            break;
                        default:
                            break;
                    }
                    

                    break;
                default:
                    break;
            }
        }
        public void OnSendComplete()
        {
            m_current_state.OnSendComplete();
        }
    }
}

