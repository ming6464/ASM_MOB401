using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _player;
    private SpriteRenderer m_sp;
    private float m_passX, m_dx;
    void Start()
    {
        m_passX = transform.position.x;
        m_sp = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (!_player) return;
        m_dx = 0;
        m_passX -= _player.position.x;
        if (m_passX < 0) m_dx = 1;
        else if (m_passX > 0) m_dx = -1;
        m_passX = _player.position.x;
        m_sp.material.mainTextureOffset += new Vector2(m_dx * _speed * Time.deltaTime, 0);
    }
}
