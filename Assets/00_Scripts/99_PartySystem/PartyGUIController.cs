using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyGUIController : Singleton<PartyGUIController>
{
    [SerializeField] TMPro.TextMeshProUGUI m_party_name;
    [SerializeField] List<PlayerInfoInPartyGUI> m_playerInfo_list;
    [SerializeField] Button m_request_button;
    [SerializeField] Button m_exit_button;

    private void Awake()
    {
        foreach (var item in m_playerInfo_list)
        {
            item.Flush();
        }
    }

    public void SetPartyInfo(string name)
    {
        m_party_name.text = name;
    }
    // index 0 - 3 (รั 4ธํ)
    public void SetPlayerInfo(int index, PlayerInfo info, bool isOwner = false)
    {
        var character_info = info.GetCharacterInfo();
        m_playerInfo_list[index].SetInfo(
            null,   // sprite
            1,      // level
            character_info.character_name,  // name
            character_info.character_type.ToString());  // character type
    }
    public void FlushPlayer(int index)
    {
        m_playerInfo_list[index].Flush();
    }
}
