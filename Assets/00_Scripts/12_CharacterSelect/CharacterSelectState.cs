using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{

    public class CharacterSelectState : NetStateBase
    {
        NetStateBase.State m_state;

        public CharacterSelectState()
        {
            m_state = NetStateBase.State.CharacterSelect;
        }

        public NetStateBase.State SessionState => m_state;

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

        public object OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            byte[] protocol_bytes;
            recvpacket.Read(out protocol_bytes, sizeof(Protocol));
            Protocol protocol = (Protocol)BitConverter.ToInt64(protocol_bytes, 0);

            byte[] result_bytes;
            recvpacket.Read(out result_bytes, sizeof(Result));
            Result result = (Result)BitConverter.ToInt32(result_bytes, 0);

            // TODO : ...


            return result;
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
