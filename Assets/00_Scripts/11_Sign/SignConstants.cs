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
