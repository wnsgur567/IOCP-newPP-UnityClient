using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameInfoManager : Singleton<ClientGameInfoManager>,
    @PlayerInput.IMoveActionsActions, IPartyInOutCallback
{
    [SerializeField] Camera player_camera;  // player 이동 따라다니는 카메라
    [SerializeField] float player_movespeed;
    NetVector3 m_before_pos;
    [SerializeField] float m_tolerance = 3.0f;

    // server 로 send 할 간격 정하기
    // 초당 몇번을 서버로 보낼 지
    static int CheckCountPerSec = 3;
    static float SendInterval = 1.0f / CheckCountPerSec;
    float interval_acc = 0.0f;

    bool IsObjActivated = false;
    // 직접 조종 가능한 플레이어 오브젝트
    PlayerObject m_controll_object = null;

    public PlayerObject ControllPlayer
    {
        get { return m_controll_object; }
    }
    public PlayerInfo ControllPlayerInfo
    {
        get
        {
            return (PlayerInfo)m_controll_object.GetInfo();
        }
    }
    public PlayerPartyInfo PartyInfo
    {
        get { return m_controll_object.GetPartyInfo(); }
    }


    // input 값 관련 var [-1, 1]
    @PlayerInput m_input_system;
    Vector2 inputVector = new Vector2();

    private void Awake()
    {
        player_camera = Camera.main;
        m_before_pos = new NetVector3();
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

            // interval 보다 커지면 서버에 현재 position 을 전송
            interval_acc += Time.deltaTime;
            if (interval_acc > SendInterval)
            {
                interval_acc -= SendInterval;
                if (CheckPlayerPos())
                    SendPlayerPos();
            }
        }
    }

    // 이전 위치와 비교해서 일정 거리 이상 벗어났을 경우만 send 보낼 수 있도록 체크
    private bool CheckPlayerPos()
    {
        float x = m_before_pos.x - m_controll_object.NetPosition.x;
        float y = m_before_pos.y - m_controll_object.NetPosition.y;
        float z = m_before_pos.z - m_controll_object.NetPosition.z;

        m_before_pos.x = m_controll_object.NetPosition.x;
        m_before_pos.y = m_controll_object.NetPosition.y;
        m_before_pos.z = m_controll_object.NetPosition.z;

        float distance = (float)Math.Sqrt(x * x + y * y + z * z);
        if (distance < m_tolerance)
            return false;
        return true;
    }

    private void SendPlayerPos()
    {
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();

        // move 에 관한건 state 에 상관없이 동일한 protocol
        // 나중에 분리해
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

    // 파티에 참가 할 경우 셋팅할 것들
    public void OnEnterParty(PlayerPartyInfo info)
    {
        m_controll_object.SetPartyInfo(info);
        if (info == null)
        {   // 파티에 가입되지 않은 경우
            // ...
        }
        else
        {   // 파티에 가입한 경우
            // ...
        }
    }

    // 파티에서 나갈 경우 할 경우 셋팅할 것들
    public void OnExitParty()
    {
        m_controll_object.SetPartyInfo(null);
    }
}
