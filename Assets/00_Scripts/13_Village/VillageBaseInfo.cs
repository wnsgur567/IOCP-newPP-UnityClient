using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

abstract class VillageBaseInfo
{
    public UInt32 m_village_id;
    public string m_village_name;
    public NetVector2Int m_pixel_size = null;

    // village 에 입장한 player container
    public Dictionary<UInt64, PlayerInfo> m_player_dic = null; 

    
    public virtual void __Initialize()
    {
        m_village_id = new uint();
        m_pixel_size = new NetVector2Int();
        m_player_dic = new Dictionary<ulong, PlayerInfo>();
    }
    public virtual void __Finalize()
    {
        m_player_dic.Clear();
    }
}
