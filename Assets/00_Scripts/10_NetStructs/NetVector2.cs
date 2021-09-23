using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetVector2 : Net.ISerializable
{
    UnityEngine.Vector2 vec;

    public Vector2 GetVector() { return vec; }

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

    public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out vec.x);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out vec.y);
        return size;
    }

    public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.x);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.y);
        return size;
    }
}

public class NetVector2Int : Net.ISerializable
{
    UnityEngine.Vector2Int vec;

    public Vector2Int GetVector() { return vec; }
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
    public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        int val;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out val);
        vec.x = val;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out val);
        vec.y = val;
        return size;
    }

    public int Serialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.x);
        size += Net.StreamReadWriter.WriteToBinStream(stream, vec.y);
        return size;
    }
}