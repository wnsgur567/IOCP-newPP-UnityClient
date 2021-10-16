using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IPartyInOutCallback
{
    // 파티에 참가한 경우 호출될 callback
    abstract void OnEnterParty(PlayerPartyInfo info);

    // 파티에서 탈퇴할 경우 호출될 callback
    abstract void OnExitParty();
}

