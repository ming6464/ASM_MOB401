using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _offsetY,_minX,_maxY,_minY,_maxX;

    private void Start()
    {
        if (!_player) _player = GameObject.FindObjectOfType<PlayerController>()?.transform;
    }

    void LateUpdate()
    {
        if (!_player) return;
        float x = Mathf.Clamp(_player.position.x, _minX, _maxX);
        float y = Mathf.Clamp(_player.position.y + _offsetY, _minY, _maxY);
        x = Mathf.Lerp(transform.position.x, x, 0.05f);
        y = Mathf.Lerp(transform.position.y, y, 0.2f);
        transform.position = new Vector3(x, y, -10);
    }
}
