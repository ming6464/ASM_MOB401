using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : Enemy
{
    [SerializeField] private PhysicsMaterial2D _highFriction;
    
    [SerializeField] private float _lengthRayBelow = 1,_lengthRayAhead = 0.1f,_speedRunning = 3.5f,_speedWalking = 1.5f;

    [SerializeField] private Transform _foot;
    
    [SerializeField] private LayerMask _layerGround,_layWall;
    
    
    private int m_direction;
    private bool m_isIdling;
    private float widthCol;

    protected override void Start()
    {
        base.Start();
        m_direction = 1;
        widthCol = Mathf.Abs(GetComponent<BoxCollider2D>().size.x * transform.localScale.x);
        UpdateDir();
        m_rg.sharedMaterial = _highFriction;

        if (!_foot) _foot = transform.Find("Foot");
    }
    

    protected override void Update()
    {
        if (!this.m_anim.enabled || m_isHit || isDeath) return;

        float speed = 0;
        
        if (UpdateDirAndCheckMove())
        {
            if (this.m_isSeePlayer && !_isBoss)
            {
                m_animCur = TagConst.A_Run;
                speed = _speedRunning;
            }else if (!m_isIdling)
            {
                speed = _speedWalking;
                m_animCur = TagConst.A_WALK;
            }
            transform.Translate(Vector3.right * m_direction * -1 * speed * Time.deltaTime);
        }
        
        this.PlayAnim(m_animCur);
    }

    private bool UpdateDirAndCheckMove()
    {
        Vector2 startPosNextBottom = _foot.position + Vector3.right * m_direction * widthCol / 2;
        Vector2 endPosNextBottom = startPosNextBottom + Vector2.down * _lengthRayBelow;
        
        Vector2 startPosPreBottom = startPosNextBottom - Vector2.right * m_direction * widthCol;
        Vector2 endPosPreBottom = startPosPreBottom + Vector2.down * _lengthRayBelow;

        Vector2 startPosNext = startPosNextBottom;
        Vector2 endPosNext = startPosNext + Vector2.right * m_direction * _lengthRayAhead;
        
        
        
        Debug.DrawLine(startPosNextBottom, endPosNextBottom,Color.blue);
        Debug.DrawLine(startPosPreBottom, endPosPreBottom,Color.red);
        Debug.DrawLine(endPosNext, startPosNext,Color.magenta);
        
        if (Physics2D.Linecast(startPosPreBottom, endPosPreBottom, _layerGround).collider)
        {
            if (!Physics2D.Linecast(startPosNext, endPosNext, _layWall).collider)
            {
                RaycastHit2D[] hits = Physics2D.LinecastAll(startPosNextBottom, endPosNextBottom)?.Where(x => 
                    (x.collider.CompareTag(TagConst.GROUND) || x.collider.CompareTag(TagConst.DEATHZONE))).ToArray();
                if (hits.Length > 0)
                {
                    if (!hits[0].collider.CompareTag(TagConst.DEATHZONE)) return true;
                }else if (this.m_isSeePlayer) return true;
            }
            ChangeDir();
            return false;
        }

        if (Physics2D.Linecast(startPosNext, endPosNext, _layWall).collider) return false;
        
        return true;
    }

    private void ChangeDir()
    {
        if (m_animCur == TagConst.A_IDLE) return;
        m_rg.velocity *= Vector2.up;
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
        m_rg.sharedMaterial = null;
        m_isHit = true;
        m_animCur = "Hit";
        this.PlayAnim(m_animCur);
    }
    private void EndHit()
    {
        m_rg.sharedMaterial = _highFriction;
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
    }
}
