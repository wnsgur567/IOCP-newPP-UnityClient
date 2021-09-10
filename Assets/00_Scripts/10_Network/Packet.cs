using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Net
{
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
        public void Read(out string str)
        {
            // get string length
            byte[] len_bytes;
            Read(out len_bytes, sizeof(int));
            int len = Convert.ToInt32(len_bytes);

            // get string 
            byte[] str_bytes;
            Read(out str_bytes, len * 2);   // unicode is 2 bytes
            str = Encoding.Unicode.GetString(str_bytes);
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
            int len = str.Length * 2; // unicode is 2 bytes
            Write(BitConverter.GetBytes(len), sizeof(int));
            Write(Encoding.Unicode.GetBytes(str), len);
        }

        public int GetLength()
        {
            return m_head;
        }
    }
}
