using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolunteerGUIController : Singleton<VolunteerGUIController>
    , IGUIAcitvationHandler, IPartyInOutCallback
{
    [SerializeField] Image m_root_panel;

    [SerializeField] Image m_character_image;
    [SerializeField] TMPro.TextMeshProUGUI m_name_tmpro;
    [SerializeField] TMPro.TextMeshProUGUI m_characterType_tmpro;

    [SerializeField] Button m_accept_button;
    [SerializeField] Button m_reject_button;
    [SerializeField] Button m_left_button;
    [SerializeField] Button m_right_button;

    // 파티 신청자 리스트
    LinkedList<PlayerInfo> m_volunteer_list = null;
    LinkedListNode<PlayerInfo> m_current_volunteer = null;

    private void Awake()
    {
        m_volunteer_list = new LinkedList<PlayerInfo>();
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
        m_volunteer_list.Clear();
        m_root_panel.gameObject.SetActive(false);
    }

    public void OnEnterParty(PlayerPartyInfo info)
    {
        // ...
    }

    public void OnExitParty()
    {
        DeActivate();
    }

   

    // 새로운 Volunteer를 추가
    public void NewVolunteer(PlayerInfo info)
    {
        m_volunteer_list.AddLast(info);
        if (m_current_volunteer == null)
        {   // current 를 갱신
            m_current_volunteer = m_volunteer_list.First;
            OnVolunteerInfoChanged();
        }
       
    }

    // Accept button 클릭 시
    // 현재 Volunteer를 거부
    public void __OnAcceptButtonClicked()
    {
        NetApp.PartyManager.Instance.SendAcceptRequest(m_current_volunteer.Value);

        var n = m_current_volunteer.Next;
        m_volunteer_list.Remove(m_current_volunteer);
        m_current_volunteer = n;
        OnVolunteerInfoChanged();
        
    }

    // Reject button 클릭 시
    // 현재 Volunteer를 수락
    public void __OnRejectButtonClicked()
    {        
        NetApp.PartyManager.Instance.SendRejectRequest(m_current_volunteer.Value);

        var n = m_current_volunteer.Next;
        m_volunteer_list.Remove(m_current_volunteer);
        m_current_volunteer = n;
        OnVolunteerInfoChanged();

    }

    // current 가 갱신되는 경우
    private void OnVolunteerInfoChanged()
    {
        if (m_current_volunteer != null)
        {
            var character_info = m_current_volunteer.Value.GetCharacterInfo();
            SetCharacterInfoUI(null, character_info.character_name, character_info.character_type.ToString());
            Activate();
        }
        else
        {
            SetCharacterInfoUI(null, "", "");
            DeActivate();
        }
    }
    public void SetCharacterInfoUI(Sprite sprite, string name, string type)
    {
        m_character_image.sprite = sprite;
        m_name_tmpro.text = name;
        m_characterType_tmpro.text = type;
    }
}
