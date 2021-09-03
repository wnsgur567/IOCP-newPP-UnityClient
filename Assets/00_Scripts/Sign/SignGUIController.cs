using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SignGUIController : MonoBehaviour, SignPanelInput.ISignKeysActions
{
    [SerializeField] Button m_dummy;

    [SerializeField] TMPro.TMP_InputField m_id_inputfield;
    [SerializeField] TMPro.TMP_InputField m_pw_inputfield;

    [SerializeField] Button m_login_button;
    [SerializeField] Button m_signup_button;
    [SerializeField] Button m_findID_button;
    [SerializeField] Button m_findPW_button;

    Dictionary<Selectable, Selectable> m_next_tab_dic;
    SignPanelInput m_input_sys;

    private void Awake()
    {
        m_next_tab_dic = new Dictionary<Selectable, Selectable>();
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
        EventSystem.current.SetSelectedGameObject(m_dummy.gameObject);

        if (m_next_tab_dic.Count == 0)
        {
            m_next_tab_dic.Add(m_dummy, m_id_inputfield);
            m_next_tab_dic.Add(m_id_inputfield, m_pw_inputfield);
            m_next_tab_dic.Add(m_pw_inputfield, m_login_button);
            m_next_tab_dic.Add(m_login_button, m_signup_button);
            m_next_tab_dic.Add(m_signup_button, m_findID_button);
            m_next_tab_dic.Add(m_findID_button, m_findPW_button);
            m_next_tab_dic.Add(m_findPW_button, m_dummy);
        }

        m_input_sys = new SignPanelInput();
        m_input_sys.SignKeys.SetCallbacks(instance: this);
        m_input_sys.SignKeys.Enable();
    }

    private void __Finalize()
    {
        m_next_tab_dic.Clear();
        m_input_sys.Disable();
    }

    public void OnTab(InputAction.CallbackContext context)
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            return;

        // change selected obj
        var selected_obj = EventSystem.current.currentSelectedGameObject;
        if (selected_obj == null)
            return;

        Selectable selectable_com = selected_obj.GetComponent<Selectable>();
        if (selectable_com == null ||
            m_next_tab_dic.ContainsKey(selectable_com) == false)
            return;

        var next_selected_com = m_next_tab_dic[selectable_com];
        EventSystem.current.SetSelectedGameObject(next_selected_com.gameObject);

        DebugConsoleGUIController.Instance.ShowMsg("focus changed" + next_selected_com.name);
    }

    #region Button Callback
    public void __OnSignInButtonCallback()
    {
        string id;
        string pw;

        id = m_id_inputfield.text;
        pw = m_pw_inputfield.text;

        NetApp.SignManager.Instance.SignInProcess(id, pw);
    }
    public void __OnSignUpButtonCallback()
    {
        string id;
        string pw;

        id = m_id_inputfield.text;
        pw = m_pw_inputfield.text;

        NetApp.SignManager.Instance.SignUpProcess(id, pw);
    }

    public void __OnFindIDButtonCallback()
    {

    }
    public void __OnFindPWButtonCallback()
    {

    }
    #endregion
}
