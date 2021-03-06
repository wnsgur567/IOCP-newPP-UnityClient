using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    using Protocol = SignState.Protocol;
    using Result = SignState.Result;


    public class SignConstants
    {
        public struct SignData
        {
            public Protocol protocol;
            public Result result;
            public RecvPacket recvPacket;
        }

        // Recv된 데이터를 큐에 저장
        // 각 매니저가 해당하는 constant의 queue에서 서버로 부터 받은 정보를 확인하여 처리함 (unity update 에서 확인)
        private static Queue<SignData> m_callback_queue = new Queue<SignData>();

        
        public static void CallbackReq(Protocol protocol, Result result, RecvPacket recvPacket)
        {
            m_callback_queue.Enqueue(
                new SignData()
                { protocol = protocol, result = result, recvPacket = recvPacket });
        }
        public static bool IsEmpty()
        {
            if (m_callback_queue.Count == 0)
                return true;
            return false;
        }
        public static SignData Dequeue()
        {
            return m_callback_queue.Dequeue();
        }
    }
}
