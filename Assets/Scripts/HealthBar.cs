using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar :MonoBehaviour
{
    [SerializeField] private Slider _sliderHealth;
    private Vector3 m_offSet;
    private float m_curHealth,m_timeShow,m_startTimeShow,m_maxHealth;
    private bool m_isShow,m_isPlayer,m_isSetData;


    private void Start()
    {
        m_timeShow = 1.5f;
    }

    private void Update()
    {
        if (!m_isSetData) return;
        if (m_isShow)
        {
            m_startTimeShow -= Time.deltaTime;
            if (m_startTimeShow <= 0)
            {
                HideSliderHealth();
            }
        }

        if (m_isPlayer) return;
        _sliderHealth.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + m_offSet);
    }

    public void SetData(float maxHealth,Vector3 offSet,bool isPlayer = false)
    {
        
        if (m_isSetData) return;
        m_offSet = offSet;
        MaxHealth = maxHealth;
        CurHeath = maxHealth;
        m_isPlayer = isPlayer;
        _sliderHealth.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(GameManager.Ins.low, GameManager.Ins.high, _sliderHealth.normalizedValue);
        m_isSetData = true;
        if(!isPlayer) HideSliderHealth();
    }
    
    public void ChangeHealth(float val)
    {
        CurHeath += val;
        _sliderHealth.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(GameManager.Ins.low, GameManager.Ins.high, _sliderHealth.normalizedValue);
        HideSliderHealth(false);
    }

    public bool CheckOutOfHealth()
    {
        return CurHeath <= 0;
    }

    public float MaxHealth
    {
        get => this.m_maxHealth;
        set
        {
            m_maxHealth = value;
            _sliderHealth.maxValue = value;
        }
    }

    public float CurHeath
    {
        get => this.m_curHealth;
        set
        {
            value = Mathf.Clamp(value, 0, MaxHealth);
            m_curHealth = value;
            _sliderHealth.value = value;
        }
    }

    public void HideSliderHealth(bool isHide = true)
    {
        if (m_isPlayer) return;
        m_isShow = !isHide;
        if (m_isShow) m_startTimeShow = m_timeShow;
        _sliderHealth.gameObject.SetActive(m_isShow);
    }

}