using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int val;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.PLAYER))
        {
            AudioManager.Ins.PlayAudio(TagConst.AUDIO_PICKUP,true);
            gObj.GetComponent<PlayerController>().ChangeHealth(val);
            Destroy(this.gameObject);
        }
    }
}
