using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    using Protocol = VillageState.Protocol;
    using Result = VillageState.Result;
    public class VillageConstants
    {
        public struct VillageData
        {
            public Protocol protocol;
            public Result result;
            public RecvPacket recvPacket;
        }

        private static Queue<VillageData> m_callback_queue = new Queue<VillageData>();

        // Recv된 데이터를 큐에 저장
        public static void CallbackReq(Protocol protocol, Result result, RecvPacket recvPacket)
        {
            m_callback_queue.Enqueue(
                new VillageData()
                { protocol = protocol, result = result, recvPacket = recvPacket });
        }
        public static bool IsEmpty()
        {
            if (m_callback_queue.Count == 0)
                return true;
            return false;
        }
        public static VillageData Dequeue()
        {
            return m_callback_queue.Dequeue();
        }
    }
}
