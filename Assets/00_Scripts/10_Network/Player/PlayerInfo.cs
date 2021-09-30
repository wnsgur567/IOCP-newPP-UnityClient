using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerInfo : NetObjectInfo
{
    CharacterSelectInfo m_character_info;
    NetVector3 m_position;

    public PlayerInfo() : base(NetObjectInfo.ENetGameObjectType.PlayerCharacter)
    {
        m_character_info = new CharacterSelectInfo();
        m_position = new NetVector3();
    }

    public Vector3 GetPosition()
    {
        return m_position.GetVector();
    }
    public NetVector3 GetNetPosition()
    {
        return m_position;
    }
    public void SetPosition(Vector3 vec)
    {
        m_position.x = vec.x;
        m_position.y = vec.y;
        m_position.z = vec.z;
    }

    public CharacterSelectInfo GetCharacterInfo()
    {
        return m_character_info;
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
