using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;
using System;

namespace Net
{
    public class SignState : NetStateBase
    {
        internal bool IsSignedIn;

        NetStateBase.State m_state;

        public NetStateBase.State SessionState { get => m_state; }

        public SignState()
        {
            IsSignedIn = false;
            m_state = NetStateBase.State.Sign;
        }

        public enum Protocol : Int64
        {
            None = 0,

            SignIn = 1 << 0,
            SignOut = 1 << 1,
            SignUp = 1 << 2,
            DeleteAccount = 1 << 3,
        }

        public enum Result : Int32
        {
            None = 0,

            ExistID = 1001,
            NotExistID,
            WrongPW,

            Success_SingIn,
            Success_SignOut,
            Success_SignUp,
            Success_DeleteAccount,
        }


        public object OnRecvComplete(RecvPacket recvPacket)
        {
            // unpack recvpacket data
            byte[] protocol_bytes;
            recvPacket.Read(out protocol_bytes, sizeof(Protocol));
            Protocol protocol = (Protocol)BitConverter.ToInt64(protocol_bytes, 0);

            byte[] result_bytes;
            recvPacket.Read(out result_bytes, sizeof(Result));
            Result result = (Result)BitConverter.ToInt32(result_bytes, 0);

            // call functions by protocol
            switch (protocol)
            {
                case Protocol.SignIn:
                    NetApp.SignManager.Instance.OnSignIn(result, recvPacket);
                    break;
                case Protocol.SignOut:
                    NetApp.SignManager.Instance.OnSignOut(result, recvPacket);
                    break;
                case Protocol.SignUp:
                    NetApp.SignManager.Instance.OnSignUp(result, recvPacket);
                    break;
                case Protocol.DeleteAccount:
                    NetApp.SignManager.Instance.OnDeleteAccount(result, recvPacket);
                    break;

                default:
                    // throw exception
                    break;
            }

            return result;
        }

        public void OnSendComplete()
        {
            // when send completed,
            // recv result from SERVER
            RecvPacket recvPacket;
            NetworkManager.Instance.Recv(out recvPacket);
        }


        #region Exception

        public void OccuredRecvException()
        {

        }

        public void OccuredSendException()
        {

        }

        #endregion
    }
}