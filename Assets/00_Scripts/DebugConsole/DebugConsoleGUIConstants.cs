using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsoleGUIConstants
{
    public struct MsgData
    {
        public string msg;
        public float delay;
    }

    // for other thread (except unity main thread)
    public static Queue<MsgData> m_msg_queue = new Queue<MsgData>();

    // for other thread 
    public static void ShowMsg_Req(string msg_text, float delay = 10.0f)
    {   // this msg will be deal with 'update' in DebugConsoleGUIController
        m_msg_queue.Enqueue(new MsgData() { msg = msg_text, delay = delay });
    }
}
