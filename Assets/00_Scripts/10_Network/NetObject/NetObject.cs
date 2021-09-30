using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class NetObject : MonoBehaviour 
{
    // 처음 생성될 때 셋팅할 infomation
    abstract public void OnCreated(NetObjectInfo initialize_info);
    // 마지막에 파괴되기 전 처리할 것
    abstract public void BeforeDestroy();
    // 상속받은 클래스에서 멤버 변수로 정의할 infomation을 반환하는 function
    abstract public NetObjectInfo GetInfo();   
    abstract public void SetInfo(NetObjectInfo info);   
}

