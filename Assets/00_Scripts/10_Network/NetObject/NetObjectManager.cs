using System;
using System.Collections.Generic;
using UnityEngine;
using ENetType = NetObjectInfo.ENetGameObjectType;

class NetObjectManager : Singleton<NetObjectManager>
{
    //[SerializeField] PlayerObject m_player_origin_obj; => 아래걸로 통합

    [SerializeField] List<NetObject> m_origins;     // Instatiate 할 NetObjectt의 오리지날 오브젝트
    Dictionary<ENetType, Func<NetObjectInfo, NetObject>> m_netObjectCreateFp_map;   // 위의 오브젝트에 대응되는 생성 프로세스를 정의한 Map


    Dictionary<UInt64, NetObject> m_netobject_map;  // 생성된 Network Object 를 관리하는 Map

    private void Awake()
    {
        m_netobject_map = new Dictionary<ulong, NetObject>();
        m_netObjectCreateFp_map = new Dictionary<ENetType, Func<NetObjectInfo, NetObject>>();

        // 생성 프로세스 맵핑
        foreach (var item in m_origins)
        {
            var net_type = item.GetInfo().NetObjType;
            m_netObjectCreateFp_map.Add(net_type,
                (NetObjectInfo object_info) =>
                {   // 새로운 Network 오브젝트를 생성하는 lamda 함수
                    var net_object = GameObject.Instantiate<NetObject>(item);
                    // object info 를 넘겨 내부에서 변수 셋팅하도록 함
                    net_object.OnCreated(object_info);
                    net_object.gameObject.SetActive(true);
                    return net_object;
                });
        }
    }

    // 이미 생성된 object 인지 판별
    public bool IsExist(UInt64 net_id)
    {
        return m_netobject_map.ContainsKey(net_id);
    }

    // 이미 존재하는 network object 를 가져오기
    public NetObject GetObject(UInt64 net_id)
    {
        if (false == m_netobject_map.ContainsKey(net_id))
            return null;
        return m_netobject_map[net_id];
    }

    // 해당 id값으로 이미 생성 된 object 가 있는 경우 바로 반환
    // 없다면 새로 생성해서 반환
    public NetObject GetObject(NetObjectInfo info)
    {
        // 이미 존재함
        if (IsExist(info.NetID))
        {  
            var retObject = m_netobject_map[info.NetID];
            retObject.SetInfo(info);    // info 를 반영
            return retObject;
        }

        // 생성
        return CreateNewNetworkObject(info);
    }  

    // TODO : 추후에 클라이언트 딴에서 네트워크오브젝트를 생성할 경우, 그걸 서버에 반영 시키는 함수를 등록 해야함


    // 새로운 네트워크 오브젝트를 생성
    public NetObject CreateNewNetworkObject(NetObjectInfo info)
    {
        var new_netObject = m_netObjectCreateFp_map[info.NetObjType].Invoke(info);
        new_netObject.OnCreated(info);                      // 생성된 오브젝트가 초기화 되도록 함
        m_netobject_map.Add(info.NetID, new_netObject);     // 관리 map에 등록
        return new_netObject;
    }

    // 기존의 네트워크 오브젝트를 제거
    public void Destroy(UInt64 net_id)
    {
        if (m_netobject_map.ContainsKey(net_id))
        {
            GameObject.Destroy(m_netobject_map[net_id].gameObject);
            m_netobject_map.Remove(net_id);
        }
    }
}

