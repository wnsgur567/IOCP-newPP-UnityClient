using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameInfoManager : Singleton<ClientGameInfoManager>, @PlayerInput.IMoveActionsActions
{
    [SerializeField] Camera player_camera;  // player �̵� ����ٴϴ� ī�޶�
    [SerializeField] float player_movespeed;

    // server �� send �� ���� ���ϱ�
    // �ʴ� ����� ������ ���� ��
    static int CheckCountPerSec = 3;
    static float SendInterval = 1.0f / CheckCountPerSec;
    float interval_acc = 0.0f;

    bool IsObjActivated = false;
    // ���� ���� ������ �÷��̾� ������Ʈ
    PlayerObject m_controll_object = null;
    // �� ������Ʈ ������ info
    PlayerInfo m_info;


    // input �� ���� var [-1, 1]
    @PlayerInput m_input_system;
    Vector2 inputVector = new Vector2();

    private void Awake()
    {
        player_camera = Camera.main;
    }

    private void OnEnable()
    {
        if (m_input_system == null)
            m_input_system = new PlayerInput();

        // callback link
        m_input_system.MoveActions.SetCallbacks(instance: this);
        m_input_system.MoveActions.Enable();
    }
    private void OnDisable()
    {
        m_input_system.MoveActions.Disable();
    }

    private void Update()
    {
        if (IsObjActivated)
        {
            var delta_vec = new Vector3(inputVector.x * player_movespeed * Time.deltaTime, inputVector.y * player_movespeed * Time.deltaTime);
            MovePlayer(delta_vec);

            // interval ���� Ŀ���� ������ ���� position �� ����
            interval_acc += Time.deltaTime;
            if (interval_acc > SendInterval)
            {
                interval_acc =- SendInterval;
                SendPlayerPos();
            }
        }
    }

    public void SendPlayerPos()
    {
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();

        // move �� ���Ѱ� state �� ������� ������ protocol
        // ���߿� �и���
        Net.VillageState.Protocol protocol = Net.VillageState.Protocol.PlayerMove;
        sendpacket.Write(((Int64)protocol));
        sendpacket.Write(m_controll_object.NetPosition);

        Net.NetworkManager.Instance.Send(sendpacket);
    }

    public void MovePlayer(Vector3 delta)
    {      
        m_controll_object.Position = m_controll_object.Position + delta;       
        player_camera.transform.position = player_camera.transform.position + delta;
    }

    public void SetControllObject(PlayerObject obj)
    {
        // set object
        IsObjActivated = true;
        m_controll_object = obj;
        m_info = (PlayerInfo)m_controll_object.GetInfo();

        m_controll_object.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);

        // set camera
        var pos = ((PlayerInfo)obj.GetInfo()).GetPosition();
        player_camera.transform.position = new Vector3(pos.x, pos.y, player_camera.transform.position.z);
    }


    public void OnNewaction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        //Debug.Log($"inputVector : {inputVector}");
    }
}
