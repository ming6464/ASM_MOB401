using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillScript :  MonoBehaviour
{
    [SerializeField] private TagConst.Skill _skill;
    [SerializeField] private Slider _skillCooldownSlider;
    [SerializeField] private float _skillCooldown = 1;
    [SerializeField] private PlayerController _player;
    [SerializeField] private TextMeshProUGUI _timeText;
    private bool m_skillUsed;
    private float m_timeCountdown;
    

    private void Start()
    {
        if (!_skillCooldownSlider) _skillCooldownSlider = GetComponent<Slider>();
        if (!_timeText) _timeText = GetComponentInChildren<TextMeshProUGUI>();

        _skillCooldownSlider.maxValue = _skillCooldown;
        
        ActiveSkill();
    }

    private void Update()
    {
        if (m_skillUsed)
        {
            _skillCooldownSlider.value = m_timeCountdown;
            m_timeCountdown -= Time.deltaTime;
            _timeText.text = Math.Round(m_timeCountdown,1).ToString("F1");
            if (m_timeCountdown <= 0) ActiveSkill();
        }
    }

    private void ActiveSkill()
    {
        m_skillUsed = false;
        _skillCooldownSlider.value = 0;
        _timeText.enabled = false;
        UpdateSkillToPlayer();
    }

    public void UseSkill()
    {
        m_timeCountdown = _skillCooldown;
        m_skillUsed = true;
        _timeText.enabled = true;
        UpdateSkillToPlayer();

    }

    private void UpdateSkillToPlayer()
    {
        if (!_player) _player = GameObject.FindObjectOfType<PlayerController>();
        _player.ActiveSkill(_skill,!m_skillUsed);
    }
}