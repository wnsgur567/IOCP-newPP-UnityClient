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
    List<PlayerInfo> m_request_player = null;

    private void Awake()
    {
        m_request_player = new List<PlayerInfo>();
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
        m_request_player.Clear();
        m_root_panel.gameObject.SetActive(false);
    }

    public void OnEnterParty(PlayerPartyInfo info)
    {
        
    }

    public void OnExitParty()
    {        
        DeActivate();
    }

    public void SetCharacterInfo(Sprite sprite, string name, string type)
    {
        m_character_image.sprite = sprite;
        m_name_tmpro.text = name;
        m_characterType_tmpro.text = type;
    }
}
