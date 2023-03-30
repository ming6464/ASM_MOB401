using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator m_anim;
    protected Rigidbody2D m_rg;
    protected bool isDeath;
    protected PlayerController m_player;
    protected string m_animPass, m_animCur;
    
    protected virtual void Start()
    {
        m_anim = GetComponent<Animator>();
        m_rg = GetComponent<Rigidbody2D>();
        ActiveAnimator(false);
        m_player = GameObject.FindWithTag(TagConst.PLAYER).GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
    }

    protected void PlayAnim(string anim)
    {
        if (anim != m_animPass)
        {
            m_anim.Play(anim);
            m_animPass = anim;
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
    public void ActiveAnimator(bool isActive)
    {
        m_anim.enabled = isActive;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameManager.Ins.isOverGame && col.gameObject.CompareTag(TagConst.CAM) && !m_anim.enabled) ActiveAnimator(true);
        if (col.gameObject.CompareTag(TagConst.SWORD))
        {
            isDeath = true;
            m_anim.SetTrigger(TagConst.ParamDeath);
            m_rg.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(TagConst.PLAYER) && m_player) m_player.PlayAnimHit();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(TagConst.PLAYER) && m_player) m_player.PlayAnimHit();
    }
}