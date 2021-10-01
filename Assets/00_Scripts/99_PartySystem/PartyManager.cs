using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NetApp
{
    using Protocol = Net.VillageState.Protocol;

    public class PartyManager : Singleton<PartyManager>
    {
        public enum Result : UInt32
        {
            None = 0,
        }

        // GUI 최상단 부모 object
        [SerializeField] Image m_partySystem_panel;
        [SerializeField] Image m_party_panel;
        [SerializeField] Image m_volunteer_panel;

        public void ShowPartySystem()
        {
            m_partySystem_panel.gameObject.SetActive(true);
        }
        public void UnShowPartySystem()
        {
            m_partySystem_panel.gameObject.SetActive(false);
        }
        public void ShowPartyInfo()
        {
            m_party_panel.gameObject.SetActive(true);
        }
        public void UnShowPartyInfo()
        {
            m_party_panel.gameObject.SetActive(false);
        }
        public void ShowVolunteer()
        {
            m_volunteer_panel.gameObject.SetActive(true);
        }
        public void UnShowVolunteer()
        {
            m_volunteer_panel.gameObject.SetActive(false);
        }

        private void Update()
        {
            CallbackCheck();
        }

        // -------------------- Send process ----------------------//
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

        // -------------------- Send process end ----------------------//


        // -------------------- On Recv process ----------------------//
        private void CallbackCheck()
        {
            while (false == Net.PartyConstants.IsEmpty())
            {
                var party_data = Net.PartyConstants.Dequeue();
                switch (party_data.protocol)
                {
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

        // 파티 신청의 결과 확인
        private void RequestParticipateProcess(Result result, Net.RecvPacket packet)
        {

        }

        // 새로운 파티 참가자
        private void NewParticipantProcess(Result result, Net.RecvPacket packet)
        {

        }

        private void ExitProcess(Result result, Net.RecvPacket packet)
        {

        }
        private void KickProcess(Result result, Net.RecvPacket packet)
        {

        }
        private void TransferOwnerProcess(Result result, Net.RecvPacket packet)
        {

        }

        // 현재 서버에 존재하는 모든 파티 정보를 가져옴
        private void AllPartyInfoProcess(Result result, Net.RecvPacket packet)
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

            // 불러온 파티 정보로 갱신시키기
            PartySystemGUIController.Instance.SetPartyList(m_partyInfo_list);
        }
    }
}