using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 파티 시스템 (파티 리스트를 출력하고 검색하는 창) GUI 내부에서
// 현재 파티원의 목록을 표시하는 리스트 중 하나의 항목에 대한 GUI class
public class PartyInfoGUIItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMPro.TextMeshProUGUI m_partyname_tmpro;
    [SerializeField] TMPro.TextMeshProUGUI m_playercount_tmpro;
    PlayerPartyInfo m_info = null;

    public void SetInfo(PlayerPartyInfo info)
    {
        m_info = info;
        OnInfoChanged(m_info.m_party_name,
            m_info.m_cur_playercount,
            m_info.m_max_playercount);
    }

    private void OnInfoChanged(string party_name, int cur_count, int max_count)
    {
        m_partyname_tmpro.text = party_name;
        m_playercount_tmpro.text = cur_count.ToString() + "/" + max_count.ToString();
    }

    public void Flush()
    {
        m_info = null;
        m_partyname_tmpro.text = "";
        m_playercount_tmpro.text = "";
    }

    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_info == null)
            return;

        // 클릭 되었을 경우 Party 정보를 표시하는 파티창을 띄움
        PartyGUIController.Instance.SetPartyInfo(m_info);
        PartyGUIController.Instance.Activate();
    }
}
