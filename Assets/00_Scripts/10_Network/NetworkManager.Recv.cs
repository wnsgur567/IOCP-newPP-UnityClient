#define __DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Net
{

    public partial class NetworkManager
    {

        const int NETFRAME_PER_SEC = 3;     // 1 second 3 frame
        //const float NETFRAME_RATIO = 1.0f / NETFRAME_PER_SEC;
        const int LOOP_WAITING_TICKS = (int)((float)1000 / NETFRAME_PER_SEC);

        //private float m_befroeFrameTime_recvloop;
        private long m_beforeFrameTick;
        private void RecvThread()
        {
#if __DEBUG
            Log("RecvThread Start...");            
#endif

            var now = DateTime.Now;
            m_beforeFrameTick = now.Ticks;

            long tick_span;
            while (false == m_thread_endFlag)
            {   // recv loop
                try
                {
                    while (m_session.m_netstream.DataAvailable)
                    {   // until recv buffer is empty
                        m_session.Recv();
                    }

                    // time sync
                    now = DateTime.Now;
                    tick_span = now.Ticks - m_beforeFrameTick;
                    m_beforeFrameTick = now.Ticks;

                    // time sync delay
                    if (tick_span < LOOP_WAITING_TICKS)
                    {
                        int delay = Convert.ToInt32(LOOP_WAITING_TICKS - tick_span);
                        Thread.Sleep(delay);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
#if __DEBUG
            Log("RecvThread End...");            
#endif
        }
    }
}
