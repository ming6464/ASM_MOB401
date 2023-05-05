using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private HealthBar _healthBar;
    [SerializeField]
    private float _moveSpeed = 4,_maxHealth = 150,_lengthRayAhead = 0.2f;

    [SerializeField] private LayerMask _layerWall,_layerGround;
    
    [SerializeField] private PhysicsMaterial2D _highFriction;

    [SerializeField] private GameObject _foot;

    [SerializeField] private BladePlayer _bladePlayer;

    [SerializeField] private SkillManager _skillManager;

    [SerializeField] private GameObject _effectBloodPlayer;

    public bool m_canAttack,m_isAttack,m_isJumpStart,m_isJump,m_isLand,isUsingSkill,isActiveSkillG,isActiveSkillF;

    private float m_directX, m_directY, m_passDirectX, m_attackAnimationDuration, m_jumpAnimationDuration;
    private Rigidbody2D m_rg;
    private Vector2 m_velJump;
    private bool m_isHit, m_isDeath, m_isHasKey, m_isMove;
    private Animator m_anim;
    private string m_passAnim, m_curAnim,m_nextAttack;
    private BoxCollider2D m_boxColl,m_boxCollFoot;
    private RaycastHit2D m_raySlope;


    void Start()
    {
        Data.Reset();
        m_rg = GetComponent<Rigidbody2D>();
        m_velJump = new Vector2(0, Mathf.Sqrt(50));
        m_anim = GetComponent<Animator>();
        m_passDirectX = 1;
        Physics2D.IgnoreLayerCollision(3,6,false);
        
        //HealthBar{
        if (!_healthBar) _healthBar = FindObjectOfType<HealthBar>();
        _healthBar.SetData(_maxHealth,Vector3.zero,true);
        //HealthBar}
        
        //High Friction{
        if(!_highFriction) _highFriction = Resources.Load<PhysicsMaterial2D>(TagConst.URL_MATERIALS + "HighFriction");
        m_rg.sharedMaterial = _highFriction;
        //High Friction}
        
        //foot{
        if (!_foot) _foot = transform.Find("Foot").gameObject;
        m_boxColl = GetComponent<BoxCollider2D>();
        m_boxCollFoot = _foot.GetComponent<BoxCollider2D>();
        //foot}
        if (!_skillManager) _skillManager = FindObjectOfType<SkillManager>();
        
        //Blade
        if (!_bladePlayer) _bladePlayer = Resources.Load<BladePlayer>(TagConst.URL_PREFABS + "BladePlayer");
        
        //Effect blood
        if (!_effectBloodPlayer)
            _effectBloodPlayer = Resources.Load<GameObject>(TagConst.URL_PREFABS + "EffectBloodPlayer");
    }
    
    void Update()
    {
        if (GameManager.Ins.isShowTutorial || m_isDeath || isUsingSkill) return;

        //Draw Ray{
        CheckWallAhead();
        UpdatePosSlope();
        //Draw Ray}
        
        m_directX = Input.GetAxisRaw("Horizontal");
        if (!m_isJumpStart)
        {
            if (!m_isAttack)
            {
                OnMove();

                OnJump();
            }
            HandleAttack();
        }

        HandleUseSkillF();
        HandleUseSkillG();
        
        PlayAnim(m_curAnim);
    }

    //UseSkillF{
    private void HandleUseSkillF()
    {
        if (!isActiveSkillF || isUsingSkill) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            isUsingSkill = true;
        
            m_rg.sharedMaterial = null;

            m_rg.velocity = Vector2.zero;
            
            m_curAnim = TagConst.A_SKILLF;
            
            _skillManager.UseSkill(TagConst.Skill.F);
            
            Immortalize();
        }
    }

    private void OnStartSkill()
    {
        ShowAfterImg();
        PlayAudio(TagConst.AUDIO_ATTACK);
    }

    private void ContinuesSkill()
    {
        if(!CheckWallAhead()) transform.Translate(Vector3.right * m_boxColl.size.x * 1.5f * m_passDirectX);
        UpdatePosSlope();
        ShowAfterImg();
        PlayAudio(TagConst.AUDIO_SKILLF);
    }
    
    private void EndSkill()
    {
        if (m_curAnim is TagConst.A_SKILLF)
        {
            ContinuesSkill();
            if(!CheckWallAhead()) transform.Translate(Vector3.right * m_boxColl.size.x * 1.5f * m_passDirectX);
            UpdatePosSlope();
            StartCoroutine(RemoveImmortalityAfterDelay(1.5f));
        }
        else RunAndShowBlade();

        m_rg.sharedMaterial = _highFriction;
        isUsingSkill = false;
        m_isAttack = false;
        m_isJumpStart = false;
        m_isJump = false;
    }
    
    IEnumerator RemoveImmortalityAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        RemoveImmortality();
    }

    private void Immortalize()
    {
        m_anim.SetBool(TagConst.ParamImmortal,true);
        Physics2D.IgnoreLayerCollision(3,6);
    }

    private void RemoveImmortality()
    {
        m_anim.SetBool(TagConst.ParamImmortal,false);
        Physics2D.IgnoreLayerCollision(3,6,false);
    }
    //UseSkillF}
    
    
    //UseSkillG{

    private void HandleUseSkillG()
    {
        if (!isActiveSkillG  || isUsingSkill) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            isUsingSkill = true;
            
            m_curAnim = TagConst.A_SKILLG;
            
            m_rg.sharedMaterial = null;

            m_rg.velocity = Vector2.zero;
            
            _skillManager.UseSkill(TagConst.Skill.G);
        }
    }
    
    
    private void RunAndShowBlade()
    {
        BladePlayer newBlade = Instantiate(_bladePlayer, transform.position, Quaternion.identity);
        newBlade.Run(m_passDirectX);
        PlayAudio(TagConst.AUDIO_SKILLG);
    }
    //UseShillG}

    private RaycastHit2D GetRayCastGroundAbove()
    {
        Vector3 endPos = _foot.transform.position;
        Vector3 startPos = isUsingSkill ? new Vector3(endPos.x, transform.position.y, 0) : endPos + Vector3.up * 0.2f;
        Debug.DrawLine(startPos,endPos,Color.red);
        return Physics2D.Linecast(startPos, endPos, _layerGround);
    }

    private void UpdatePosSlope()
    {

        m_raySlope = GetRayCastGroundAbove();
        if (m_raySlope.collider)
        {
            Vector3 posCur = transform.position;
            float distanceBodyAndFoot = posCur.y - _foot.transform.position.y;
            transform.position = new Vector3(posCur.x, m_raySlope.point.y + distanceBodyAndFoot, posCur.z);
        }
    }

    private void ShowAfterImg()
    {
        AfterImagePool.Ins.GetFromPool();
    }

    //Attack{

    private void HandleAttack()
    {
        if (m_isAttack)
        {
            AnimatorStateInfo animationStateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
            
            if (animationStateInfo.normalizedTime >= 1.0f)
            {
                if (m_curAnim is TagConst.A_ATTACK_1 or TagConst.A_ATTACK_2 or TagConst.A_ATTACK_3)
                {
                    m_rg.sharedMaterial = _highFriction;
                    m_isAttack = false;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_canAttack = true;
            if (m_isAttack) return;
            m_nextAttack = TagConst.A_ATTACK_1;
            m_isAttack = true;
            m_canAttack = false;
            OnAttack();
        }
        
    }
    
    private void OnAttack()
    { 
        m_rg.sharedMaterial = null;
        m_rg.velocity = Vector2.right * 0.45f/ 0.3f * m_passDirectX;
        m_curAnim = m_nextAttack;
        m_nextAttack = m_nextAttack == TagConst.A_ATTACK_1 ? TagConst.A_ATTACK_2 :
            m_nextAttack == TagConst.A_ATTACK_2 ? TagConst.A_ATTACK_3 : TagConst.A_ATTACK_1;
        PlayAudio(TagConst.AUDIO_ATTACK);
    }

    private void EndAttack()
    {
        if (m_canAttack)
        {
            if(!CheckWallAhead()) transform.Translate(Vector3.right * (_moveSpeed * Time.deltaTime * m_passDirectX));
            UpdatePosSlope();
            m_canAttack = false;
            OnAttack();
        }
    }

    //Attack}

    private void OnMove()
    {
        if (m_directX != 0)
        {
            if(!CheckWallAhead()) transform.Translate(Vector3.right * (_moveSpeed * Time.deltaTime * m_directX));
            if (m_directX != m_passDirectX)
            {
                m_passDirectX = m_directX;
                transform.localScale = new Vector3(m_directX, 1, 0);
            }

            if (!m_isJump && m_isLand)
            {
                m_curAnim = TagConst.A_Run;
            }
        }
    }

    private bool CheckWallAhead()
    {
        float m_widthColFoot = m_boxCollFoot.size.x;
        float offSetFootX = m_boxCollFoot.offset.x;
        bool check = true;
        Vector3 startPos = _foot.transform.position + Vector3.right *(m_widthColFoot/2 + offSetFootX) * m_passDirectX;
        Vector3 endPos = startPos + Vector3.right * _lengthRayAhead * m_passDirectX;

        if (!Physics2D.Linecast(startPos, endPos, _layerWall).collider) check = false;
            
        Debug.DrawLine(startPos,endPos,Color.red);
        
        return check;
    }
    

    private void OnJump()
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
        m_rg.sharedMaterial = null;
        m_isJumpStart = false;
        PlayAudio(TagConst.AUDIO_JUMP1);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.GROUND))
        {
            m_isLand = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.GROUND))
        {
            
            if (!m_isJump || m_isJumpStart) return;
            
            if(m_rg.velocity.y == 0)
            {
                m_isJump = false;
                m_rg.sharedMaterial = _highFriction;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj.CompareTag(TagConst.GROUND))
        {
            m_isLand = false;
        }
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
        m_anim.SetBool(TagConst.ParamHit,m_isHit);
        Physics2D.IgnoreLayerCollision(3,6);
        StartCoroutine(CountDownResetHit());
        ChangeHealth(damage * -1);
    }

    private IEnumerator CountDownResetHit()
    {
        yield return new WaitForSeconds(2);
        m_isHit = false;
        m_anim.SetBool(TagConst.ParamHit,m_isHit);
        Physics2D.IgnoreLayerCollision(3,6,false);
    }
    
    public void ChangeHealth(float val)
    {
        _healthBar.ChangeHealth(val);
        m_isDeath = _healthBar.CheckOutOfHealth();
        if (m_isDeath) End();
    }

    private void End()
    {
        Instantiate(_effectBloodPlayer, transform.position, Quaternion.identity);
        PlayAudio(TagConst.AUDIO_DEATH);
        m_isDeath = true;
        m_anim.SetBool(TagConst.ParamHit,false);
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

    public void ActiveSkill(TagConst.Skill skill,bool isActive = true)
    {
        switch (skill)
        {
            case TagConst.Skill.F:
                isActiveSkillF = isActive;
                break;
            case TagConst.Skill.G:
                isActiveSkillG = isActive;
                break;
        }
    }
}
