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
                    case Protocol.VillageInit:
                        VillageInitialize(char_data.result, char_data.recvPacket);
                        break;
                }
            }
        }

        private void VillageInitialize(Result result, Net.RecvPacket packet)
        {
            UInt32 village_id;
            packet.Read(out village_id);

            Debug.LogFormat("village_id : {0}", village_id.ToString());


            village_bk_obj.gameObject.SetActive(true);
        }

        // -------------------- On Recv process end-------------------//
    }
}

