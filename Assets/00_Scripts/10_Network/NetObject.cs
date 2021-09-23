using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class NetObject : MonoBehaviour
{    
    abstract public void OnCreated(NetObjectInfo initialize_info);
    abstract public void BeforeDestroy();
}

