using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : NetObject
{
    PlayerInfo m_info;

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
