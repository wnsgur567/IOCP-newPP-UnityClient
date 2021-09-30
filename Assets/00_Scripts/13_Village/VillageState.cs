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

            //VillageInit = 1 << 0,                     // village ���� ����
            EnterVillage = 1UL << 1,                      // village ����
            ExitVillage = 1UL << 2,						// village ����

            FirstInit = 1UL << 20,            // village ���� �� �ʱ� ������
            FirstInit_Others = 1UL << 21,     // village ���� �� �ֺ� player ������ ������
            PlayerAction = 1UL << 22,                                 // �̵����� �ʴ� ��� ������
            PlayerMove = 1UL << 23,                                   // �̵��ϴ� ������
            PlayerMoveAndAction = (PlayerAction) | (PlayerMove),	// action �� move �� ���� �Ͼ �� ����

            EnterInView = 1UL << 24,          // �þ� ���� ���� ���� ���ο� ������Ʈ
            LeaveInView = 1UL << 25,			// �þ� ���� ������ ���� ���� ������Ʈ
            EnterSection = 1UL << 26,         // ���ο� ���ǿ� �� ��� ���ο� ���ǿ� �����ϴ� player���� �׷��� ��
            LeaveSection = 1UL << 27          // ���ο� ���ǿ� �� ��� ������ �׸� �ʿ� ���� ����� �����ߵ�
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
