using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform _nextPos;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(TagConst.PLAYER)){
            if(Mathf.Abs(other.transform.position.x - transform.position.x) > 0.45f) other.transform.position = _nextPos.position;
        }
    }
}
