using Unity.Mathematics;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _key;
    [SerializeField] protected bool _isBoss;
    protected Animator m_anim;
    protected Rigidbody2D m_rg;
    protected bool isDeath, m_isHit, m_isSeePlayer,m_isPlayerAttack;
    private PlayerController m_player;
    protected string m_animPass, m_animCur;

    [SerializeField]
    protected HealthBar _healthBar;
    [SerializeField]
    private Vector3 _healthBarOffSet;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private string _name;
    [SerializeField] private int _point,_damage;
    

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
        if (m_isPlayerAttack)
        {
            Data.UpdateData(_name,_point);
        }

        if (_isBoss && _key)
        {
            GameObject newKey = Instantiate(_key, transform.position, quaternion.identity);
            newKey.GetComponent<Rigidbody2D>().velocity = new Vector2(2 * FindDirPlayer(), 2);
        }
        AudioManager.Ins.PlayAudio(TagConst.AUDIO_KILL,true);
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
        if (gObj.CompareTag(TagConst.PLAYER) && m_player)
        {
            m_player.OnHit(_damage);
        }
        if (gObj.CompareTag(TagConst.DEATHZONE))
        {
            m_isPlayerAttack = false;
            Death();
        }
    }

    private void SeePlayer(bool isSee)
    {
        m_isSeePlayer = isSee;
    }
    public abstract void OnHit(int damage);

}