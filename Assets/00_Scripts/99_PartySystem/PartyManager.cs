using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace NetApp
{
    using Protocol = Net.VillageState.Protocol;

    // 네트워크 스레드에서 받은 패킷을 분석하여 
    // 해당 상황에 맞는 프로세스를 진행하도록 뿌리는 역활
    public class PartyManager : Singleton<PartyManager>
    {
        public enum Result : UInt32
        {
            None = 0,

            PartyCreated = 1U << 0,            // 파티 생성 완료

            RequestChecking = 1U << 1,     // 파티 참가 신청 완료,
            RequestAccept = 1U << 2,           // 파티 참가 신청을 파티장이 수락
            RequestReject = 1U << 3,           // 파티 참가 신청을 파티장이 거부

            ExitComplete = 1U << 4,            // 파티 퇴장 완료
            ExitCompleteOther = 1U << 5,       // 파티원 중 한명이 파티를 퇴장

            Kicked = 1U << 6,               // 강퇴 당함
            KickedOther = 1U << 7,          // 파티원 중 한명이 강퇴당함				

            NotExistParty = 1U << 8,			// 존재하지 않는 파티
            SuccessAllPartyInfo = 1U << 9,		// 모든 파티 정보 검색 성공
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

        // -------------------- Send process end ----------------------//


        // -------------------- On Recv process ----------------------//
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

        // 파티 생성을 서버에게 요청한 후 돌아오는 생성 결과
        private void CreatePartyProcess(Result result, Net.RecvPacket packet)
        {
            // 파티가 성공적으로 생성 됨
            if (result == Result.PartyCreated)
            {
                Debug.Log("파티 생성 완료");

                PlayerPartyInfo party_info;
                packet.ReadSerializable(out party_info);
                // 플레이어의 파티 정보 갱신
                ClientGameInfoManager.Instance.SetParty(party_info);
                // 현재 플레이 중인 플레이어의 정보
                PlayerInfo cur_playerinfo = ClientGameInfoManager.Instance.ControllPlayerInfo;
                // UI 상단 파티원 정보 창 갱신
                PartyPlayersGUIController.Instance.SetInfo(0, cur_playerinfo);
                PartyPlayersGUIController.Instance.Activate();
            }
            else
            {
                Debug.Log("파티 생성 실패");
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
            if (result == Result.ExitComplete)
            {
                Debug.Log("파티 탈퇴 성공");

                // 현재 플레이어의 파티 정보를 갱신
                ClientGameInfoManager.Instance.SetParty(null);
                // UI 상단 파티원 정보 창 끄기
                PartyPlayersGUIController.Instance.DeActivate();
                // 파티 정보 창 열려있다면 끄기
                PartyGUIController.Instance.DeActivate();
            }
            else
            {
                Debug.Log("파티 탈퇴 실패");
            }
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
            if (result == Result.NotExistParty)
            {   // 생성된 파티가 아예없는 경우
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
                Debug.Log("파티 정보 갱신");
                // 불러온 파티 정보로 갱신시키기
                PartySystemGUIController.Instance.SetPartyList(m_partyInfo_list);
            }
        }

    }
}