using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-80)]
public class DebugConsoleGUIController : Singleton<DebugConsoleGUIController>
{
     

    [ReadOnly, SerializeField] bool IsShow;
    [Space(10)]
    [SerializeField, Min(100)] int m_max_lineCount;

    [Space(10)]
    Queue<TextSlot> m_slot_pool;
    List<TextSlot> m_active_slots_list;
    GameObject m_slots_parent;
    [SerializeField] TextSlot m_origin_slot;
    [SerializeField] ContentSizeFitter content_parent;

    private void Awake()
    {
        var mainPanel = GameObject.FindObjectOfType<Canvas>();
        m_slots_parent = new GameObject();
        m_slots_parent.transform.SetParent(mainPanel.transform);
        m_slots_parent.transform.localPosition = new Vector3(0, 0, 0);
        m_slots_parent.name = "slot_parent";

        m_active_slots_list = new List<TextSlot>();
        m_slot_pool = new Queue<TextSlot>();
    }

    private void OnEnable()
    {
        __Initailize_slots();         
    }
    private void OnDisable()
    {
        __Finalize_slots();  
    }

    private void Update()
    {
        while (Net.DebugConsoleGUIConstants.m_msg_queue.Count > 0)
        {
            var msg_data = Net.DebugConsoleGUIConstants.m_msg_queue.Dequeue();
            ShowMsg(msg_data.msg, msg_data.delay);
        }
    }

    public void __Initailize_slots()
    {
        for (int i = 0; i < m_max_lineCount; i++)
        {
            var newSlot = GameObject.Instantiate<TextSlot>(m_origin_slot);
            newSlot.gameObject.SetActive(false);
            newSlot.transform.SetParent(m_slots_parent.transform);
            newSlot.transform.localPosition = Vector3.zero;
            m_slot_pool.Enqueue(newSlot);
        }
    }

    public void __Finalize_slots()
    {
        foreach (var item in m_active_slots_list)
        {
            Retrieve(item);
        }
        m_active_slots_list.Clear();

        while (m_slot_pool.Count > 0)
        {
            var slot = m_slot_pool.Dequeue();
            GameObject.Destroy(slot);
        }
    }

 
    // for unity thread
    public void ShowMsg(string msg_text, float delay = 10.0f)
    {
        if (m_slot_pool.Count < 1)
            return;

        var show_slot = m_slot_pool.Dequeue();
        show_slot.SetText(msg_text);
        show_slot.transform.SetParent(content_parent.transform);
        show_slot.gameObject.SetActive(true);
        show_slot.transform.localScale = new Vector3(1, 1, 1);    
        
        Debug.Log(msg_text);
        StartCoroutine(Co_RetrieveProcess(show_slot, delay));
    }

    

    private IEnumerator Co_RetrieveProcess(TextSlot slot, float delay)
    {
        if (slot == null || slot.gameObject.activeSelf == false)
            yield break;

        yield return new WaitForSeconds(delay);

        Retrieve(slot);
    }
    private void Retrieve(TextSlot slot)
    {
        slot.gameObject.SetActive(false);
        slot.transform.SetParent(m_slots_parent.transform,true);       
        m_slot_pool.Enqueue(slot);
    }

    [ContextMenu("test message")]
    public void Test()
    {
        ShowMsg("test message!!");
    }
}
