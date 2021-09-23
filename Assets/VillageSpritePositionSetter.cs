using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSpritePositionSetter : MonoBehaviour
{
    SpriteRenderer m_renderer;

    private void Awake()
    {
        m_renderer = this.GetComponent<SpriteRenderer>();
        Debug.Log(m_renderer.size);
        Debug.Log( m_renderer.bounds.size);
    }
}
