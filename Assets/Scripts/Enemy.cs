using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected HealthBar _healthBar;
    [SerializeField]
    private Vector3 _healthBarOffSet;
    [SerializeField] private float _maxHealth = 100;
    protected Animator m_anim;
    protected Rigidbody2D m_rg;
    protected bool isDeath,m_isHit;
    protected PlayerController m_player;
    protected string m_animPass, m_animCur;



    protected virtual void Start()
    {
        m_anim = GetComponent<Animator>();
        m_rg = GetComponent<Rigidbody2D>();
        ActiveAnimator(false);
        m_player = GameObject.FindWithTag(TagConst.PLAYER).GetComponent<PlayerController>();
        _healthBar.SetData(_maxHealth,_healthBarOffSet);
    }

    protected virtual void Update()
    {
    }

    protected Vector2 GetVelocityHit()
    {
        return Vector2.right * (1.2f - 0.45f) / 0.3f * FindDirPlayer() * -1;
    }

    protected int FindDirPlayer()
    {
        int d = 1;
        if ((m_player.transform.position.x - transform.position.x) < 0) d = -1;
        return d;
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
        GameObject gObj = col.gameObject;
        if (!GameManager.Ins.isOverGame && gObj.CompareTag(TagConst.CAM) && !m_anim.enabled) ActiveAnimator(true);
        if (gObj.CompareTag(TagConst.SWORD)) Hit();
        if(gObj.CompareTag(TagConst.PLAYER) && m_player) m_player.Hitted();
        if(gObj.CompareTag(TagConst.DEATHZONE)) Death();
    }
    
    public abstract void Hit();

}