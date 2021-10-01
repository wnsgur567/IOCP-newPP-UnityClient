using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Net
{
    using VillageResult = NetApp.VillageManager.Result;
    using PartyResult = NetApp.PartyManager.Result;
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
            EnterVillage = 1UL << 1,                    // village 입장
            ExitVillage = 1UL << 2,						// village 퇴장

            FirstInit = 1UL << 20,            // village 입장 시 초기 정보들
            FirstInit_Others = 1UL << 21,     // village 입장 시 주변 player 정보를 가져옴
            PlayerAction = 1UL << 22,                               // 이동하지 않는 모든 움직임
            PlayerMove = 1UL << 23,                                 // 이동하는 움직임
            PlayerMoveAndAction = (PlayerAction) | (PlayerMove),	// action 과 move 가 같이 일어날 수 있음

            EnterInView = 1UL << 24,          // 시야 범위 내로 들어온 새로운 오브젝트
            LeaveInView = 1UL << 25,		  // 시야 범위 밖으로 나간 기존 오브젝트
            EnterSection = 1UL << 26,         // 새로운 섹션에 들어갈 경우 새로운 섹션에 존재하는 player들을 그려야 함
            LeaveSection = 1UL << 27,          // 새로운 섹션에 들어갈 경우 기존에 그릴 필요 없는 놈들은 없에야됨

            /// <summary>
            /// party system
            /// </summary>
            CreateParty = 1UL << 30,           // 파티 생성		, Result 있음
            RequestParticipate = 1UL << 31,    // 파티 참가 요청 , Result 있음

            NewParticipant = 1UL << 32,        // 새로운 파티 참가자, 기존 파티원에게 보낼때만

            Exit = 1UL << 33,                  // 자신이 파티에서 나감
            Kick = 1UL << 34,                  // 파티장이 파티에서 강퇴
            TransferOwner = 1UL << 35,         // 파티장 위임

            AllPartyInfo = 1UL << 36,			// 모든 파티 정보
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

            switch(protocol)
            {
                // village
                case Protocol.EnterVillage:
                case Protocol.ExitVillage:
                case Protocol.FirstInit:
                case Protocol.FirstInit_Others:
                case Protocol.PlayerMove:
                case Protocol.EnterInView:
                case Protocol.LeaveInView:
                case Protocol.EnterSection:
                case Protocol.LeaveSection:
                    {   // 현재 RESULT 따로 없음
                        VillageResult result = VillageResult.None;
                    Net.VillageConstants.CallbackReq(protocol, result, recvpacket);
                    }
                    break;

                // party
                case Protocol.CreateParty:
                case Protocol.RequestParticipate:
                case Protocol.NewParticipant:
                case Protocol.Exit:
                case Protocol.Kick:
                case Protocol.TransferOwner:
                case Protocol.AllPartyInfo:
                    {
                        PartyResult result = PartyResult.None;
                        recvpacket.Read<UInt32, PartyResult>(out result);
                        Net.PartyConstants.CallbackReq(protocol, result, recvpacket);
                    }
                    break;
            }
        }

        public void OnSendComplete()
        {
            //throw new NotImplementedException();
        }
    }
}
