using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyActiveSkill
{
    [SerializeField, Tooltip("�G�������Ă���X�L��")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("�G�������Ă���X�L��")]
    private SkillType type;

    [SerializeField, Tooltip("�X�L���𔭓�����m��(��퓬��)")]
    private int p_ordinary;
    [SerializeField, Tooltip("�X�L���𔭓�����m��(�퓬��)")]
    private int p_battle;

    public ActiveSkill Skill { get { return skill; } set { skill = value; } }
    public SkillType Type { get { return type; } }
    public int P_ordinary { get { return p_ordinary; } set { p_ordinary = value; } }
    public int P_battle { get { return p_battle; } set { p_battle = value; } }

    /// <summary>
    /// �m�����Q�Ƃ��āA�m����˔j�����ꍇ��true��Ԃ��܂��B
    /// </summary>
    public bool JudgeBattle { get { return Random.Range(1, 101) <= p_battle; } }

    /// <summary>
    /// �m�����Q�Ƃ��āA�m����˔j�����ꍇ��true��Ԃ��܂��B
    /// </summary>
    public bool JudgeOrdinary { get { return Random.Range(1, 101) <= p_ordinary; } }
}

public enum SkillType
{
    [Tooltip("��퓬���̂ݎg���܂��B")]
    ordinary,
    [Tooltip("�퓬���̂ݎg���܂��B")]
    battle,
    [Tooltip("���ł��g���܂��B")]
    always
}