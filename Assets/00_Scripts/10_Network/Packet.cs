using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace Net
{
    public class PacketBase
    {
        public const int stream_capacity = 512;
    }

    // size 와 packet id 는 recv 받는 순간 확인함
    // packet id 를 포함하지 않는, 이후의 데이터 부터  recvpacket 에 기록 (data 부분만)
    public class RecvPacket : PacketBase
    {
        // state + othres
        int m_stream_size;      // max size from network recv stream
        protected MemoryStream m_stream;

        public RecvPacket()
        {

        }

        public void __Initialize()
        {
            m_stream = new MemoryStream(stream_capacity);
            m_stream.Position = 0;
        }
        public void Clear()
        {
            m_stream.Position = 0;
        }

        public void GetDataFromNetStream(NetworkStream recvstream, int copy_size)
        {
            m_stream.SetLength(copy_size);
            recvstream.Read(m_stream.GetBuffer(), 0, copy_size);
            m_stream_size = copy_size;
            m_stream.Position = 0;
        }

        public int Read<T, TEnum>(out TEnum item) where T : IConvertible where TEnum : Enum
        {
            T outData;
            int ret_size = StreamReadWriter.ReadFromBinStream(m_stream, out outData);
            item = (TEnum)(object)outData;
            return ret_size;
        }

        // Enum 이 함수 쓰지 마세요
        // 위에걸로
        public int Read<T>(out T item) where T : IConvertible
        {
            return StreamReadWriter.ReadFromBinStream(m_stream, out item);
        }

        // 이미 object 가 할당된 곳에 받아올 경우
        public int Read(ISerializable serializable)
        {
            return StreamReadWriter.ReadFromBinStream(m_stream, serializable);
        }
        // 새로 object 를 생성해서 받아와야 될 경우
        public int ReadSerializable<T>(out T derived) where T : ISerializable, new()
        {
            derived = new T();
            return StreamReadWriter.ReadFromBinStreamSerializable(m_stream, out derived);
        }
        public int ReadSerializabel<T>(out List<T> derived) where T : ISerializable, new()
        {
            return StreamReadWriter.ReadFromBinStreamSerializable(m_stream, out derived);
        }
        public int Read(out string str)
        {
            return StreamReadWriter.ReadFromBinStream(m_stream, out str);
        }
        public int Read<T>(out List<T> item_list) where T : IConvertible
        {
            return StreamReadWriter.ReadFromBinStream(m_stream, out item_list);
        }

        public void UnPacking()
        {
            // decryption 
            CipherManager.Decryption(m_stream.GetBuffer(), m_stream_size);
        }


        public int GetLength()
        {
            return Convert.ToInt32(m_stream.Position);
        }
    }

    public class SendPacket : PacketBase
    {
        // data (state + others)
        // m_stream has only data      
        protected MemoryStream m_stream;

        public SendPacket()
        {

        }

        public void __Initialize()
        {
            m_stream = new MemoryStream(stream_capacity);
            m_stream.Position = 0;
        }

        public void Clear()
        {
            m_stream.Position = 0;
        }

        public int Write<T, TEnum>(TEnum inItem) where T : IConvertible where TEnum : Enum
        {
            return StreamReadWriter.WriteToBinStream(m_stream, (T)(Object)inItem);
        }

        // for primitive
        // enum 을 param으로 하면 터짐 / 캐스팅이나 위 함수 사용할 것
        public int Write<T>(T inItem) where T : IConvertible
        {
            return StreamReadWriter.WriteToBinStream(m_stream, inItem);
        }
        // for string
        public int Write(string str)
        {
            return StreamReadWriter.WriteToBinStream(m_stream, str);
        }
        public int Write(ISerializable serializableObj)
        {
            return StreamReadWriter.WriteToBinStream(m_stream, serializableObj);
        }


        // return total send size for network send(networkstream.write)
        // total send size = total size + packet id size + encrypted data size
        public int Packing(byte[] sendstream, int packet_id)
        {
            // encryption;
            int encrypted_data_size = CipherManager.Encryption(m_stream.GetBuffer(),
                                                               Convert.ToInt32(m_stream.Position));
            m_stream.Position = encrypted_data_size;

            /// set sendstream for send
            int sendstream_head = 0;

            // copy 'total size' to sendstream
            // total size = packet id size + encrypted data size
            int total_size = sizeof(int) /*packet id size*/ + encrypted_data_size;
            byte[] total_size_bytes = BitConverter.GetBytes(total_size);
            Array.Copy(total_size_bytes, 0, sendstream, sendstream_head, total_size_bytes.Length);
            sendstream_head += total_size_bytes.Length;

            // copy 'packet_id' to sendstream           
            byte[] id_bytes = BitConverter.GetBytes(packet_id);
            Array.Copy(id_bytes, 0, sendstream, sendstream_head, id_bytes.Length);
            sendstream_head += id_bytes.Length;

            // copy 'encrypted data' to sendstream
            Array.Copy(m_stream.GetBuffer(), 0, sendstream, sendstream_head, encrypted_data_size);
            sendstream_head += encrypted_data_size;

            return sendstream_head;
        }


        public int GetLength()
        {
            return Convert.ToInt32(m_stream.Position);
        }
    }
}
