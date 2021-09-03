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
        // -------------------- send process ----------------------//
        public void SignInProcess(string id, string pw)
        {
            Protocol protocol = Protocol.SignIn;
            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int32)protocol), sizeof(Protocol));
            sendpacket.Write(id);
            sendpacket.Write(pw);
        }

        public void SignOutProcess()
        {
            Protocol protocol = Protocol.SignOut;

            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int32)protocol), sizeof(Protocol));
        }

        public void SignUpProcess(string id, string pw)
        {
            Protocol protocol = Protocol.SignUp;

            Net.SendPacket sendpacket = new Net.SendPacket();
            sendpacket.__Initialize();


            // write to sendpacket's stream
            sendpacket.Write(BitConverter.GetBytes((Int32)protocol), sizeof(Protocol));
            sendpacket.Write(id);
            sendpacket.Write(pw);

            Net.NetworkManager.Instance.Send(sendpacket);
        }
        // -------------------- send process end ------------------//


        // -------------------- On Recv process ----------------------//
        public void OnSignIn(Result result, Net.RecvPacket packet)
        {
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

