using System;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    
    [Header("PlayerDamageReceiver")]
    [SerializeField] private HealthBar _healthBar;

    [SerializeField] private PlayerController _playerCtl;

    public override void Reborn()
    {
        base.Reborn();
        LoadHealthBar();
        LoadPlayerCtl();
    }

    private void LoadHealthBar()
    {
        if (!_healthBar) _healthBar = GameObject.FindWithTag("HealthBarPlayer").GetComponent<HealthBar>();
        _healthBar.SetData(this._maxHp,Vector3.zero,true);
    }

    private void LoadPlayerCtl()
    {
        if (_playerCtl) return;
        _playerCtl = gameObject.GetComponent<PlayerController>();
    }

    public override void Add(int add)
    {
        base.Add(add);
        _healthBar.ChangeHealth(this._hp);
    }

    public override void Deduct(int deduct)
    {
        if (_playerCtl.isHit) return;
        base.Deduct(deduct);
        _playerCtl.isHit = true;
        _healthBar.ChangeHealth(this._hp);
        _playerCtl.OnHit();
    }

    protected override void OnDead()
    {
        _playerCtl.OnDead();
    }
}