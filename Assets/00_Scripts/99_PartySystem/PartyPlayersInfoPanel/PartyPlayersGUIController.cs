using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPlayersGUIController :
    Singleton<PartyPlayersGUIController>, IGUIAcitvationHandler, IPartyInOutCallback
{
    [SerializeField] Image m_root_panel;
    [SerializeField] List<PartyPlayerGUIItem> m_playerGUI_list;

    private void Awake()
    {   // 첫 시작시 비 활성화 된 상태로
        OnExitParty();
        DeActivate();
    }

    private void OnEnable()
    {
        NetApp.PartyManager.Instance.LinkPartyEventCallbacks(this);
    }

    private void OnDisable()
    {
        NetApp.PartyManager.Instance.UnLinkPartyEventCallbacks(this);
    }


    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    public void SetInfo(int index, PlayerInfo info)
    {
        m_playerGUI_list[index].SetInfo(info);
    }

    public void OnEnterParty(PlayerPartyInfo info)
    {    
        // list 내 각 slot의 정보를 업데이트   
        for (int i = 0; i < info.m_max_playercount; i++)
        {   // 해당위치에 플레이어 정보가 존재하면
            if (info.m_player_vec[i] != null)
            {   // 해당 slot 의 GUI를 업데이트 시키기
                m_playerGUI_list[i].SetInfo(info.m_player_vec[i]);
            }
        }
        // UI 활성화
        Activate();
    }

    public void OnExitParty()
    {
        // UI 비 활성화
        DeActivate();
    }
}
