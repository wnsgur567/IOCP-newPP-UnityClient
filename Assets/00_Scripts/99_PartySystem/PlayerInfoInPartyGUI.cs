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

    public void SetInfo(Sprite sprite, int level, string name, string type)
    {
        m_character_image.sprite = sprite;
        m_level_tmpro.text = "LV" + level.ToString();
        m_name_tmpro.text = name;
        m_type_tmpro.text = type;
    }

    public void Flush()
    {
        m_character_image.sprite = null;
        m_level_tmpro.text = "";
        m_name_tmpro.text = "";
        m_type_tmpro.text = "";
    }
}
