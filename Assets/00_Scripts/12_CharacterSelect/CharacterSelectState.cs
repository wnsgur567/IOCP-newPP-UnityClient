using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{

    public class CharacterSelectState : INetStateBase
    {
        NetSession session;
        INetStateBase.State m_state;
        public NetSession Owner
        {
            get => session;
            set { session = value; }
        }
        public CharacterSelectState(NetSession Owner)
        {
            m_state = INetStateBase.State.CharacterSelect;
            this.Owner = Owner;
        }

        public INetStateBase.State SessionState => m_state;

        public enum Protocol : Int64
        {
            None = 0,

            // flags...
        }
        public enum Result : Int32
        {
            None = 0,

            // start 2000
        }

        public void OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            Protocol protocol;
            recvpacket.Read<Int64, Protocol>(out protocol);

            Result result;
            recvpacket.Read<Int32, Result>(out result);
            // TODO : ...



        }

        public void OnSendComplete()
        {

        }

        public void OccuredRecvException()
        {
            throw new System.NotImplementedException();
        }

        public void OccuredSendException()
        {
            throw new System.NotImplementedException();
        }
    }
}
