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


    public override void OnCreated(NetObjectInfo initialize_info)
    {
        // info �� ������Ʈ �ʱ�ȭ ��
        SetInfo(initialize_info as PlayerInfo);
        Debug.Log("Player Obj Created");
        this.gameObject.SetActive(true);
    }

    public override void BeforeDestroy()
    {
        //throw new System.NotImplementedException();
    }

    public override NetObjectInfo GetInfo()
    {
        return m_info;
    }

    public override void SetInfo(NetObjectInfo info)
    {
        m_info = (PlayerInfo)info;
        OnInfoChanged();
    }


    private void OnInfoChanged()
    {
        this.transform.position = m_info.GetPosition();
    }
}