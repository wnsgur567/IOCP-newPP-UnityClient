using System.Collections;
using System.Collections.Generic;
using Net;
using System;

namespace Net
{
    public class SignState : INetStateBase
    {
        NetSession session;
        INetStateBase.State m_state;
        public NetSession Owner
        {
            get => session;
            set { session = value; }
        }

        public INetStateBase.State SessionState { get => m_state; }

        public SignState(NetSession Owner)
        {
            m_state = INetStateBase.State.Sign;
            this.Owner = Owner;
        }

        public enum Protocol : UInt64
        {
            None = 0,

            // flags...
            SignIn = 1UL << 0,
            SignOut = 1UL << 1,
            SignUp = 1UL << 2,
            DeleteAccount = 1UL << 3,
        }

        public enum Result : UInt32
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
            // get protocol 
            Protocol protocol;            
            recvpacket.Read<UInt64, Protocol>(out protocol);

            // get sign result
            Result result;
            recvpacket.Read<UInt32, Result>(out result);

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

        
        public void OnChanged()
        { }


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