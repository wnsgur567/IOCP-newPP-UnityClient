using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 
public class CharacterSelectInfo : Net.ISerializable
{
    public UInt64 character_id;
    public UInt64 user_id;
    public UInt32 character_type;
    public string character_name;


    public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out character_id);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out user_id);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out character_type);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out character_name);
        return size;
    }

    public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, character_id);
        size += Net.StreamReadWriter.WriteToBinStream(stream, user_id);
        size += Net.StreamReadWriter.WriteToBinStream(stream, character_type);
        size += Net.StreamReadWriter.WriteToBinStream(stream, character_name);
        return size;
    }

    public void SetInfo(UInt64 id, UInt64 user_id, UInt32 char_type, string char_name)
    {
        this.character_id = id;
        this.user_id = user_id;
        this.character_type = char_type;
        this.character_name = char_name;
    }
}
