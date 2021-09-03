using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net
{
    public interface NetStateBase
    {
        public void OnRecved(RecvPacket recvpacket);
        public void OnSended();

        public void OccuredRecvException();
        public void OccuredSendException();

    }
}
