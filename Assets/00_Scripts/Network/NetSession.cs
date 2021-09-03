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
        private byte[] m_sendstream;

        private packetId_t m_newSendID;
        private packetId_t m_newRecvID;

    

        public NetSession()
        {
            
        }
        
        public void __Initialize()
        {
            m_newSendID = 1;
            m_newRecvID = 1;

            m_client = new TcpClient(SERVER_IP, SERVER_PORT);
            m_netstream = m_client.GetStream();
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
            int size = 0;
            byte[] size_byte = BitConverter.GetBytes(size);            
            m_netstream.Read(size_byte, 0, sizeof(int));
            size = BitConverter.ToInt32(size_byte,0);

            // data recv
            m_netstream.Read(recvPacket.m_stream, 0, size);
                        
            // get packet id
            byte[] id_bytes;
            recvPacket.Read(out id_bytes, sizeof(packetSize_t));
            int packet_id = BitConverter.ToInt32(id_bytes,0);

            // ม฿บน packet
            if (packet_id < m_newRecvID)
            {
                recvPacket = null;
                return;
            }
            ++m_newRecvID;

            // decryption
        }
        public void Send(SendPacket sendPacket)
        {
            // encryption
            packetSize_t encrypted_packetsize = 0;
            // send
            m_netstream.Write(sendPacket.m_stream, 0, encrypted_packetsize);
        }
    }
}

