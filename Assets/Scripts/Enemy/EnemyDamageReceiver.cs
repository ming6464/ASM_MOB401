using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    [SerializeField]
    protected HealthBar _healthBar;
    [SerializeField] private Vector3 _healthBarOffSet;
    [SerializeField] private Enemy _enemyCtrl;

    public override void Reborn()
    {
        base.Reborn();
        LoadHealthBar();
        LoadEnemyCtl();
        _healthBar.SetData(this._maxHp,_healthBarOffSet);
    }

    private void LoadEnemyCtl()
    {
        if(_enemyCtrl) return;
        _enemyCtrl = gameObject.GetComponent<Enemy>();
    }

    private void LoadHealthBar()
    {
        if (_healthBar) return;
        _healthBar = GetComponentInChildren<HealthBar>();
    }

    public override void Deduct(int deduct)
    {
        if (_enemyCtrl.isDeath) return;
        base.Deduct(deduct);
        _healthBar.ChangeHealth(this._hp);
        _enemyCtrl.OnHit();
    }

    protected override void OnDead()
    {
        _enemyCtrl.isDeath = true;
        _enemyCtrl.OnDead();
    }
}