using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager :  MonoBehaviour
{
    
    private Skill[] _skills;
    
    private void Start()
    {
        _skills = GetComponentsInChildren<Skill>();
        foreach (var skill in _skills)
        {
            skill.ActiveSkill();
        }
    }

    public void UseSkill(TagConst.Skill skillType)
    {
        foreach (var skill in _skills)
        {
            if (skill.skillType == skillType)
            {
                skill.UseSkill();
                return;
            }
        }
        Debug.Log("Can not find skill !");
    }
}