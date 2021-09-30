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

        public enum Protocol : UInt64
        {
            None = 0,

            //VillageInit = 1 << 0,                     // village 정보 전송
            EnterVillage = 1UL << 1,                      // village 입장
            ExitVillage = 1UL << 2,						// village 퇴장

            FirstInit = 1UL << 20,            // village 입장 시 초기 정보들
            FirstInit_Others = 1UL << 21,     // village 입장 시 주변 player 정보를 가져옴
            PlayerAction = 1UL << 22,                                 // 이동하지 않는 모든 움직임
            PlayerMove = 1UL << 23,                                   // 이동하는 움직임
            PlayerMoveAndAction = (PlayerAction) | (PlayerMove),	// action 과 move 가 같이 일어날 수 있음

            EnterInView = 1UL << 24,          // 시야 범위 내로 들어온 새로운 오브젝트
            LeaveInView = 1UL << 25,			// 시야 범위 밖으로 나간 기존 오브젝트
            EnterSection = 1UL << 26,         // 새로운 섹션에 들어갈 경우 새로운 섹션에 존재하는 player들을 그려야 함
            LeaveSection = 1UL << 27          // 새로운 섹션에 들어갈 경우 기존에 그릴 필요 없는 놈들은 없에야됨
        }
        public enum Result : UInt32
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
            recvpacket.Read<UInt64, Protocol>(out protocol);

            Result result = Result.None;
            // recvpacket.Read<UInt32, Result>(out result);

            switch(protocol)
            {
                case Protocol.EnterVillage:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.ExitVillage:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.FirstInit:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.FirstInit_Others:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.PlayerMove:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.EnterInView:
                    Net.VillageConstants.CallbackReq(protocol,result,recvpacket);
                    break;
                case Protocol.LeaveInView:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.EnterSection:
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    break;
                case Protocol.LeaveSection:
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
            //throw new NotImplementedException();
        }
    }
}
