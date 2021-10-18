using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

// 파티 하나에 되한 정보
public class PlayerPartyInfo : Net.ISerializable
{
    public UInt32 m_party_id;
    public string m_party_name;
    public int m_max_playercount;
    public int m_cur_playercount;

    public PlayerInfo m_party_owner;
    public int m_owner_index;

    public List<PlayerInfo> m_player_vec;

    public PlayerPartyInfo()
    {
        m_player_vec = new List<PlayerInfo>();
        m_player_vec.Resize<PlayerInfo>(4);
    }

    public int DeSerialize(MemoryStream stream)
    {
        int size = 0;
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_party_id);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_party_name);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_cur_playercount);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_max_playercount);
        size += Net.StreamReadWriter.ReadFromBinStream(stream, out m_owner_index);

        for (int i = 0; i < m_cur_playercount; i++)
        {
            int index = -1;
            PlayerInfo info;
            size += Net.StreamReadWriter.ReadFromBinStream(stream, out index);
            size += Net.StreamReadWriter.ReadFromBinStreamSerializable(stream, out info);
            m_player_vec[index] = info;
        }

        return size;
    }

    // 사용 할 일 없음
    public int Serialize(MemoryStream stream)
    {
        throw new NotImplementedException();
        //int size = 0;
        //return size;
    }
}

// 검색 , 파티리스트 , 파티 생성 등을 총괄하는 Main GUI 컨트롤러
public class PartySystemGUIController : Singleton<PartySystemGUIController>,
    IGUIAcitvationHandler, IPartyInOutCallback, @PartyKeyboardInput.IPartySystemActions
{
    [SerializeField] Image m_root_panel;

    [SerializeField] TMPro.TMP_InputField m_inputfeild;       // 파티 검색 inputfield
    [SerializeField] Button m_search_button;        // 파티 검색 버튼
    [SerializeField] Button m_flush_button;         // inputfield 비우기 벝튼
    [SerializeField] Button m_create_button;        // 파티 생성 버튼
    [SerializeField] Button m_refresh_button;       // 파티 리스트 갱신 버튼 (서버 통신)

    [SerializeField] Button m_left_button;
    [SerializeField] Button m_right_button;

    int m_cur_page;
    const int ItemCountPerPage = 5;     // 한패이지당 5개의 파티가 보여짐  
    [SerializeField] List<PartyInfoGUIItem> m_show_list;    // PartyInfo 를 출력할 UI item list
    List<PlayerPartyInfo> m_party_infos;      // server 에서 불러온 파티 infomation
    List<PlayerPartyInfo> m_searched_infos;                  // search 버튼을 눌러 검색된 파티정보 리스트

    // key input
    PartyKeyboardInput m_input_system;

    private void Awake()
    {
        m_party_infos = new List<PlayerPartyInfo>();
        m_searched_infos = new List<PlayerPartyInfo>();
        ClearShowList();
        LinkButtonCallback();

        // 처음은 파티가입이 안 되어있는 상태로 시작
        OnExitParty();
        DeActivate();
    }

    private void OnEnable()
    {
        NetApp.PartyManager.Instance.LinkPartyEventCallbacks(this);

        // key input
        if (m_input_system == null)
            m_input_system = new PartyKeyboardInput();
        m_input_system.PartySystem.SetCallbacks(instance: this);
        m_input_system.PartySystem.Enable();
    }

    private void OnDisable()
    {
        NetApp.PartyManager.Instance.UnLinkPartyEventCallbacks(this);

        // key input
        m_input_system.PartySystem.Disable();
    }

    public void OnEnterParty(PlayerPartyInfo info)
    {
        m_create_button.interactable = false;
    }

    public void OnExitParty()
    {
        m_create_button.interactable = true;
    }


    // 서버에서 온 정보를 셋팅
    public void SetPartyList(List<PlayerPartyInfo> infos)
    {
        m_party_infos = infos;
        UpdatePartyInfoGUI();
    }

    private void LinkButtonCallback()
    {
        m_search_button.onClick.AddListener(SearchPartyProcess);
        m_flush_button.onClick.AddListener(FlushProcess);
        m_create_button.onClick.AddListener(ActivateCreateNewPartyPanel);
        m_refresh_button.onClick.AddListener(RefreshParty);

        // left...
        // right...
    }

    public void ClearShowList()
    {
        // gui party 정보들을 비우기
        foreach (var item in m_show_list)
        {
            item.Flush();
        }
    }
    // m_party_infos 를 기준으로 gui 업데이트
    private void UpdatePartyInfoGUI(int page = 0)
    {
        // gui 비우기
        ClearShowList();
        // 현재 표시중인 페이지 설정
        m_cur_page = page;
        // [begin , end)
        int begin_item_index = page * ItemCountPerPage;
        int end_item_index = begin_item_index + ItemCountPerPage;

        // info list의 item 개수 와 end 를 비교
        int list_count = m_party_infos.Count;
        for (int i = begin_item_index;
            i < ((m_party_infos.Count >= end_item_index) ? end_item_index : m_party_infos.Count);
            i++)
        {   // 파티 정보 셋팅 및 ui 업데이트
            m_show_list[(i - begin_item_index)].SetInfo(m_party_infos[i]);
        }
    }
    //m_serached_list 를 기준으로 gui를 업데이트
    private void UpdatePartyInfoGUI_Searched(int page = 0)
    {
        m_cur_page = page;
        // [begin , end)
        int begin_item_index = page * ItemCountPerPage;
        int end_item_index = begin_item_index + ItemCountPerPage;
    }

    // 인풋 필드에 있는 정보를 읽어
    // 파티 리스트에서 검색
    // 서버와 통신 x
    private void SearchPartyProcess()
    {

        // 검색 키워드 (input filed 에서 가져올 값)
        string keyword = m_inputfeild.text;
        m_searched_infos.Clear();
        foreach (var item in m_party_infos)
        {   // keyword 가 존재한다면 리스트에 추가
            if (item.m_party_name.Contains(keyword))
                m_searched_infos.Add(item);
        }
        // GUI 갱신
        UpdatePartyInfoGUI_Searched();
    }

    // m_inputfield 의 text 비우기
    // show list가 검색된 리스트를 보여주고 있는 상태라면 
    // m_party_info 가 표시되도록 변경
    private void FlushProcess()
    {
        m_cur_page = 0;
        m_inputfeild.text = "";
        // GUI 업데이트
        UpdatePartyInfoGUI();
    }

    // 파티 생성 창을 띄우기
    private void ActivateCreateNewPartyPanel()
    {
        PartyCreateGUIController.Instance.Activate();
    }

    // 서버로부터 정보를 받아 파티 리스트를 갱신함
    private void RefreshParty()
    {
        NetApp.PartyManager.Instance.SendRequestAllPartyList();
    }

    public void Activate()
    {
        m_root_panel.gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        m_root_panel.gameObject.SetActive(false);
    }

    public void OnPartySystemOnOffAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Party Key input");
            if (m_root_panel.gameObject.activeSelf)
            {   // ui is activated
                // .. do deactivate UI
                DeActivate();
            }
            else
            {   // ui is deactivated
                // .. do activate UI
                Activate();
            }
        }
    }
}
