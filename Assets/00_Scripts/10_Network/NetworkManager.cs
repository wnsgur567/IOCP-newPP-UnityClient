#define __DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using System;
using System.Text;

namespace Net
{
    [DefaultExecutionOrder(-30)]
    public partial class NetworkManager : Singleton<NetworkManager>
    {
        public enum State
        {
            Error = -1,

            BeforeInitialize = 0,

            Connecting,
            Connected,

            Disconnecting,
            Disconnected,
        }

        private State m_state;

        private NetSession m_session;

        private bool m_thread_endFlag;
        private EventWaitHandle m_hSendEvent;
        private Thread m_net_main_thread;
        private Thread m_net_recv_thread;

        public State NetState { get { return m_state; } }


        private void Awake()
        {
            m_state = State.BeforeInitialize;
        }

        private void OnEnable()
        {
            __Initialize();
        }
        private void OnApplicationQuit()
        {
            __Finalize();
        }

        public void Log(string msg)
        {
            DebugConsoleGUIConstants.ShowMsg_Req(msg);
        }

        public void __Initialize()
        {
            m_state = State.Connecting;

            CipherManager.__Initialize();

            m_thread_endFlag = false;
            m_net_main_thread = new Thread(NetThread);
            m_net_main_thread.Start();
        }

        public void __Finalize()
        {
            m_state = State.Disconnecting;
            m_session.__Finalize();
            m_state = State.Disconnected;

            m_thread_endFlag = true;

            CipherManager.__Finalize();
        }


        public void Send(SendPacket sendpacket)
        {
            m_session.SendReq(sendpacket);
            m_hSendEvent.Set();
        }

        //public void Recv(out RecvPacket recvpacket)
        //{
        //    m_session.Recv(out recvpacket);
        //}


        void NetThread()
        {
#if __DEBUG
            Log("NetThread Start...");
#endif

            m_hSendEvent = new EventWaitHandle(false,EventResetMode.AutoReset);

            m_session = new NetSession();
            m_session.__Initialize();           

            m_state = State.Connected;

            m_net_recv_thread = new Thread(RecvThread);
            m_net_recv_thread.Start();
                        
            while (false == m_thread_endFlag)
            {   // send loop
                try
                {
                    m_hSendEvent.WaitOne();
                    // all sendpacket will be finished
                    // object that request send will be called by callback functions
                    m_session.SendQueueProcess();                    
                }
                catch (Exception)
                {

                    throw;
                }
            }

            m_net_recv_thread.Join();

            __Finalize();

#if __DEBUG
            Log("NetThread End...");            
#endif
        }
    }   
}

