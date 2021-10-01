using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerPartyInfo : Net.ISerializable
{
    public UInt32 m_party_id;
    public string m_party_name;
    public int m_max_playercount;
    public int m_cur_playercount;

    public PlayerInfo m_party_owner;
    public int m_owner_index;

    public List<PlayerInfo> m_player_vec;

    public int DeSerialize(MemoryStream stream)
    {
        throw new System.NotImplementedException();
    }

    public int Serialize(MemoryStream stream)
    {
        throw new System.NotImplementedException();
    }
}

public class PartySystemGUIController : Singleton<PartySystemGUIController>
{
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

    private void Awake()
    {
        m_party_infos = new List<PlayerPartyInfo>();
        m_searched_infos = new List<PlayerPartyInfo>();
        ClearShowList();
        LinkButtonCallback();
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
        m_create_button.onClick.AddListener(CreateNewParty);
        m_refresh_button.onClick.AddListener(RefreshParty);

        // left...
        // right...
    }

    private void ClearShowList()
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

    // 새로운 파티를 생성 , 서버에 해당 정보를 전송
    private void CreateNewParty()
    {

    }

    // 서버로부터 정보를 받아 파티 리스트를 갱신함
    private void RefreshParty()
    {

    }
}
