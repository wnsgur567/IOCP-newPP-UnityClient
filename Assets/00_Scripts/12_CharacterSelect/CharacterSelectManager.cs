using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetApp
{
    using Protocol = Net.CharacterSelectState.Protocol;
    using Result = Net.CharacterSelectState.Result;

    public class CharacterSelectManager : Singleton<CharacterSelectManager>
    {
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
            sendpacket.Write(((int)protocol));
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
            while (false == Net.CharacterSelectConstatnts.IsEmpty())
            {
                var char_data = Net.CharacterSelectConstatnts.Dequeue();
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
                packet.ReadSerializabel(out info_list);

                CharacterSelectGUIController.Instance.SetCharacterInfomation(info_list);

                string msg;
                packet.Read(out msg);
                DebugConsoleGUIController.Instance.ShowMsg(msg);
            }
        }

        private void OnCharacterSelect(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                case Result.Success_CharacterSelect:
                    break;
                case Result.UndefinedCharacter:
                    break;
            }
        }
        private void OnSignOut(Result result, Net.RecvPacket packet)
        {
            // not implement
        }
        // -------------------- On Recv process end-------------------//
    }

}