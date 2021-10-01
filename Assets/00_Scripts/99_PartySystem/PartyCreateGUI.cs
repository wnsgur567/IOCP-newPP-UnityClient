using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyCreateGUI : MonoBehaviour
{
    [SerializeField] Image m_root_panel;

    [SerializeField] TMPro.TMP_InputField m_inputfield;
    [SerializeField] TMPro.TMP_Dropdown m_maxPlayerCount_dropdown;
    [SerializeField] Button m_ok_button;
    [SerializeField] Button m_cancle_button;

    Dictionary<int, int> m_dropdownIndex_to_playercount_dic;

    private void Awake()
    {
        m_dropdownIndex_to_playercount_dic = new Dictionary<int, int>();
        m_dropdownIndex_to_playercount_dic.Add(0, 4);
        m_dropdownIndex_to_playercount_dic.Add(1, 3);
        m_dropdownIndex_to_playercount_dic.Add(2, 2);
        m_dropdownIndex_to_playercount_dic.Add(3, 1);

        Flush();
        LinkButtonCallback();
    }

    public void Flush()
    {
        m_inputfield.text = "";
        m_maxPlayerCount_dropdown.value = 0;
    }
    private void LinkButtonCallback()
    {
        m_ok_button.onClick.AddListener(OnOkButtonClicked);
        m_cancle_button.onClick.AddListener(OnCancelButtonClicked);
    }

    public void Show()
    {
        m_root_panel.gameObject.SetActive(true);
    }
    public void UnShow()
    {
        m_root_panel.gameObject.SetActive(false);
    }


    private void OnOkButtonClicked()
    {
        string party_name = m_inputfield.text;
        if (party_name == "")
            party_name = "No Named";
        var player_count =
            m_dropdownIndex_to_playercount_dic[m_maxPlayerCount_dropdown.value];

        NetApp.PartyManager.Instance.SendCreateRoomData(party_name, player_count);        
    }
    private void OnCancelButtonClicked()
    {

    }
}
