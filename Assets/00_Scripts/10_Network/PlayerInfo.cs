using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerInfo : NetObjectInfo
{
    CharacterSelectInfo m_character_info;
    NetVector3 m_position;

    public PlayerInfo()
    {
        m_character_info = new CharacterSelectInfo();
        m_position = new NetVector3();
    }

    public Vector3 GetPosition()
    {
        return m_position.GetVector();
    }

    override public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += base.Serialize(stream);
        size += Net.StreamReadWriter.WriteToBinStream(stream, m_character_info);
        size += Net.StreamReadWriter.WriteToBinStream(stream, m_position);
        return size;
    }

    public override int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += base.DeSerialize(stream);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, m_character_info);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, m_position);
        return size;
    }
}
