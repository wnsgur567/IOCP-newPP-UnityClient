using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        Debug.Log("thread Á¤¸®");
        Net.NetworkManager.Instance.__Finalize();
    }
}
