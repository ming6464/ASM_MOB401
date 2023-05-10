using UnityEngine;

public class DamageSender : MonoBehaviour
{
    [Header("Damage Sender")]
    [SerializeField] protected int _damage;

    protected virtual void SendDamage(GameObject obj)
    {
        DamageReceiver damageRc = obj.GetComponent<DamageReceiver>();
        if(!damageRc) return;
        SendDamage(damageRc);
    }

    protected virtual void SendDamage(DamageReceiver damageRc)
    {
        if (!damageRc) return;
        damageRc.Deduct(_damage);
    }
}