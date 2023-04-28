using System;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    private SpriteRenderer m_playSR;
    private SpriteRenderer m_SR;
    private PlayerController m_player;
    private bool m_isRunTime;
    private Animator m_anim;

    private void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_isRunTime = false;
        m_SR = GetComponent<SpriteRenderer>();
        m_player = GameObject.FindWithTag(TagConst.PLAYER).GetComponent<PlayerController>();
        m_playSR = m_player.GetComponent<SpriteRenderer>();

        transform.position = m_player.transform.position;
        transform.rotation = m_player.transform.rotation;
        transform.localScale = m_player.transform.localScale;

        m_SR.sprite = m_playSR.sprite;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!m_player.isUsingSkill && !m_isRunTime)
        {
            m_isRunTime = true;
            if(!m_anim) m_anim = GetComponent<Animator>();
            m_anim.Play("Active");
        }
    }

    private void AddToPool()
    {
        AfterImagePool.Ins.AddToPool(transform.gameObject);
    }
}
