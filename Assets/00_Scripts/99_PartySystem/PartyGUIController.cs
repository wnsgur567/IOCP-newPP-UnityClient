using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyGUIController : Singleton<PartyGUIController>, IGUIAcitvationHandler
{
    [SerializeField] Image m_root_panel;

    [SerializeField] TMPro.TextMeshProUGUI m_party_name;
    [SerializeField] List<PlayerInfoInPartyGUI> m_playerInfo_list;
    [SerializeField] Button m_request_button;
    [SerializeField] Button m_exit_button;

    // 가장 최근에 표시했던 파티 정보
    PlayerPartyInfo m_current_party_info;

    private void Awake()
    {
        foreach (var item in m_playerInfo_list)
        {
            item.Flush();
        }

        m_request_button.onClick.AddListener(__OnRequestButtonClicked);
        m_exit_button.onClick.AddListener(__OnExitButtonClicked);

        DeActivate();
    }


    public void SetPartyInfo(PlayerPartyInfo info)
    {
        m_current_party_info = info;
        m_party_name.text = info.m_party_name;

        for (int i = 0; i < 4; i++)
        {
            var cur_player = info.m_player_vec[i];
            if (cur_player != null)
            {
                m_playerInfo_list[i].SetInfo(cur_player);
            }
        }
    }

    public void FlushPlayer(int index)
    {
        m_playerInfo_list[index].Flush();
    }

    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    public void __OnRequestButtonClicked()
    {

    }
    public void __OnExitButtonClicked()
    {
        NetApp.PartyManager.Instance.SendExitParty(m_current_party_info.m_party_id);
    }
}
