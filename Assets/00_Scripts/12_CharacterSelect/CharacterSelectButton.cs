using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    private CharacterSelectInfo m_info;
    [SerializeField] Image m_character_image;
    [SerializeField] TMPro.TextMeshProUGUI m_character_name_text;
    [SerializeField] TMPro.TextMeshProUGUI m_character_type_text;
    [SerializeField] Button m_button;    

    public void SetInfo(CharacterSelectInfo info)
    {
        m_info = info;
        OnInfoChanged();
    }

    public void ButtonInteractionOFF()
    {
        m_button.interactable = false;
    }
    public void ButtonInteractionON()
    {
        m_button.interactable = true;
    }


    private void OnInfoChanged()
    {
        // TODO : Character Button UI Update process
        // iamage ui update ...
        m_character_name_text.text = m_info.character_name;
        m_character_type_text.text = m_info.character_id.ToString();
    }

    public void __OnButtonClicked()
    {
        CharacterSelectGUIController.Instance.__OnCharacterSelected(m_info);
    }
}
