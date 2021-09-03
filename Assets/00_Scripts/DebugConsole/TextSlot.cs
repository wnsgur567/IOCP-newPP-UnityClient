using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSlot : MonoBehaviour
{    
    [SerializeField] TMPro.TextMeshProUGUI m_text;

    public void SetText(string msg)
    {
        m_text.text = msg;
    }
}
