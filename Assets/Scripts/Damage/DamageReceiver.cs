using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Damage Receiver")]
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _hp;
    protected bool m_isDead;

    private void OnEnable()
    {
        this.Reborn();
    }

    public virtual void Reborn()
    {
        _hp = _maxHp;
    }
    
    public virtual void Add(int add)
    {
        if (this.m_isDead) return;
        _hp += add;
        if (_hp >= _maxHp) _hp = _maxHp;
    }

    public virtual void Deduct(int deduct)
    {
        if (this.m_isDead) return;
        _hp -= deduct;
        if (_hp < 0) _hp = 0;
        CheckIsDead();
    }

    protected virtual bool IsDead()
    {
        return this._hp <= 0;
    }

    protected virtual void CheckIsDead()
    {
        if (!IsDead()) return;
        this.m_isDead = true;
        this.OnDead();
    }

    protected virtual void OnDead()
    {
        throw new NotImplementedException();
    }
}