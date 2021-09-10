using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;
using System;

namespace Net
{
    public class SignState : INetStateBase
    {
        NetSession session;
        INetStateBase.State m_state;
        public NetSession Owner { 
            get => session; 
            set { session = value; }
        }

        public INetStateBase.State SessionState { get => m_state; }

        public SignState(NetSession Owner)
        {
            m_state = INetStateBase.State.Sign;
            this.Owner = Owner;
        }

        public enum Protocol : Int64
        {
            None = 0,

            // flags...
            SignIn = 1 << 0,
            SignOut = 1 << 1,
            SignUp = 1 << 2,
            DeleteAccount = 1 << 3,
        }

        public enum Result : Int32
        {
            None = 0,

            // start 1000
            ExistID = 1001,
            NotExistID,
            WrongPW,

            Success_SingIn,
            Success_SignOut,
            Success_SignUp,
            Success_DeleteAccount,
        }


        public void OnRecvComplete(RecvPacket recvpacket)
        {
            // unpack recvpacket data
            byte[] protocol_bytes;
            recvpacket.Read(out protocol_bytes, sizeof(Protocol));
            Protocol protocol = (Protocol)BitConverter.ToInt64(protocol_bytes, 0);

            byte[] result_bytes;
            recvpacket.Read(out result_bytes, sizeof(Result));
            Result result = (Result)BitConverter.ToInt32(result_bytes, 0);

            // call functions by protocol
            switch (protocol)
            {
                case Protocol.SignIn:
                case Protocol.SignOut:
                case Protocol.SignUp:
                case Protocol.DeleteAccount:
                    SignConstants.CallbackReq(protocol, result, recvpacket);
                    break;

                default:
                    // throw exception
                    break;
            }

            switch (result)
            {
                case Result.Success_SingIn:
                    Owner.IsSignedIn = true;
                    Owner.ChangeState(Owner.m_charselect_state);
                    DebugConsoleGUIConstants.ShowMsg_Req("Change State!!");                    
                    break;
                case Result.Success_SignOut:
                    Owner.IsSignedIn = false;
                    break;
                case Result.Success_DeleteAccount:
                    Owner.IsSignedIn = false;
                    break;

                default:

                    break;
            }            
        }

        public void OnSendComplete()
        {

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