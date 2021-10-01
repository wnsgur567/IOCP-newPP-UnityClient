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
        [SerializeField] GameObject BeforeGameStart;    // ���� ���۵Ǳ� �� ����� ������Ʈ���� �ֻ�� object
        [SerializeField] GameObject InGame;             // ���� ���� �� ����� ������Ʈ���� �ֻ�� object

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

        // Ű���� ���� ȭ�鿡 ������� ���
        // ���� ������ ���� ��� ĳ���͸� �����κ��� �ҷ���
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

        // ������ ĳ���� ������ 
        // ������ ����� �����κ��� �ҷ���
        private void OnCharacterSelect(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                // ĳ���� ���� �Ϸ�
                // village state �� �̵� 
                case Result.Success_CharacterSelect:
                    {
                        string msg;
                        packet.Read(out msg);
                        DebugConsoleGUIController.Instance.ShowMsg(msg);

                        //CharacterSelectPanel.gameObject.SetActive(false);
                        // �α���, ĳ���� ���� � ���̴� ��� ������Ʈ�� ��Ȱ��ȭ ��
                        BeforeGameStart.SetActive(false);   
                        // ���� ���ο��� ����� ������Ʈ�� Ȱ��ȭ
                        InGame.SetActive(true);
                    }
                    break;
                // id ��Ȯ�� �ʿ�
                case Result.UndefinedCharacter:
                    {
                        string msg;
                        packet.Read(out msg);
                        DebugConsoleGUIController.Instance.ShowMsg(msg);
                    }
                    break;
            }
        }

        // sign in �ϱ� �� ���·� �̵�
        private void OnSignOut(Result result, Net.RecvPacket packet)
        {
            // not implement
        }
        // -------------------- On Recv process end-------------------//
    }

}