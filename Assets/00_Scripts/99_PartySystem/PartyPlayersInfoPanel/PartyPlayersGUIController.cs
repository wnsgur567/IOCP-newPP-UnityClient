using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPlayersGUIController :
    Singleton<PartyPlayersGUIController>, IGUIAcitvationHandler
{
    [SerializeField] Image m_root_panel;
    [SerializeField] List<PartyPlayerGUIItem> m_playerGUI_list;

    private void Awake()
    {
        DeActivate();
    }
    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    public void SetInfo(int index, PlayerInfo info)
    {
        m_playerGUI_list[index].SetInfo(info);
    }
}
