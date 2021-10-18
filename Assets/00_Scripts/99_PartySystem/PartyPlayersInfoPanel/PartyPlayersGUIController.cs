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
    {   // ù ���۽� �� Ȱ��ȭ �� ���·�
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
        // list �� �� slot�� ������ ������Ʈ   
        for (int i = 0; i < info.m_max_playercount; i++)
        {   // �ش���ġ�� �÷��̾� ������ �����ϸ�
            if (info.m_player_vec[i] != null)
            {   // �ش� slot �� GUI�� ������Ʈ ��Ű��
                m_playerGUI_list[i].SetInfo(info.m_player_vec[i]);
            }
        }
        // UI Ȱ��ȭ
        Activate();
    }

    public void OnExitParty()
    {
        // UI �� Ȱ��ȭ
        DeActivate();
    }
}
