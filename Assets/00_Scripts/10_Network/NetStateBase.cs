using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    public interface NetStateBase
    {
        public enum State
        {
            None,

            Sign = 1,
            CharacterSelect = 2,

            Chat = 100,
        }

        public State SessionState { get; }

        public object OnRecvComplete(RecvPacket recvpacket);
        public void OnSendComplete();

        public void OccuredRecvException();
        public void OccuredSendException();

    }
}
