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

            VillageInit = 1 << 0,                       // village ���� ����
            EnterVillage = 1 << 1,                      // village ����
            ExitVillage = 1 << 2,                       // village ����
            PlayerAction = 1 << 3,                      // �̵����� �ʴ� ��� ������
            PlayerMove = 1 << 4,                        // �̵��ϴ� ������
            PlayerMoveAndAction = (1 << 3) | (1 << 4),	// action �� move �� ���� �Ͼ �� ����
        }
        public enum Result : Int32
        {

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

            Result result;
            recvpacket.Read<Int32, Result>(out result);

            switch(protocol)
            {
                case Protocol.VillageInit:
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
