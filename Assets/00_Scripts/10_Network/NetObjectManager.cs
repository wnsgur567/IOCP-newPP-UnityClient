using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class NetObjectManager : Singleton<NetObjectManager>
{
    [SerializeField] PlayerObject m_player_origin_obj;

    Dictionary<UInt64, NetObject> m_netobject_map;

    private void Awake()
    {
        m_netobject_map = new Dictionary<ulong, NetObject>();
    }

    public bool IsExist(UInt64 net_id)
    {
        return m_netobject_map.ContainsKey(net_id);
    }

    public T GetObject<T, TInfo>(TInfo info) where T : NetObject, new() where TInfo : notnull, NetObjectInfo
    {
        T obj;
        if (false == m_netobject_map.ContainsKey(info.NetID))
            obj = Create<T, TInfo>(info);
        else
            obj = (T)m_netobject_map[info.NetID];

        return obj;
    }

    private T Create<T, TInfo>(TInfo info) where T : NetObject, new() where TInfo : notnull, NetObjectInfo
    {
        //var n = GameObject.Instantiate<PlayerObject>(m_player_origin_obj);

        // player 외의 다른 타입 생성 시  type 에 맞게 생성 되도록 확장 필요
        // dictionary 로 origin 관리 하면 될듯
        var type = info.NetObjType;

        T retObj = GameObject.Instantiate<T>(m_player_origin_obj as T);
        retObj.OnCreated(info);
        return retObj;
    }

    public void Destroy(UInt64 net_id)
    {
        if (m_netobject_map.ContainsKey(net_id))
            m_netobject_map.Remove(net_id);
    }
}

