using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NetApp
{
    using Protocol = Net.VillageState.Protocol;
    using Result = Net.VillageState.Result;

    class VillageManager : Singleton<VillageManager>
    {
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
                        break;
                    case Protocol.FirstInit:
                        FirstInitProcess(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.FirstInit_Others:
                        FirstInitOthersProcess(char_data.result, char_data.recvPacket);
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

        private void FirstInitProcess(Result result, Net.RecvPacket packet)
        {
            PlayerInfo controll_info;
            packet.ReadSerializable(out controll_info);
            var obj = NetObjectManager.Instance.GetObject<PlayerObject, PlayerInfo>(controll_info);

            // control 오브젝트 등록
            ClientGameInfoManager.Instance.SetControllObject(obj);
        }
        private void FirstInitOthersProcess(Result result, Net.RecvPacket packet)
        {
            Int32 player_count;
            packet.Read(out player_count);

            for (int i = 0; i < player_count; i++)
            {
                PlayerInfo info;
                packet.ReadSerializable(out info);
                var obj = NetObjectManager.Instance.GetObject<PlayerObject,PlayerInfo>(info);                
            }
        }
        // -------------------- On Recv process end-------------------//
    }
}

