using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NetApp
{
    using Protocol = Net.VillageState.Protocol;

    // ��Ʈ��ũ �����忡�� ���� ��Ŷ�� �м��Ͽ� 
    // �ش� ��Ȳ�� �´� ���μ����� �����ϵ��� �Ѹ��� ��Ȱ
    public class PartyManager : Singleton<PartyManager>
    {
        delegate void OnEnterPartyHandler(PlayerPartyInfo party_info);
        delegate void OnExitPartyHandler();
        event OnEnterPartyHandler OnEnterPartyEvent;
        event OnExitPartyHandler OnExitPartyEvent;

        public void LinkPartyEventCallbacks(IPartyInOutCallback listner)
        {
            OnEnterPartyEvent += listner.OnEnterParty;
            OnExitPartyEvent += listner.OnExitParty;
        }

        public enum Result : UInt32
        {
            None = 0,

            PartyCreated = 1U << 0,            // ��Ƽ ���� �Ϸ�

            RequestChecking = 1U << 1,     // ��Ƽ ���� ��û �Ϸ�,
            RequestAccept = 1U << 2,           // ��Ƽ ���� ��û�� ��Ƽ���� ����
            RequestReject = 1U << 3,           // ��Ƽ ���� ��û�� ��Ƽ���� �ź�

            ExitComplete = 1U << 4,            // ��Ƽ ���� �Ϸ�
            ExitCompleteOther = 1U << 5,       // ��Ƽ�� �� �Ѹ��� ��Ƽ�� ����

            Kicked = 1U << 6,               // ���� ����
            KickedOther = 1U << 7,          // ��Ƽ�� �� �Ѹ��� �������				

            NotExistParty = 1U << 8,			// �������� �ʴ� ��Ƽ
            SuccessAllPartyInfo = 1U << 9,		// ��� ��Ƽ ���� �˻� ����
        }

        private void Update()
        {
            CallbackCheck();
        }

        // -------------------- Send process ----------------------//
        #region Send Process
        public void SendCreateRoomData(string name, int max_count)
        {
            Net.SendPacket sendPacket = new Net.SendPacket();
            sendPacket.__Initialize();
            // protocol + name + max_count
            Protocol protocol = Protocol.CreateParty;
            sendPacket.Write((UInt64)protocol);
            sendPacket.Write(name);
            sendPacket.Write(max_count);

            Net.NetworkManager.Instance.Send(sendPacket);
        }

        public void SendRequestAllPartyList()
        {
            Net.SendPacket sendPacket = new Net.SendPacket();
            sendPacket.__Initialize();

            Protocol protocol = Protocol.AllPartyInfo;
            sendPacket.Write((UInt64)protocol);

            Net.NetworkManager.Instance.Send(sendPacket);
        }

        public void SendExitParty(uint party_id)
        {
            Net.SendPacket sendPacket = new Net.SendPacket();
            sendPacket.__Initialize();

            Protocol protocol = Protocol.Exit;
            sendPacket.Write((UInt64)protocol);
            sendPacket.Write(party_id);

            Net.NetworkManager.Instance.Send(sendPacket);
        }
        #endregion
        // -------------------- Send process end ----------------------//


        // -------------------- On Recv process ----------------------//
        #region RecvProcess
        private void CallbackCheck()
        {
            while (false == Net.PartyConstants.IsEmpty())
            {
                var party_data = Net.PartyConstants.Dequeue();
                switch (party_data.protocol)
                {
                    case Net.VillageState.Protocol.CreateParty:
                        CreatePartyProcess(party_data.result, party_data.recvPacket);
                        break;

                    case Net.VillageState.Protocol.RequestParticipate:
                        RequestParticipateProcess(party_data.result, party_data.recvPacket);
                        break;
                    case Net.VillageState.Protocol.NewParticipant:
                        NewParticipantProcess(party_data.result, party_data.recvPacket);
                        break;
                    case Net.VillageState.Protocol.Exit:
                        ExitProcess(party_data.result, party_data.recvPacket);
                        break;
                    case Net.VillageState.Protocol.Kick:
                        KickProcess(party_data.result, party_data.recvPacket);
                        break;
                    case Net.VillageState.Protocol.TransferOwner:
                        TransferOwnerProcess(party_data.result, party_data.recvPacket);
                        break;
                    case Net.VillageState.Protocol.AllPartyInfo:
                        AllPartyInfoProcess(party_data.result, party_data.recvPacket);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        // ��Ƽ ������ �������� ��û�� �� ���ƿ��� ���� ���
        private void CreatePartyProcess(Result result, Net.RecvPacket packet)
        {
            // ��Ƽ�� ���������� ���� ��
            if (result == Result.PartyCreated)
            {
                Debug.Log("��Ƽ ���� �Ϸ�");

                // ������ ���� �޾ƿ� , ������ ��Ƽ�� ����
                PlayerPartyInfo party_info;
                packet.ReadSerializable(out party_info);

                // ��Ƽ ���� �� �̺�Ʈ �ݹ�
                OnEnterPartyEvent?.Invoke(party_info);
            }
            else
            {
                Debug.Log("��Ƽ ���� ����");
            }
        }

        // ��Ƽ ��û�� ��� Ȯ��
        private void RequestParticipateProcess(Result result, Net.RecvPacket packet)
        {

        }

        // ���ο� ��Ƽ ������
        private void NewParticipantProcess(Result result, Net.RecvPacket packet)
        {

        }

        private void ExitProcess(Result result, Net.RecvPacket packet)
        {
            if (result == Result.ExitComplete)
            {
                Debug.Log("��Ƽ Ż�� ����");
                // ��Ƽ Ż�� �� �̺�Ʈ �ݹ�
                OnExitPartyEvent?.Invoke(); 
            }
            else
            {
                Debug.Log("��Ƽ Ż�� ����");
            }
        }
        private void KickProcess(Result result, Net.RecvPacket packet)
        {

        }
        private void TransferOwnerProcess(Result result, Net.RecvPacket packet)
        {

        }

        // ���� ������ �����ϴ� ��� ��Ƽ ������ ������
        private void AllPartyInfoProcess(Result result, Net.RecvPacket packet)
        {
            if (result == Result.NotExistParty)
            {   // ������ ��Ƽ�� �ƿ����� ���
                PartySystemGUIController.Instance.ClearShowList();
            }
            else if (result == Result.SuccessAllPartyInfo)
            {
                // party vector size + party vector data
                // size
                int size;
                packet.Read(out size);

                // data
                List<PlayerPartyInfo> m_partyInfo_list = new List<PlayerPartyInfo>(size);
                for (int i = 0; i < size; i++)
                {
                    PlayerPartyInfo info;
                    packet.ReadSerializable(out info);
                    m_partyInfo_list.Add(info);
                }
                Debug.Log("��Ƽ ���� ����");
                // �ҷ��� ��Ƽ ������ ���Ž�Ű��
                PartySystemGUIController.Instance.SetPartyList(m_partyInfo_list);
            }
        }

    }
}