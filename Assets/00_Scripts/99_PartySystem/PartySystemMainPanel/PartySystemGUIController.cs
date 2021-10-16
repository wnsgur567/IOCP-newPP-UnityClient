using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

// ��Ƽ �ϳ��� ���� ����
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

    // ��� �� �� ����
    public int Serialize(MemoryStream stream)
    {
        throw new NotImplementedException();
        //int size = 0;
        //return size;
    }
}

// �˻� , ��Ƽ����Ʈ , ��Ƽ ���� ���� �Ѱ��ϴ� Main GUI ��Ʈ�ѷ�
public class PartySystemGUIController : Singleton<PartySystemGUIController>
{
    [SerializeField] Image m_create_party_panel;

    [SerializeField] TMPro.TMP_InputField m_inputfeild;       // ��Ƽ �˻� inputfield
    [SerializeField] Button m_search_button;        // ��Ƽ �˻� ��ư
    [SerializeField] Button m_flush_button;         // inputfield ���� ��ư
    [SerializeField] Button m_create_button;        // ��Ƽ ���� ��ư
    [SerializeField] Button m_refresh_button;       // ��Ƽ ����Ʈ ���� ��ư (���� ���)

    [SerializeField] Button m_left_button;
    [SerializeField] Button m_right_button;

    int m_cur_page;
    const int ItemCountPerPage = 5;     // ���������� 5���� ��Ƽ�� ������  
    [SerializeField] List<PartyInfoGUIItem> m_show_list;    // PartyInfo �� ����� UI item list
    List<PlayerPartyInfo> m_party_infos;      // server ���� �ҷ��� ��Ƽ infomation
    List<PlayerPartyInfo> m_searched_infos;                  // search ��ư�� ���� �˻��� ��Ƽ���� ����Ʈ

    private void Awake()
    {
        m_party_infos = new List<PlayerPartyInfo>();
        m_searched_infos = new List<PlayerPartyInfo>();
        ClearShowList();
        LinkButtonCallback();
    }

    // �������� �� ������ ����
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

    public void ClearShowList()
    {
        // gui party �������� ����
        foreach (var item in m_show_list)
        {
            item.Flush();
        }
    }
    // m_party_infos �� �������� gui ������Ʈ
    private void UpdatePartyInfoGUI(int page = 0)
    {
        // gui ����
        ClearShowList();
        // ���� ǥ������ ������ ����
        m_cur_page = page;
        // [begin , end)
        int begin_item_index = page * ItemCountPerPage;
        int end_item_index = begin_item_index + ItemCountPerPage;

        // info list�� item ���� �� end �� ��
        int list_count = m_party_infos.Count;
        for (int i = begin_item_index;
            i < ((m_party_infos.Count >= end_item_index) ? end_item_index : m_party_infos.Count);
            i++)
        {   // ��Ƽ ���� ���� �� ui ������Ʈ
            m_show_list[(i - begin_item_index)].SetInfo(m_party_infos[i]);
        }
    }
    //m_serached_list �� �������� gui�� ������Ʈ
    private void UpdatePartyInfoGUI_Searched(int page = 0)
    {
        m_cur_page = page;
        // [begin , end)
        int begin_item_index = page * ItemCountPerPage;
        int end_item_index = begin_item_index + ItemCountPerPage;
    }

    // ��ǲ �ʵ忡 �ִ� ������ �о�
    // ��Ƽ ����Ʈ���� �˻�
    // ������ ��� x
    private void SearchPartyProcess()
    {

        // �˻� Ű���� (input filed ���� ������ ��)
        string keyword = m_inputfeild.text;
        m_searched_infos.Clear();
        foreach (var item in m_party_infos)
        {   // keyword �� �����Ѵٸ� ����Ʈ�� �߰�
            if (item.m_party_name.Contains(keyword))
                m_searched_infos.Add(item);
        }
        // GUI ����
        UpdatePartyInfoGUI_Searched();
    }

    // m_inputfield �� text ����
    // show list�� �˻��� ����Ʈ�� �����ְ� �ִ� ���¶�� 
    // m_party_info �� ǥ�õǵ��� ����
    private void FlushProcess()
    {
        m_cur_page = 0;
        m_inputfeild.text = "";
        // GUI ������Ʈ
        UpdatePartyInfoGUI();
    }

    // ��Ƽ ���� â�� ����
    private void CreateNewParty()
    {
        m_create_party_panel.gameObject.SetActive(true);
    }

    // �����κ��� ������ �޾� ��Ƽ ����Ʈ�� ������
    private void RefreshParty()
    {
        NetApp.PartyManager.Instance.SendRequestAllPartyList();
    }
}