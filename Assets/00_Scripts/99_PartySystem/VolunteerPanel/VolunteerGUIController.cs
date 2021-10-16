using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolunteerGUIController : Singleton<VolunteerGUIController>
{
    [SerializeField] Image m_character_image;
    [SerializeField] TMPro.TextMeshProUGUI m_name_tmpro;
    [SerializeField] TMPro.TextMeshProUGUI m_characterType_tmpro;

    [SerializeField] Button m_accept_button;
    [SerializeField] Button m_reject_button;
    [SerializeField] Button m_left_button;
    [SerializeField] Button m_right_button;

    public void SetCharacterInfo(Sprite sprite, string name, string type )
    {
        m_character_image.sprite = sprite;
        m_name_tmpro.text = name;
        m_characterType_tmpro.text = type;
    }
}
