using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    private float m_directX, m_directY, m_passDirectX;
    private Rigidbody2D m_rg;
    private Vector2 m_velJump;
    private bool m_isJump, m_isLand,m_isJumpStart,m_isJumpEnd,m_isAttack,m_isHit;
    private Animator m_anim;
    private string m_passAnim, m_curAnim;
    void Start()
    {
        m_isLand = true;
        m_rg = GetComponent<Rigidbody2D>();
        m_velJump = new Vector2(0, Mathf.Sqrt(50));
        m_anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        m_directX = Input.GetAxisRaw("Horizontal");
        if (!m_isJumpStart && !m_isAttack)
        {
            if (m_directX != 0)
            {
                transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime * m_directX);
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
                        m_curAnim = TagConst.A_JUMP;
                        m_isJump = true;
                        m_isLand = false;
                    }else if(m_directX == 0) m_curAnim = TagConst.A_IDLE;
                }
                else if (!m_isLand && m_rg.velocity.y < 0)
                {
                    m_curAnim = TagConst.A_Fall;
                    m_isLand = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !m_isAttack && !m_isJumpStart)
            {
                m_isAttack = true;
                m_rg.velocity = Vector2.zero;
                m_anim.SetTrigger("isAttack");
            }
            PlayAnim(m_curAnim);
        }
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
    }

    private void EndAttack()
    {
        m_anim.ResetTrigger("isAttack");
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

    private void OnCollisionExit2D(Collision2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj.CompareTag(TagConst.GROUND)) m_isLand = false;
    }
    
    public void PlayAnimHit()
    {
        if (m_isHit) return;
        m_isHit = true;
        m_anim.SetBool("isHit",m_isHit);
        Physics2D.IgnoreLayerCollision(3,6);
        StartCoroutine(CountDownResetHit());
    }

    private IEnumerator CountDownResetHit()
    {
        yield return new WaitForSeconds(2);
        m_isHit = false;
        m_anim.SetBool("isHit",m_isHit);
        Physics2D.IgnoreLayerCollision(3,6,false);
    }

    private void Landed()
    {
        m_isJumpEnd = false;
    }
}
