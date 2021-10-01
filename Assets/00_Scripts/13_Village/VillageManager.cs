using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NetApp
{
    using Protocol = Net.VillageState.Protocol;    

    public class VillageManager : Singleton<VillageManager>
    {
        public enum Result
        {
            None = 0
        }

        [SerializeField] SpriteRenderer village_bk_obj;

        VillageBaseInfo m_current_village_info;

        private void Update()
        {
            CallbackCheck();
        }


        // -------------------- On Recv process ----------------------//
        private void CallbackCheck()
        {
            while (false == Net.VillageConstants.IsEmpty())
            {
                var char_data = Net.VillageConstants.Dequeue();
                switch (char_data.protocol)
                {
                    case Protocol.EnterVillage:
                        EnterVillageProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.ExitVillage:
                        ExitVillageProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.FirstInit:
                        FirstInitProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.FirstInit_Others:
                        FirstInitOthersProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.EnterInView:
                        EnterInViewProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.LeaveInView:
                        LeaveInViewProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.PlayerMove:
                        PlayerMoveProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.EnterSection:
                        EnterSectionProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.LeaveSection:
                        LeaveSectionProcess(char_data.result, char_data.recvPacket);
                        break;
                }
            }
        }

        private void EnterVillageProcess(Result result, Net.RecvPacket packet)
        {
            UInt32 village_id;
            packet.Read(out village_id);

            Debug.LogFormat("village_id : {0}", village_id.ToString());

            // 추후 id에 대응되는 정보로 셋팅하기 / create function 필요
            m_current_village_info = new FirstVillageInfo();
            m_current_village_info.__Initialize();
            m_current_village_info.m_pixel_size.x = 180 * 8;
            m_current_village_info.m_pixel_size.y = 180 * 3;
            m_current_village_info.m_village_name = "Test Village";

            // 추후 village id 에 대응되는 놈 활성화 되도록 변경 핅요함
            village_bk_obj.gameObject.SetActive(true);
        }
        private void ExitVillageProcess(Result result, Net.RecvPacket packet)
        {

        }


        // 첫 마을 입장시 자신의 플레이어 오브젝트 정보 recv 결과 처리
        private void FirstInitProcess(Result result, Net.RecvPacket packet)
        {
            PlayerInfo controll_info;
            packet.ReadSerializable(out controll_info);
            var obj = NetObjectManager.Instance.GetObject(controll_info);

            // control 오브젝트 등록
            ClientGameInfoManager.Instance.SetControllObject(obj as PlayerObject);
        }

        // 첫 마을 입장시 자신을 제외한 플레이어 오브젝트 정보 recv 결과 처리
        private void FirstInitOthersProcess(Result result, Net.RecvPacket packet)
        {
            Int32 player_count;
            packet.Read(out player_count);

            Debug.LogFormat("player count : {0}", player_count);

            for (int i = 0; i < player_count; i++)
            {
                PlayerInfo info;
                packet.ReadSerializable(out info);
                // 현재 클라에게 정보가 있으면 가져오고
                // 없으면 생성하고 가져오고 
                var obj = NetObjectManager.Instance.GetObject(info);
                obj.SetInfo(info);
            }
        }

        // sector 내부에서만 이동하는 경우 (이미 있는 오브젝트임)
        private void PlayerMoveProcess(Result result, Net.RecvPacket packet)
        {
            UInt64 net_id;
            packet.Read(out net_id);
            NetVector3 position;
            packet.ReadSerializable(out position);
            var obj = NetObjectManager.Instance.GetObject(net_id) as PlayerObject;
            if (obj)
            {
                obj.Position = new Vector3(position.x, position.y, position.z);
            }
        }
        private void EnterInViewProcess(Result result, Net.RecvPacket packet)
        {
            PlayerInfo info;
            packet.ReadSerializable(out info);
            var obj = NetObjectManager.Instance.GetObject(info);           
        }
        private void LeaveInViewProcess(Result result, Net.RecvPacket packet)
        {
            UInt64 net_id;
            packet.Read(out net_id);
            NetObjectManager.Instance.Destroy(net_id);
        }

        private void EnterSectionProcess(Result result,Net.RecvPacket packet)
        {
            Int32 player_count;
            packet.Read(out player_count);

            for (int i = 0; i < player_count; i++)
            {
                PlayerInfo info;
                packet.ReadSerializable(out info);
                // 현재 클라에게 정보가 있으면 가져오고
                // 없으면 생성하고 가져오고 
                var obj = NetObjectManager.Instance.GetObject(info);
                obj.SetInfo(info);
            }
        }

        private void LeaveSectionProcess(Result result,Net.RecvPacket packet)
        {
            Int32 player_count;
            packet.Read(out player_count);

            for (int i = 0; i < player_count; i++)
            {
                UInt64 net_id;
                packet.Read(out net_id);
                NetObjectManager.Instance.Destroy(net_id);
            }
        }
        // -------------------- On Recv process end-------------------//
    }
}

