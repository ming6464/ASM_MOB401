using Unity.Mathematics;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public bool isBoss,isHit,isDeath;
    [SerializeField] private GameObject _key;
    [SerializeField] private string _name;
    [SerializeField] private int _point;
    [SerializeField] private GameObject _effectBlood;
    
    protected Animator m_anim;
    protected Rigidbody2D m_rg;
    protected bool m_isSeePlayer;
    private PlayerController m_player;
    protected string m_animPass, m_animCur;

    protected virtual void Start()
    {
        m_anim = GetComponent<Animator>();
        m_rg = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindWithTag(TagConst.PLAYER).GetComponent<PlayerController>();
        if (isBoss && !_key) _key = Resources.Load<GameObject>(TagConst.URL_PREFABS + "Key");
        if (!_effectBlood) _effectBlood = Resources.Load<GameObject>(TagConst.URL_PREFABS + "EffectBlood");
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

    public void OnDead()
    {
        m_anim.SetTrigger(TagConst.ParamDeath);
        m_rg.velocity = new Vector2(0f, 0f);
    }

    private void Death()
    {
        Data.UpdateData(_name,_point);
        if (isBoss)
        {
            if(!_key) _key = Resources.Load<GameObject>(TagConst.URL_PREFABS + "Key");
            GameObject newKey = Instantiate(_key, transform.position, quaternion.identity);
            newKey.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);
        }
        AudioManager.Ins.PlayAudio(TagConst.AUDIO_KILL,true);
        Instantiate(_effectBlood, transform.position, quaternion.identity);
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.DEATHZONE)) Death();
    }

    private void SeePlayer(bool isSee)
    {
        m_isSeePlayer = isSee;
    }
    public abstract void OnHit();

}