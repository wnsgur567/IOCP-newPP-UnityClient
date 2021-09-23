using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Net
{

    public class VillageState : INetStateBase
    {
        NetSession session;
        INetStateBase.State m_state;

        public VillageState(NetSession Owner)
        {
            m_state = INetStateBase.State.Village;
            this.Owner = Owner;
        }

        public NetSession Owner
        {
            get => session;
            set { session = value; }
        }

        public INetStateBase.State SessionState => m_state;

        public enum Protocol : Int64
        {
            None = 0,

            //VillageInit = 1 << 0,                     // village 정보 전송
            EnterVillage = 1 << 1,                      // village 입장
            ExitVillage = 1 << 2,						// village 퇴장

            FirstInit = 1 << 20,            // village 입장 시 초기 정보들
            FirstInit_Others = 1 << 21,     // village 입장 시 주변 player 정보를 가져옴
            PlayerAction = 1 << 22,                                 // 이동하지 않는 모든 움직임
            PlayerMove = 1 << 23,                                   // 이동하는 움직임
            PlayerMoveAndAction = (PlayerAction) | (PlayerMove),	// action 과 move 가 같이 일어날 수 있음
        }
        public enum Result : Int32
        {
            None,
        }

        public void OccuredRecvException()
        {
            throw new NotImplementedException();
        }

        public void OccuredSendException()
        {
            throw new NotImplementedException();
        }

        public void OnChanged()
        {
            Debug.Log("Village select state start");
        }

        public void OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            Protocol protocol;
            recvpacket.Read<Int64, Protocol>(out protocol);

            Result result = Result.None;
            // recvpacket.Read<Int32, Result>(out result);

            switch(protocol)
            {
                case Protocol.EnterVillage:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.ExitVillage:

                    break;
                case Protocol.FirstInit:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.FirstInit_Others:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
            }

            switch (result)
            {                
                default:
                    break;
            }

        }

        public void OnSendComplete()
        {
            throw new NotImplementedException();
        }
    }
}
