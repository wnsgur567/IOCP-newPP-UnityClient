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

            AllCharacterInfo = 1 << 0,
            CharacterSelect = 1 << 1,
            SignOut = 1 << 2,
        }
        public enum Result : Int32
        {
            None = 0,

            NoData = 1 << 0,
            CharaterInfos = 1 << 1,
            Success_CharacterSelect = 1 << 2,
            UndefinedCharacter = 1 << 3,
        }


        public void OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            Protocol protocol;
            recvpacket.Read<Int64, Protocol>(out protocol);

            Result result;
            recvpacket.Read<Int32, Result>(out result);

            switch (protocol)
            {                
                case Protocol.AllCharacterInfo:                    
                case Protocol.CharacterSelect:                   
                case Protocol.SignOut:
                    CharacterSelectConstatnts.CallbackReq(protocol, result, recvpacket);
                    break;
                default:
                    // throw exception
                    break;
            }

            // result 따라 state 변환 시키기
            switch (result)
            {
                case Result.None:
                    break;
                case Result.NoData:
                    break;
                case Result.CharaterInfos:
                    break;
                case Result.Success_CharacterSelect:
                    break;
                case Result.UndefinedCharacter:
                    break;
                default:
                    break;
            }
        }

        public void OnSendComplete()
        {

        }

        public void OnChanged()
        {
            Debug.Log("character select state start");
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
