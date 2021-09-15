using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SignInfo : Net.ISerializable
{
    string m_id;
    string m_pw;

    public string ID { get; private set; }
    public string PW { get; private set; }

    public void SetInfo(string inID, string inPW)
    {
        m_id = inID;
        m_pw = inPW;
    }

    public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_id);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_pw);
        return size;
    }

    public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, m_id);
        size += Net.StreamReadWriter.WriteToBinStream(stream, m_pw);
        return size;
    }
}

