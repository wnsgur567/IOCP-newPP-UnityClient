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

            //VillageInit = 1 << 0,                     // village ���� ����
            EnterVillage = 1UL << 1,                    // village ����
            ExitVillage = 1UL << 2,						// village ����

            FirstInit = 1UL << 20,            // village ���� �� �ʱ� ������
            FirstInit_Others = 1UL << 21,     // village ���� �� �ֺ� player ������ ������
            PlayerAction = 1UL << 22,                               // �̵����� �ʴ� ��� ������
            PlayerMove = 1UL << 23,                                 // �̵��ϴ� ������
            PlayerMoveAndAction = (PlayerAction) | (PlayerMove),	// action �� move �� ���� �Ͼ �� ����

            EnterInView = 1UL << 24,          // �þ� ���� ���� ���� ���ο� ������Ʈ
            LeaveInView = 1UL << 25,		  // �þ� ���� ������ ���� ���� ������Ʈ
            EnterSection = 1UL << 26,         // ���ο� ���ǿ� �� ��� ���ο� ���ǿ� �����ϴ� player���� �׷��� ��
            LeaveSection = 1UL << 27,          // ���ο� ���ǿ� �� ��� ������ �׸� �ʿ� ���� ����� �����ߵ�

            /// <summary>
            /// party system
            /// </summary>
            CreateParty = 1UL << 30,           // ��Ƽ ����		, Result ����
            RequestParticipate = 1UL << 31,    // ��Ƽ ���� ��û , Result ����

            NewParticipant = 1UL << 32,        // ���ο� ��Ƽ ������, ���� ��Ƽ������ ��������

            Exit = 1UL << 33,                  // �ڽ��� ��Ƽ���� ����
            Kick = 1UL << 34,                  // ��Ƽ���� ��Ƽ���� ����
            TransferOwner = 1UL << 35,         // ��Ƽ�� ����

            AllPartyInfo = 1UL << 36,			// ��� ��Ƽ ����
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
                    {   // ���� RESULT ���� ����
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
