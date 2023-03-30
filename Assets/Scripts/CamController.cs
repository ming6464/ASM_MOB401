using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : Singleton<CamController>
{
    [SerializeField]
    private float _minX,_maxY,_minY,_maxX;

    private float m_passX, m_passY, m_dx;
    public override void Start()
    {
        m_passX = transform.position.x;
        m_passY = transform.position.y;
    }

    public void ChangePos(Vector3 pos)
    {
        m_dx = 0;
        float x = Mathf.Clamp(pos.x, _minX, _maxX);
        float y = Mathf.Clamp(pos.y + 2f, _minY, _maxY);
        if (m_passX != x || m_passY != y)
        {
            m_passX -= x;
            m_passY -= y;
            transform.position = new Vector3(x, y, -10);
            if (m_passX < 0) m_dx = 1;
            else if (m_passX > 0) m_dx = -1;
            m_passX = x;
            m_passY = y;
        }
        BackgroundScroll.Ins.Scrolling(m_dx);
    }
}
