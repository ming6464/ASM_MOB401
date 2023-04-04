using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField]
    private float _positionEndX,m_posEndY,_speedRunning,_speedWalking;
    private int m_direction;
    private float m_posMaxX,m_posMinX,m_posMinY,m_posMaxY,m_nextDestination;
    private bool m_isIdling,m_isRunning,m_isBehind;

    protected override void Start()
    {
        base.Start();
        m_posMinY = transform.position.y;
        m_posMaxY = m_posEndY;
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

        if (m_posMinY > m_posMaxY) (m_posMinY, m_posMaxY) = (m_posMaxY, m_posMinY);
        m_posMinY -= 1f;
        m_posMaxY += 1f;
        UpdateDir();

    }

    protected override void Update()
    {
        if (!this.m_anim.enabled) return;
        if (!isDeath && !m_isHit)
        {
            m_animCur = TagConst.A_IDLE;
            if (!m_isIdling)
            {
                AbilityUpdate();
                float speed = _speedWalking;
                m_animCur = TagConst.A_WALK;
                if (m_isRunning)
                {
                    speed = _speedRunning;
                    m_animCur = TagConst.A_Run;
                }
                m_rg.velocity = new Vector2(speed * m_direction, m_rg.velocity.y);
                float posCur = transform.position.x;
                if ((m_nextDestination - transform.position.x) * m_direction < 0)
                {
                    m_rg.velocity = new Vector2(0, 0);
                    transform.position = new Vector3(posCur, transform.position.y, 0f);
                    StartCoroutine(Idled());
                }
            }
        }
        this.PlayAnim(m_animCur);
    }

    public override void OnHit(int damage)
    {
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
            m_isRunning = false;
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

    void AbilityUpdate()
    {
        if (!this.m_player || m_isRunning) return;
        float x = this.m_player.transform.position.x;
        float y = this.m_player.transform.position.y;

        float valX = Mathf.Clamp(x, m_posMinX - 0.5f, m_posMaxX + 0.5f);
        float valY = Mathf.Clamp(y, m_posMinY, m_posMaxY);

        if (y == valY && x == valX)
        {
            int d = 1;
            if (x < transform.position.x) d = -1;
            if (d == m_direction) m_isRunning = true;
        }
    }
}
