using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladePlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 10, _timeAcitve = 2,_damage = 35;
    private bool m_isRun;
    private float m_dir = 1, m_countdonwnActived;

    // Update is called once per frame
    void Update()
    {
        if (!m_isRun) return;
        transform.Translate(Vector3.right * m_dir * _speed * Time.deltaTime);
        m_countdonwnActived -= Time.deltaTime;
        if (m_countdonwnActived <= 0) Destroy(gameObject);
    }

    public void Run(float xdirection)
    {
        m_dir = xdirection;
        transform.localScale = new Vector3(xdirection, 1, 1);
        m_isRun = true;
        m_countdonwnActived = _timeAcitve;
    }
}
