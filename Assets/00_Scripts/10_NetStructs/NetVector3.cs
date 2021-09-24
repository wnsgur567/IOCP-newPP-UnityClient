using Net;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class NetVector3 : Net.ISerializable
{
    UnityEngine.Vector3 vec;

    public Vector3 GetVector() { return vec; }

    public float x
    {
        get { return vec.x; }
        set { vec.x = value; }
    }
    public float y
    {
        get { return vec.y; }
        set { vec.y = value; }
    }
    public float z
    {
        get { return vec.z; }
        set { vec.z = value; }
    }

    int ISerializable.DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out vec.x);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out vec.y);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out vec.z);
        return size;
    }

    int ISerializable.Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.x);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.y);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.z);
        return size;
    }
}

public class NetVector3Int : Net.ISerializable
{
    UnityEngine.Vector3Int vec;

    public Vector3Int GetVector() { return vec; }
    public int x
    {
        get { return vec.x; }
        set { vec.x = value; }
    }
    public int y
    {
        get { return vec.y; }
        set { vec.y = value; }
    }
    public int z
    {
        get { return vec.z; }
        set { vec.z = value; }
    }

    int ISerializable.DeSerialize(MemoryStream stream)
    {
        int size = 0;
        int val;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out val);
        vec.x = val;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out val);
        vec.y = val;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out val);
        vec.z = val;
        return size;
    }

    int ISerializable.Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.x);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.y);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.z);
        return size;
    }
}
