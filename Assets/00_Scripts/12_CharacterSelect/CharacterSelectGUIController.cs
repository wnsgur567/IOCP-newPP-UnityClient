using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectGUIController : Singleton<CharacterSelectGUIController>
{
    [SerializeField] Image m_root_panel;
    [SerializeField] Image m_buttons_parent;        // character select button 들의 부모
    [SerializeField] List<CharacterSelectButton> m_buttons_list;        // character select button 관리 list

    private void Awake()
    {
        // child 를 관리 리스트로
        var children = m_buttons_parent.transform.GetComponentsInChildren<CharacterSelectButton>();
        m_buttons_list.AddRange(children);
    }

    private void OnEnable()
    {
        __Initialize();
    }
    private void OnDisable()
    {
        __Finalize();
    }

    private void __Initialize()
    {

    }
    private void __Finalize()
    {

    }

    public void __ON()
    {
        m_root_panel.gameObject.SetActive(true);
    }
    public void __OFF()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    public void SetCharacterInfomation(List<CharacterSelectInfo> infoList)
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            m_buttons_list[i].SetInfo(infoList[i]);
        }
    }

    #region Button Callback
    public void __OnCharacterSelected(CharacterSelectInfo info)
    {
        foreach (var item in m_buttons_list)
        {
            item.ButtonInteractionOFF();
        }
        NetApp.CharacterSelectManager.Instance.CharacterSelectedProcess(info);
    }
    public void __OnSignOutSelected()
    {
        NetApp.CharacterSelectManager.Instance.SignOutSelectedProcess();
    }

    #endregion
}
