using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPlayerGUI : MonoBehaviour
{
    [SerializeField] Image m_character_image;
    [SerializeField] TMPro.TextMeshProUGUI m_name_tmpro;
    [SerializeField] Slider m_hp_slider;
    [SerializeField] Slider m_mp_slider;

    PlayerInfo m_cur_info;

    public void Flush()
    {
        m_cur_info = null;
        m_character_image.sprite = null;
        m_name_tmpro.text = "";
        m_hp_slider.value = 1.0f;
        m_mp_slider.value = 1.0f;
    }
    public void SetInfo(PlayerInfo info)
    {
        m_cur_info = info;
        OnInfoChagned();
    }

    private void OnInfoChagned()
    {
        if (m_cur_info == null)
            Flush();

        var char_info = m_cur_info.GetCharacterInfo();
        // image set
        m_name_tmpro.text = char_info.character_name;
        m_hp_slider.value = 0.7f;   // temp
        m_mp_slider.value = 0.7f;   // temp
    }       
}
