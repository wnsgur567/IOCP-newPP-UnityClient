using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NetApp
{
    using Protocol = Net.SignState.Protocol;
    using Result = Net.SignState.Result;

    public class SignManager : Singleton<SignManager>
    {
        private void Update()
        {
            CallbackCheck();
        }

        // -------------------- send process ----------------------//
        public void SignInProcess(string id, string pw)
        {
            Protocol protocol = Protocol.SignIn;
            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int64)protocol), sizeof(Protocol));
            sendpacket.Write(id);
            sendpacket.Write(pw);

            Net.NetworkManager.Instance.Send(sendpacket);
        }

        public void SignOutProcess()
        {
            Protocol protocol = Protocol.SignOut;

            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int64)protocol), sizeof(Protocol));
        }

        public void SignUpProcess(string id, string pw)
        {
            Protocol protocol = Protocol.SignUp;

            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int64)protocol), sizeof(Protocol));
            sendpacket.Write(id);
            sendpacket.Write(pw);

            Net.NetworkManager.Instance.Send(sendpacket);
        }
        // -------------------- send process end ------------------//


        // -------------------- On Recv process ----------------------//
        private void CallbackCheck()
        {
            // sign recv callback queue check
            while (false == Net.SignConstants.IsEmpty())
            {
                var sign_data = Net.SignConstants.Dequeue();
                switch (sign_data.protocol)
                {
                    case Protocol.SignIn:
                        OnSignIn(sign_data.result, sign_data.recvPacket);
                        break;
                    case Protocol.SignOut:
                        OnSignOut(sign_data.result, sign_data.recvPacket);
                        break;
                    case Protocol.SignUp:
                        OnSignUp(sign_data.result, sign_data.recvPacket);
                        break;
                    case Protocol.DeleteAccount:
                        OnDeleteAccount(sign_data.result, sign_data.recvPacket);
                        break;
                }
            }
        }

        public void OnSignIn(Result result, Net.RecvPacket packet)
        {
            byte[] msglen_bytes;
            packet.Read(out msglen_bytes, sizeof(int));
            int msg_len = BitConverter.ToInt32(msglen_bytes, 0);

            byte[] msg_bytes;
            packet.Read(out msg_bytes, msg_len);
            string msg = Encoding.Unicode.GetString(msg_bytes);

            DebugConsoleGUIController.Instance.ShowMsg(msg);

            switch (result)
            {
                case Result.NotExistID:
                    break;
                case Result.WrongPW:
                    break;
                case Result.Success_SingIn:
                    break;

                default:
                    // exception
                    break;
            }
        }
        public void OnSignOut(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                case Result.Success_SignOut:
                    break;

                default:
                    // exception
                    break;
            }
        }
        public void OnSignUp(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                case Result.ExistID:
                    break;
                case Result.Success_SignUp:
                    break;

                default:
                    // exception
                    break;
            }
        }

        public void OnDeleteAccount(Result result, Net.RecvPacket packet)
        {
            switch (result)
            {
                case Result.WrongPW:
                    break;
                case Result.Success_DeleteAccount:
                    break;

                default:
                    // exception
                    break;
            }
        }

        // -------------------- On Recv process end-------------------//
    }
}

