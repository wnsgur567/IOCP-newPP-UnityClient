using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectGUIController : Singleton<CharacterSelectGUIController>
{
    [SerializeField] Image m_root_panel;
    [SerializeField] Image m_buttons_parent;        // character select button ���� �θ�
    [SerializeField] List<CharacterSelectButton> m_buttons_list;        // character select button ���� list

    private void Awake()
    {
        // child �� ���� ����Ʈ��
        var children = m_buttons_parent.transform.GetComponentsInChildren<CharacterSelectButton>(true);
        m_buttons_list.AddRange(children);
        m_root_panel.gameObject.SetActive(false);
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
        // ��� ��ư�� ��Ȱ��ȭ
        // ���� ������ ���� �� ��� ���õǴ� ��ư�� Ȱ��ȭ ��ų��
        foreach (var item in m_buttons_list)
        {
            item.ButtonInteractionOFF();
        }
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
        // ���� ����
        // ���� �� ��ư Ȱ��ȭ
        int len = infoList.Count > m_buttons_list.Count ? m_buttons_list.Count : infoList.Count;
        for (int i = 0; i < len; i++)
        {
            m_buttons_list[i].SetInfo(infoList[i]);
            m_buttons_list[i].ButtonInteractionON();
        }
    }

    #region Button Callback
    public void __OnCharacterSelected(CharacterSelectInfo info)
    {
        foreach (var item in m_buttons_list)
        {
            item.ButtonInteractionOFF();
        }
        // server�� ��û
        NetApp.CharacterSelectManager.Instance.CharacterSelectedProcess(info);
        // player info �� ���� ĳ���ͷ� ����

    }
    public void __OnSignOutSelected()
    {
        // server�� ��û
        NetApp.CharacterSelectManager.Instance.SignOutSelectedProcess();
    }

    #endregion
}
