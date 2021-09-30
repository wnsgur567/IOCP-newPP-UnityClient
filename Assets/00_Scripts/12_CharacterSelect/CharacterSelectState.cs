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

        public enum Protocol : UInt64
        {
            None = 0,

            AllCharacterInfo = 1UL << 0, 
            CharacterSelect = 1UL << 1,
            SignOut = 1UL << 2,
        }
        public enum Result : UInt32
        {
            None = 0,

            NoData = 1U << 0,
            CharaterInfos = 1U << 1,
            Success_CharacterSelect = 1U << 2,
            UndefinedCharacter = 1U << 3,
        }


        public void OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            Protocol protocol;
            recvpacket.Read<UInt64, Protocol>(out protocol);

            Result result;
            recvpacket.Read<UInt32, Result>(out result);

            switch (protocol)
            {                
                case Protocol.AllCharacterInfo:                    
                case Protocol.CharacterSelect:                   
                case Protocol.SignOut:                    
                    break;
                default:
                    // throw exception
                    break;
            }

            // manager 에게 넘길 queue 에 등록
            switch (result)
            {
                case Result.CharaterInfos:
                    CharacterSelectConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Result.None:
                case Result.NoData:
                case Result.UndefinedCharacter:
                    break;
                case Result.Success_CharacterSelect:
                    CharacterSelectConstants.CallbackReq(protocol, result, recvpacket);
                    Owner.ChangeState(Owner.m_village_state);
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
