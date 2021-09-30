using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public abstract class NetObjectInfo : Net.ISerializable
{
    public enum ENetGameObjectType
    {
        None,

        PlayerCharacter,
        Monster
    }

    [SerializeField] UInt64 m_net_id;
    ENetGameObjectType m_type;

    protected NetObjectInfo(ENetGameObjectType type)
    {
        m_type = type;
    }

    public UInt64 NetID { get { return m_net_id; } }
    public ENetGameObjectType NetObjType { get { return m_type; } }


    virtual public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_net_id);
        int type_raw;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out type_raw);
        m_type = (ENetGameObjectType)type_raw;
        return size;
    }

    virtual public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, m_net_id);
        size += Net.StreamReadWriter.WriteToBinStream(stream, (Int32)m_type);
        return size;
    }

}
