#define __DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gid32_t = System.Int32;
using gid64_t = System.Int64;
using gsize32_t = System.UInt32;
using gsize64_t = System.UInt64;
using System.Threading;
using System;
using System.Text;

namespace Net
{
    using packetSize_t = gid32_t;
    using packetId_t = gid32_t;

    public class NetworkManager : Singleton<NetworkManager>
    {
        public enum State
        {
            Error = -1,

            BeforeInitialize = 0,

            Connecting,
            Connected,

            Disconnecting,
            Disconnected,
        }

        private State m_state;

        private NetSession m_session;
        private Thread m_net_main_thread;
        private Thread m_net_recv_thread;

        public State NetState { get { return m_state; } }


        private void Awake()
        {
            m_state = State.BeforeInitialize;
        }

        private void OnEnable()
        {
            __Initialize();
        }

        public void __Initialize()
        {
            m_state = State.Connecting;

            m_net_main_thread = new Thread(NetThread);
            m_net_main_thread.Start();
        }

        public void __Finalize()
        {
            m_state = State.Disconnecting;
            m_session.__Finalize();
            m_state = State.Disconnected;
        }


        public void Send(SendPacket sendpacket)
        {
            m_session.Send(sendpacket);
        }

        public void Recv(out RecvPacket recvpacket)
        {
            m_session.Recv(out recvpacket);
        }


        void NetThread()
        {
            m_session = new NetSession();
            m_session.__Initialize();

            m_state = State.Connected;

            m_net_recv_thread = new Thread(RecvThread);
            m_net_recv_thread.Start();

            m_net_recv_thread.Join();

            __Finalize();
        }

        void RecvThread()
        {
            while (true)
            {

            }
        }
    }


    public class PacketBase
    {
        public const int stream_capacity = 512;
    }

    public class RecvPacket : PacketBase
    {
        // state + othres

        int m_head;
        internal byte[] m_stream;

        public RecvPacket()
        {

        }

        public void __Initialize()
        {
            m_head = 0;
            m_stream = new byte[stream_capacity];
        }

        public void Read(out byte[] data_bytes, int size)
        {
            data_bytes = new byte[size];
            Array.ConstrainedCopy(m_stream, m_head, data_bytes, 0, size);
            m_head += size;
        }

        public int GetLength()
        {
            return m_head;
        }
    }

    public class SendPacket : PacketBase
    {
        // data (state + others)
        // m_stream has only data
        int m_head;
        protected internal byte[] m_stream;

        public SendPacket()
        {

        }

        public void __Initialize()
        {
            m_head = 0;
            m_stream = new byte[stream_capacity];
        }

        public void Write(byte[] data_bytes, int size)
        {
            Array.ConstrainedCopy(data_bytes, 0, m_stream, m_head, size);
            m_head += size;
        }
        public void Write(string str)
        {
            int len = str.Length;
            Write(BitConverter.GetBytes(len), sizeof(int));
            Write(Encoding.Unicode.GetBytes(str), len);
        }

        public int GetLength()
        {
            return m_head;
        }
    }
}

