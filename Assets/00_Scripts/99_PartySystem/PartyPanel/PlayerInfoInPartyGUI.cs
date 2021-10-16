using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoInPartyGUI : MonoBehaviour
{
    [SerializeField] Image m_character_image;
    [SerializeField] TMPro.TextMeshProUGUI m_level_tmpro;
    [SerializeField] TMPro.TextMeshProUGUI m_name_tmpro;
    [SerializeField] TMPro.TextMeshProUGUI m_type_tmpro;

    PlayerInfo m_current_playerinfo;

    public void SetInfo(PlayerInfo info)
    {
        m_current_playerinfo = info;
        m_character_image.sprite = null;
        var character_info = info.GetCharacterInfo();
        m_level_tmpro.text = "LV" + 10.ToString();
        m_name_tmpro.text = character_info.character_name;
        m_type_tmpro.text = character_info.character_type.ToString();
    }

    public void Flush()
    {
        m_current_playerinfo = null;
        m_character_image.sprite = null;
        m_level_tmpro.text = "";
        m_name_tmpro.text = "";
        m_type_tmpro.text = "";
    }
}
