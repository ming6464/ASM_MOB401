using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibleArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag(TagConst.PLAYER)) SendMessageUpwards("SeePlayer",true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(TagConst.PLAYER)) SendMessageUpwards("SeePlayer",false);
    }
}
