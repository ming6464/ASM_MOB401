using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public TagConst.Skill skillType;
    public float skillCooldown = 1f;
    private Slider m_skillCooldownSlider;
    private TextMeshProUGUI m_timeText;
    private PlayerController m_player;
    private bool m_isUsed;

    private void Awake()
    {
        m_skillCooldownSlider = GetComponent<Slider>();
        m_timeText = GetComponentInChildren<TextMeshProUGUI>();
        m_player = FindObjectOfType<PlayerController>();
        m_skillCooldownSlider.maxValue = skillCooldown;
    }

    private void Update()
    {
        if (!m_isUsed) return;
        m_skillCooldownSlider.value = skillCooldown;
        m_timeText.text = Math.Round(skillCooldown, 1).ToString("F1");
        skillCooldown -= Time.deltaTime;
        if(skillCooldown <= 0) ActiveSkill();
    }

    public void UseSkill()
    {
        skillCooldown = m_skillCooldownSlider.maxValue;
        m_isUsed = true;
        UpdateSkillToPlayer();
    }

    public void ActiveSkill()
    {
        m_isUsed = false;
        m_skillCooldownSlider.value = 0;
        UpdateSkillToPlayer();
    }
    private void UpdateSkillToPlayer()
    {
        m_timeText.enabled = m_isUsed;
        m_player.ActiveSkill(skillType,!m_isUsed);
    }
}