using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 파티 창 GUI
// 파티 시스템 창에서 해당 파티 slot 을 눌렀을경우 표시하는 파티 세부 정보
// 현재 파티의 이름과 파티원 4명의 대한 정보를 표시함
// 파티 신청 버튼(플레이어가 파티 미 가입 시)
// 파티 탈퇴 버튼(플레이어가 파티 가입된 상태일 시)
public class PartyGUIController : Singleton<PartyGUIController>, IGUIAcitvationHandler, IPartyInOutCallback
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

    // 파티 시스템에서 선택한 slot의 파티정보에 따라 GUI를 갱신합니다
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

    // GUI 를 초기상태로 되돌립니다
    public void FlushPlayer(int index)
    {
        m_playerInfo_list[index].Flush();
    }

    // GUI 를 활성화
    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }
    // GUI 를 비활성화
    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    // Request button 클릭시 callback 함수
    public void __OnRequestButtonClicked()
    {

    }

    // Exit button 클릭시 callback 함수
    public void __OnExitButtonClicked()
    {
        NetApp.PartyManager.Instance.SendExitParty(m_current_party_info.m_party_id);
    }

    // 플레이어가 파티 참가 시 업데이트 할 것들    
    public void OnEnterParty(PlayerPartyInfo info)
    {
        // 파티 신청 버튼 비 활성화
        m_request_button.interactable = false;
    }
    // 플레이어가 파티 탈퇴 시 업데이트 할 것들
    public void OnExitParty()
    {
        // 파티 신청 버튼 활성화
        m_request_button.interactable = true;
    }
    

}
