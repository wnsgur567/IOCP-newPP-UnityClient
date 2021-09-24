using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObject : NetObject
{
    [SerializeField] PlayerInfo m_info;

    public Vector3 Position
    {
        get { return m_info.GetPosition(); }
        set
        {
            m_info.SetPosition(value);
            this.transform.position = value;
        }
    }

    public NetVector3 NetPosition
    {
        get { return m_info.GetNetPosition(); }
    }


    public void SetInfo(PlayerInfo info)
    {
        m_info = info;
        OnInfoChanged();
    }

    public PlayerInfo GetInfo()
    {
        return m_info;
    }

    private void OnInfoChanged()
    {
        this.transform.position = m_info.GetPosition();
    }

    public override void OnCreated(NetObjectInfo initialize_info)
    {
        // info 로 오브젝트 초기화 시
        SetInfo(initialize_info as PlayerInfo);
        Debug.Log("Player Obj Created");
        this.gameObject.SetActive(true);
    }

    public override void BeforeDestroy()
    {
        //throw new System.NotImplementedException();
    }
}
