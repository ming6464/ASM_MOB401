using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Serializable]
    public class EffectParallax
    {
        public GameObject background;
        [Range(0.01f,0.06f)]
        public float speed;
    }
    [SerializeField] private EffectParallax[] _effectParallaxes;
    [SerializeField] private Transform _player;
    [SerializeField] private bool _isStartMap;
    private float m_passX, m_dx;
    private Material[] m_mat;
    private int BgCount;
    void Start()
    {
        m_passX = transform.position.x;
        if (_effectParallaxes != null)
        {
            BgCount = _effectParallaxes.Length;
            m_mat = new Material[BgCount];
            for (int i = 0; i < BgCount; i++)
            {
                m_mat[i] = _effectParallaxes[i].background.GetComponent<Renderer>().material;
            }
        }
    }

    private void LateUpdate()
    {
        if (_isStartMap)
        {
            for (int i = 0; i < BgCount; i++)
            {
                m_mat[i].mainTextureOffset += Time.deltaTime * _effectParallaxes[i].speed * Vector2.right;
            }

            return;
        }
        if (!_player) return;
        m_dx = 0;
        m_passX -= _player.position.x;
        if (m_passX < 0) m_dx = 1;
        else if (m_passX > 0) m_dx = -1;
        m_passX = _player.position.x;
        if (m_dx == 0) return;
        for (int i = 0; i < BgCount; i++)
        {
            m_mat[i].mainTextureOffset += Time.deltaTime * _effectParallaxes[i].speed * m_dx * Vector2.right;
        }
    }
}
