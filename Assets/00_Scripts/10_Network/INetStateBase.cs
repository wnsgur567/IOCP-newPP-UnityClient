using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    public interface INetStateBase
    {
        public enum State
        {
            None,

            Sign = 1,
            CharacterSelect = 2,

            Chat = 100,
        }

        public NetSession Owner { get; set; }
        public State SessionState { get; }

        public void OnRecvComplete(RecvPacket recvpacket);
        public void OnSendComplete();

        public void OccuredRecvException();
        public void OccuredSendException();

    }
}
