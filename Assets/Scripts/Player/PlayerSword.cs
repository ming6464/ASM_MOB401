using System;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{

    [SerializeField]
    private int _damage = 10,_damegaSkill = 35;

    private PlayerController m_player;

    private void Start()
    {
        m_player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.ENEMY))
        {
            float damage = _damage;

            if (m_player.isUsingSkill) damage = _damegaSkill;
            
            gObj.GetComponent<Enemy>().OnHit(damage);
        }
    }
}
