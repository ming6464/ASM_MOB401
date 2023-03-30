using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : Singleton<BackgroundScroll>
{
    [SerializeField] private float _speed;
    private SpriteRenderer m_sp;
    public override void Awake()
    {
        
    }

    public override void Start()
    {
        m_sp = GetComponent<SpriteRenderer>();
    }

    public void Scrolling(float dx)
    {
        if (m_sp)
        {
            float speed = _speed * Time.deltaTime;
            m_sp.material.mainTextureOffset += new Vector2(dx * speed, 0);
        }
    }
}
