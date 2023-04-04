using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    private float _positionEndX,_speedRunning,_speedWalking;
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
        if (!this.m_anim.enabled || m_isHit || isDeath || !CheckGroundBelow()) return;
        float speed = 0;
        if (this.m_isSeePlayer)
        {
            m_animCur = TagConst.A_Run;
            speed = _speedRunning;
        }
        else if (!m_isIdling)
        {
            speed = _speedWalking;
            m_animCur = TagConst.A_WALK;
            float posCur = transform.position.x;
            if ((m_nextDestination - transform.position.x) * m_direction < 0)
            {
                m_animCur = TagConst.A_IDLE;
                transform.position = new Vector3(posCur, transform.position.y, 0f);
                StartCoroutine(Idled());
            }
        }
        m_rg.velocity = new Vector2(speed * m_direction, m_rg.velocity.y);
        this.PlayAnim(m_animCur);
    }

    private bool CheckGroundBelow()
    {
        bool check = false;
        Vector2 startPos = transform.position + Vector3.left * width / 2;
        Vector2 endPos = startPos + Vector2.down;
        if (Physics2D.Linecast(startPos,endPos , LayerMask.GetMask("Ground"))
            .collider)
        {
            check = true;
        }else if (Physics2D.Linecast(startPos + Vector2.right * width,endPos + Vector2.right * width, LayerMask.GetMask("Ground"))
                  .collider)
        {
            check = true;
        }
        Debug.DrawLine(startPos, endPos,Color.blue);
        Debug.DrawLine(startPos + Vector2.right * width,endPos + Vector2.right * width,Color.blue);

        return check;
    }
    
    public override void OnHit(int damage)
    {
        this.m_isPlayerAttack = true;
        m_rg.velocity = GetVelocityHit();
        m_isHit = true;
        m_animCur = "Hit";
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
        transform.localScale = new Vector3(m_direction, transform.localScale.y, 0);
        m_nextDestination = m_posMaxX;
        if (m_direction < 0) m_nextDestination = m_posMinX;
    }
}
