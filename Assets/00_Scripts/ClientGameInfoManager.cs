using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameInfoManager : Singleton<ClientGameInfoManager>
{
    [SerializeField] Camera player_camera;  // player �̵� ����ٴϴ� ī�޶�

    // ���� ���� ������ �÷��̾� ������Ʈ
    PlayerObject m_controll_object;
    // �� ������Ʈ ������ info
    PlayerInfo m_info;


    private void Awake()
    {
        player_camera = Camera.main;
    }

    public void SetControllObject(PlayerObject obj)
    {
        // set object
        m_controll_object = obj;
        m_info = m_controll_object.GetInfo();

        // set camera
        var pos = obj.GetInfo().GetPosition();
        player_camera.transform.position = new Vector3(pos.x, pos.y, player_camera.transform.position.z);
    }
}
