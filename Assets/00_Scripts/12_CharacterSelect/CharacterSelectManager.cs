using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetApp
{
    using Protocol = Net.CharacterSelectState.Protocol;
    using Result = Net.CharacterSelectState.Result;

    public class CharacterSelectManager : Singleton<CharacterSelectManager>
    {
        [SerializeField] GameObject BeforeGameStart;    // 게임 시작되기 전 사용할 오브젝트들의 최상단 object
        [SerializeField] GameObject InGame;             // 게임 시작 후 사용할 오브젝트들의 최상단 object

        [SerializeField] Image CharacterSelectPanel;

        private void Update()
        {
            CallbackCheck();
        }

        // -------------------- send process ----------------------//
        public void CharacterSelectedProcess(CharacterSelectInfo inCharInfo)
        {
            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();

            Protocol protocol = Protocol.CharacterSelect;
            sendpacket.Write(((Int64)protocol));
            sendpacket.Write(inCharInfo);

            Net.NetworkManager.Instance.Send(sendpacket);
        }
        public void SignOutSelectedProcess()
        {
            // TODO : through Sign Manager...
        }
        // -------------------- send process end ------------------//


        // -------------------- On Recv process ----------------------//
        private void CallbackCheck()
        {
            while (false == Net.CharacterSelectConstants.IsEmpty())
            {
                var char_data = Net.CharacterSelectConstants.Dequeue();
                switch (char_data.protocol)
                {
                    case Protocol.AllCharacterInfo:
                        OnAllCharacterInfo(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.CharacterSelect:
                        OnCharacterSelect(char_data.result, char_data.recvPacket);
                        break;
                    case Protocol.SignOut:
                        OnSignOut(char_data.result, char_data.recvPacket);
                        break;
                }
            }
        }

        // 키릭터 선택 화면에 들오왔을 경우
        // 현재 유저에 대한 모든 캐릭터를 서버로부터 불러옴
        private void OnAllCharacterInfo(Result result, Net.RecvPacket packet)
        {
            if (result == Result.NoData)
            {
                string msg;
                packet.Read(out msg);
                DebugConsoleGUIController.Instance.ShowMsg(msg);
            }
            else
            {
                List<CharacterSelectInfo> info_list = new List<CharacterSelectInfo>();
                packet.ReadSerializable(out info_list);

                CharacterSelectGUIController.Instance.SetCharacterInfomation(info_list);

                string msg;
                packet.Read(out msg);
                DebugConsoleGUIController.Instance.ShowMsg(msg);
            }
        }

        // 유저가 캐릭터 선택후 
        // 선택한 결과를 서버로부터 불러옴
        private void OnCharacterSelect(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                // 캐릭터 선택 완료
                // village state 로 이동 
                case Result.Success_CharacterSelect:
                    {
                        string msg;
                        packet.Read(out msg);
                        DebugConsoleGUIController.Instance.ShowMsg(msg);

                        //CharacterSelectPanel.gameObject.SetActive(false);
                        // 로그인, 캐릭터 선택 등에 쓰이던 모든 오브젝트를 비활성화 함
                        BeforeGameStart.SetActive(false);   
                        // 게임 내부에서 사용할 오브젝트를 활성화
                        InGame.SetActive(true);
                    }
                    break;
                // id 재확인 필요
                case Result.UndefinedCharacter:
                    {
                        string msg;
                        packet.Read(out msg);
                        DebugConsoleGUIController.Instance.ShowMsg(msg);
                    }
                    break;
            }
        }

        // sign in 하기 전 상태로 이동
        private void OnSignOut(Result result, Net.RecvPacket packet)
        {
            // not implement
        }
        // -------------------- On Recv process end-------------------//
    }

}