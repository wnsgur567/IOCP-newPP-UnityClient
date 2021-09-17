using System.Collections;
using System.Collections.Generic;

namespace Net
{
    using Protocol = CharacterSelectState.Protocol;
    using Result = CharacterSelectState.Result;

    public class CharacterSelectConstatnts
    {
        public struct CharacterSelectData
        {
            public Protocol protocol;
            public Result result;
            public RecvPacket recvPacket;
        }

        private static Queue<CharacterSelectData> m_callback_queue = new Queue<CharacterSelectData>();

        // Recv된 데이터를 큐에 저장
        public static void CallbackReq(Protocol protocol,Result result, RecvPacket recvPacket)
        {
            m_callback_queue.Enqueue(
                new CharacterSelectData()
                { protocol = protocol, result = result, recvPacket = recvPacket });
        }
        public static bool IsEmpty()
        {
            if (m_callback_queue.Count == 0)
                return true;
            return false;
        }
        public static CharacterSelectData Dequeue()
        {
            return m_callback_queue.Dequeue();
        }
    }
}


