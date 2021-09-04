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

        public static Queue<SignData> m_callback_queue = new Queue<SignData>();

        public static void CallbackReq(Protocol protocol, Result result, RecvPacket recvPacket)
        {
            m_callback_queue.Enqueue(
                new SignData()
                { protocol = protocol, result = result, recvPacket = recvPacket });
        }
    }
}
