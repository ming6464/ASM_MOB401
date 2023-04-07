using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    private float _lengthRay,_positionEndX,_speedRunning,_speedWalking;
    private int m_direction;
    private float m_posMaxX,m_posMinX,m_nextDestination,m_passX;
    private bool m_isIdling, m_isBehind;
    private int m_countFrame,m_countStartFrame;
    private float width;

    protected override void Start()
    {
        base.Start();
        float curX = transform.position.x;
        
        if (curX < _positionEndX)
        {
            m_direction = 1;
            m_posMinX = transform.position.x;
            m_posMaxX = _positionEndX;
        }
        else
        {
            m_direction = -1;
            m_posMaxX = transform.position.x;
            m_posMinX = _positionEndX;
        }

        width = GetComponent<BoxCollider2D>().size.x;
        UpdateDir();

    }
    

    protected override void Update()
    {
        if (!this.m_anim.enabled || m_isHit || isDeath) return;
        if (CheckDeathZoneBelow()) ChangeDir();
        else
        {
            float speed = 0;
            if (this.m_isSeePlayer && !_isBoss)
            {
                m_animCur = TagConst.A_Run;
                speed = _speedRunning;
            }
            else if (!m_isIdling)
            {
                speed = _speedWalking;
                m_animCur = TagConst.A_WALK;
                if ((m_nextDestination - transform.position.x) * m_direction < 0) ChangeDir();
            }

            m_rg.velocity = new Vector2(speed * m_direction, m_rg.velocity.y);
        }
        this.PlayAnim(m_animCur);
    }

    private bool CheckDeathZoneBelow()
    {
        bool check = false;
        Vector2 startPos = transform.position + Vector3.left * width / 2;
        Vector2 endPos = startPos + Vector2.down * _lengthRay;
        if (Physics2D.Linecast(startPos,endPos , LayerMask.GetMask("DeathZone"))
            .collider)
        {
            if(m_direction < 0) check = true;
            
        }else if (Physics2D.Linecast(startPos + Vector2.right * width,endPos + Vector2.right * width, LayerMask.GetMask("DeathZone"))
                  .collider)
        {
            if(m_direction > 0) check = true;
        }
        Debug.DrawLine(startPos, endPos,Color.red);
        Debug.DrawLine(startPos + Vector2.right * width,endPos + Vector2.right * width,Color.blue);

        return check;
    }

    private void ChangeDir()
    {
        if (m_animCur == TagConst.A_IDLE) return;
        m_rg.velocity = Vector2.zero;
        m_animCur = TagConst.A_IDLE;
        StartCoroutine(Idled());
    }
    
    public override void OnHit(int damage)
    {
        if (_healthBar)
        {
            _healthBar.ChangeHealth(damage * -1);
            if (_healthBar.CheckOutOfHealth())
            {
                isDeath = true;
                m_anim.SetTrigger(TagConst.ParamDeath);
                m_rg.velocity = new Vector2(0f, 0f);
            }
        }

        if (_isBoss) return;
        this.m_isPlayerAttack = true;
        m_rg.velocity = GetVelocityHit();
        m_isHit = true;
        m_animCur = "Hit";
        this.PlayAnim(m_animCur);
    }
    private void EndHit()
    {
        m_isIdling = false;
        m_isHit = false;
        m_direction = FindDirPlayer();
        UpdateDir();
    }

    private IEnumerator Idled()
    {
        m_isIdling = true;
        yield return new WaitForSeconds(1.5f);
        if (m_isIdling)
        {
            m_isIdling = false;
            m_direction *= -1;
            UpdateDir();
        }
    }

    private void UpdateDir()
    {
        transform.localScale = new Vector3(m_direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        m_nextDestination = m_posMaxX;
        if (m_direction < 0) m_nextDestination = m_posMinX;
    }
}
