using System;
using UnityEngine;

public class EnemyDamageSender : DamageSender
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.ENEMY)) return;
        SendDamage(gObj);
    }
}