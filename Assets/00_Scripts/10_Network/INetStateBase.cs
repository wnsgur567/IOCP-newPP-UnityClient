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

        public abstract NetSession Owner { get; set; }
        public abstract State SessionState { get; }

        public abstract void OnRecvComplete(RecvPacket recvpacket);
        public abstract void OnSendComplete();
        public abstract void OnChanged();

        public abstract void OccuredRecvException();
        public abstract void OccuredSendException();

    }
}
