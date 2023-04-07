using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private HealthBar _healthBar;
    [SerializeField]
    private float _moveSpeed,_maxHealth = 100;
    private float m_directX, m_directY, m_passDirectX;
    private Rigidbody2D m_rg;
    private Vector2 m_velJump;
    private bool m_isJump, m_isLand, m_isJumpStart, m_isJumpEnd, m_isAttack, m_isHit, m_isDeath,m_isHasKey;
    private Animator m_anim;
    private string m_passAnim, m_curAnim,m_paramAttack,m_paramHit;
    void Start()
    {
        Data.Reset();
        m_rg = GetComponent<Rigidbody2D>();
        m_velJump = new Vector2(0, Mathf.Sqrt(50));
        m_anim = GetComponent<Animator>();
        _healthBar.SetData(_maxHealth,Vector3.zero,true);
        m_paramAttack = "isAttack";
        m_paramHit = "isHit";
        if(m_rg) m_rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(3,6,false);
    }
    
    void Update()
    {
        if (m_isDeath) return;
        m_directX = Input.GetAxisRaw("Horizontal");
        if (!m_isJumpStart && !m_isAttack)
        {
            if (m_directX != 0)
            {
                transform.Translate(Vector3.right * (_moveSpeed * Time.deltaTime * m_directX));
                if (m_directX != m_passDirectX)
                {
                    m_passDirectX = m_directX;
                    transform.localScale = new Vector3(m_directX, 1, 0);
                }

                if (!m_isJump && m_isLand)
                {
                    m_isJumpEnd = false;
                    m_curAnim = TagConst.A_Run;
                }
            }

            if (!m_isJumpEnd)
            {
                if (m_isLand && !m_isJump)
                {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        m_isJumpStart = true;
                        m_curAnim = TagConst.A_SPRINGY;
                        m_isJump = true;
                        m_isLand = false;
                    }else if(m_directX == 0) m_curAnim = TagConst.A_IDLE;
                }
                else if (!m_isLand)
                {
                    if (m_rg.velocity.y < 0)
                    {
                        m_curAnim = TagConst.A_Fall;
                        m_isLand = false;
                    }else if (m_rg.velocity.y > 0)
                    {
                        m_curAnim = TagConst.A_JUMP;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && !m_isJumpStart)
            {
                m_isAttack = true;
                m_anim.SetTrigger(m_paramAttack);
                if (m_rg.velocity.y != 0)
                {
                    m_rg.velocity = Vector2.zero;
                    m_curAnim = TagConst.A_Fall;
                }
                PlayAudio(TagConst.AUDIO_HIT);
            }
        }
        
        PlayAnim(m_curAnim);
    }

    public void PlayAnim(string anim)
    {
        if (m_passAnim != anim)
        {
            m_passAnim = anim;
            m_anim.Play(anim);
        }
    }

    private void Jumped()
    {
        m_rg.velocity = m_velJump;
        m_isJumpStart = false;
        PlayAudio(TagConst.AUDIO_JUMP1);
    }
    
    private void EndAttack()
    {
        m_anim.ResetTrigger(m_paramAttack);
        m_isAttack = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.GROUND))
        {
            if (m_curAnim != TagConst.A_Fall) return;
            m_isJump = false;
            m_isJumpEnd = true;
            m_curAnim = TagConst.A_Landing;
            PlayAnim(m_curAnim);
            m_isLand = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.GROUND)) m_isLand = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj.CompareTag(TagConst.GROUND)) m_isLand = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.FINISH) && m_isHasKey)
        {
            PlayAudio(TagConst.AUDIO_WIN);
            GameManager.Ins.StateGame(true);
        }
        else if (gObj.CompareTag(TagConst.DEATHZONE)) End();
        if (gObj.CompareTag(TagConst.KEY))
        {
            UIManager.Ins.ShowKey();
            m_isHasKey = true;
            Destroy(gObj);
        }
    }


    public void OnHit(int damage)
    {
        if (m_isHit) return;
        m_isHit = true;
        m_anim.SetBool(m_paramHit,m_isHit);
        Physics2D.IgnoreLayerCollision(3,6);
        StartCoroutine(CountDownResetHit());
        ChangeHealth(damage * -1);
    }

    private IEnumerator CountDownResetHit()
    {
        yield return new WaitForSeconds(2);
        m_isHit = false;
        m_anim.SetBool(m_paramHit,m_isHit);
        Physics2D.IgnoreLayerCollision(3,6,false);
    }
    private void Landed()
    {
        PlayAudio(TagConst.AUDIO_JUMP2);
        m_isJumpEnd = false;
    }

    public void ChangeHealth(float val)
    {
        _healthBar.ChangeHealth(val);
        m_isDeath = _healthBar.CheckOutOfHealth();
        if (m_isDeath) End();
    }

    private void End()
    {
        m_isDeath = true;
        m_anim.ResetTrigger(m_paramAttack);
        m_anim.SetBool(m_paramHit,false);
        m_anim.SetTrigger(TagConst.ParamDeath);
        m_rg.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Death()
    {
        GameManager.Ins.StateGame(false);
    }

    private void PlayAudio(string name)
    {
        AudioManager.Ins.PlayAudio(name,true);
    }
}
