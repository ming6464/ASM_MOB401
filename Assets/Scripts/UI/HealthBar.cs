using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar :MonoBehaviour
{
    [SerializeField] private Slider _sliderHealth;
    [SerializeField] private float _timeShow;
    private Vector3 m_offSet;
    private float m_curHealth,m_startTimeShow,m_maxHealth;
    private bool m_isShow,m_isPlayer,m_isSetData;

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
        _sliderHealth.maxValue = maxHealth;
        _sliderHealth.value = maxHealth;
        m_isPlayer = isPlayer;
        m_isSetData = true;
        if(!isPlayer) HideSliderHealth();
        UpdateColorHealthBar();
    }
    
    public void ChangeHealth(int val)
    {
        val = Mathf.Abs(val);
        _sliderHealth.value = val;
        UpdateColorHealthBar();
        ShowSliderHealth();
    }

    private void UpdateColorHealthBar()
    {
        _sliderHealth.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(GameManager.Ins.colorLowHealth, GameManager.Ins.colorHighHealth, _sliderHealth.normalizedValue);
    }

    public void ShowSliderHealth()
    {
        if (m_isPlayer) return;
        m_startTimeShow = _timeShow;
        m_isShow = true;
        _sliderHealth.gameObject.SetActive(m_isShow);
    }

    public void HideSliderHealth()
    {
        m_isShow = false;
        _sliderHealth.gameObject.SetActive(m_isShow);
    }

}