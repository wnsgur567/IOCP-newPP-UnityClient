using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��Ƽ â GUI
// ��Ƽ �ý��� â���� �ش� ��Ƽ slot �� ��������� ǥ���ϴ� ��Ƽ ���� ����
// ���� ��Ƽ�� �̸��� ��Ƽ�� 4���� ���� ������ ǥ����
// ��Ƽ ��û ��ư(�÷��̾ ��Ƽ �� ���� ��)
// ��Ƽ Ż�� ��ư(�÷��̾ ��Ƽ ���Ե� ������ ��)
public class PartyGUIController : Singleton<PartyGUIController>, IGUIAcitvationHandler, IPartyInOutCallback
{
    [SerializeField] Image m_root_panel;

    [SerializeField] TMPro.TextMeshProUGUI m_party_name;
    [SerializeField] List<PlayerInfoInPartyGUI> m_playerInfo_list;
    [SerializeField] Button m_request_button;
    [SerializeField] Button m_exit_button;

    // ���� �ֱٿ� ǥ���ߴ� ��Ƽ ����
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

    // ��Ƽ �ý��ۿ��� ������ slot�� ��Ƽ������ ���� GUI�� �����մϴ�
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

    // GUI �� �ʱ���·� �ǵ����ϴ�
    public void FlushPlayer(int index)
    {
        m_playerInfo_list[index].Flush();
    }

    // GUI �� Ȱ��ȭ
    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }
    // GUI �� ��Ȱ��ȭ
    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    // Request button Ŭ���� callback �Լ�
    public void __OnRequestButtonClicked()
    {

    }

    // Exit button Ŭ���� callback �Լ�
    public void __OnExitButtonClicked()
    {
        NetApp.PartyManager.Instance.SendExitParty(m_current_party_info.m_party_id);
    }

    // �÷��̾ ��Ƽ ���� �� ������Ʈ �� �͵�    
    public void OnEnterParty(PlayerPartyInfo info)
    {
        // ��Ƽ ��û ��ư �� Ȱ��ȭ
        m_request_button.interactable = false;
    }
    // �÷��̾ ��Ƽ Ż�� �� ������Ʈ �� �͵�
    public void OnExitParty()
    {
        // ��Ƽ ��û ��ư Ȱ��ȭ
        m_request_button.interactable = true;
    }
    

}
