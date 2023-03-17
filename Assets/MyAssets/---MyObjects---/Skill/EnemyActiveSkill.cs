using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyActiveSkill
{
    [SerializeField, Tooltip("敵が持っているスキル")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("敵が持っているスキルのタイプ")]
    private SkillType type;

    [SerializeField, Tooltip("スキルを発動する確率(非戦闘時)")]
    private int p_ordinary;
    [SerializeField, Tooltip("スキルを発動する確率(戦闘時)")]
    private int p_battle;

    public ActiveSkill Skill { get { return skill; } set { skill = value; } }
    public SkillType Type { get { return type; } set { type = value; } }
    public int P_ordinary { get { return p_ordinary; } set { p_ordinary = value; } }
    public int P_battle { get { return p_battle; } set { p_battle = value; } }

    /// <summary>
    /// スキル発動を試みて、確率を突破した場合はtrueを返します。
    /// </summary>
    public bool Judge(SkillType type)
    {
        if (type == SkillType.battle)
        {
            return Random.Range(1, 101) <= p_battle;
        }
        else if (type == SkillType.ordinary)
        {
            return Random.Range(1, 101) <= p_ordinary;
        }
        else
        {
            return false;
        }
    }
}

public enum SkillType
{
    [Tooltip("非戦闘時のみ使えます。")]
    ordinary,
    [Tooltip("戦闘時のみ使えます。")]
    battle,
    [Tooltip("いつでも使えます。")]
    always
}