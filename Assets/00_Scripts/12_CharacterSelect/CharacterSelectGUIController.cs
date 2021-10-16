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
        // 모든 버튼을 비활성화
        // 추후 정보가 셋팅 될 경우 셋팅되는 버튼만 활성화 시킬것
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
        // 정보 셋팅
        // 셋팅 된 버튼 활성화
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
        // server에 요청
        NetApp.CharacterSelectManager.Instance.CharacterSelectedProcess(info);
        // player info 를 현재 캐릭터로 갱신

    }
    public void __OnSignOutSelected()
    {
        // server에 요청
        NetApp.CharacterSelectManager.Instance.SignOutSelectedProcess();
    }

    #endregion
}
