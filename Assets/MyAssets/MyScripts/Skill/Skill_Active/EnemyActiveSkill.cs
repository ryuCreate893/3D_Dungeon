using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyActiveSkill
{
    [SerializeField, Tooltip("敵が持っているスキル")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("敵が持っているスキル")]
    private SkillType type;

    [SerializeField, Tooltip("スキルを発動する確率(非戦闘時)")]
    private int p_ordinary;
    [SerializeField, Tooltip("スキルを発動する確率(戦闘時)")]
    private int p_battle;

    public ActiveSkill Skill { get { return skill; } set { skill = value; } }
    public SkillType Type { get { return type; } }
    public int P_ordinary { get { return p_ordinary; } set { p_ordinary = value; } }
    public int P_battle { get { return p_battle; } set { p_battle = value; } }

    /// <summary>
    /// 確率を参照して、確率を突破した場合はtrueを返します。
    /// </summary>
    public bool JudgeBattle { get { return Random.Range(1, 101) <= p_battle; } }

    /// <summary>
    /// 確率を参照して、確率を突破した場合はtrueを返します。
    /// </summary>
    public bool JudgeOrdinary { get { return Random.Range(1, 101) <= p_ordinary; } }
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