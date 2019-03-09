using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None,
    Fly,
    Dash,
    Stress
}

[System.Serializable]
public struct SkillInfo
{
    public int id;
    public SkillType skillType;
    public int effectId;
    public int duration;
}

public class SkillConfig : ScriptableObject
{
    public SkillInfo[] skillInfoList;

    public SkillInfo? GetSkillInfo(int skillId)
    {
        for (var i = 0; i < skillInfoList.Length; i++)
        {
            var skill = skillInfoList[i];
            if (skill.id == skillId)
            {
                return skill;
            }
        }

        return null;
    }
}
