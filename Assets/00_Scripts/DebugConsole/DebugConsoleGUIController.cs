using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-80)]
public class DebugConsoleGUIController : Singleton<DebugConsoleGUIController>, @NewControls.IActivate_toggleActions
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

    //[Space(10)]
    @NewControls m_input_control;

    private void Awake()
    {
        var mainPanel = GameObject.FindObjectOfType<Canvas>();
        m_slots_parent = new GameObject();
        m_slots_parent.transform.SetParent(m_slots_parent.transform);
        m_slots_parent.name = "slot_parent";

        m_active_slots_list = new List<TextSlot>();
        m_slot_pool = new Queue<TextSlot>();
    }

    private void OnEnable()
    {
        __Initailize_slots();
        __Initialize_InputSys();
    }
    private void OnDisable()
    {
        __Finalize_slots();
        __Finalize_InputSys();
    }

    public void __Initailize_slots()
    {
        for (int i = 0; i < m_max_lineCount; i++)
        {
            var newSlot = GameObject.Instantiate<TextSlot>(m_origin_slot);
            newSlot.gameObject.SetActive(false);
            newSlot.transform.SetParent(m_slots_parent.transform);
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

    public void __Initialize_InputSys()
    {
        m_input_control = new NewControls();
        m_input_control.activate_toggle.SetCallbacks(instance: this);
        m_input_control.activate_toggle.Enable();
    }

    public void __Finalize_InputSys()
    {
        m_input_control.activate_toggle.Disable();
    }

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
            yield return null;

        yield return new WaitForSeconds(delay);

        Retrieve(slot);
    }
    private void Retrieve(TextSlot slot)
    {
        slot.gameObject.SetActive(false);
        slot.transform.SetParent(m_slots_parent.transform,true);       
        m_slot_pool.Enqueue(slot);
    }

    public void OnActivate_toggle(InputAction.CallbackContext context)
    {
        IsShow = !IsShow;
        Debug.Log("Input System Callback");
    }

    [ContextMenu("test message")]
    public void Test()
    {
        ShowMsg("test message!!");
    }
}
