using System;
using UnityEngine;

public class PlayerDamageSender : DamageSender
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TagConst.PLAYER)) return;
        this.SendDamage(col.gameObject);
    }
}