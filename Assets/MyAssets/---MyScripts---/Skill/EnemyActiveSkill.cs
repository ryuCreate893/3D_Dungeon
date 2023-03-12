using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyActiveSkill
{
    [SerializeField, Tooltip("�G�������Ă���X�L��")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("�G�������Ă���X�L���̃^�C�v")]
    private SkillType type;

    [SerializeField, Tooltip("�X�L���𔭓�����m��(��퓬��)")]
    private int p_ordinary;
    [SerializeField, Tooltip("�X�L���𔭓�����m��(�퓬��)")]
    private int p_battle;

    public ActiveSkill Skill { get { return skill; } set { skill = value; } }
    public SkillType Type { get { return type; } set { type = value; } }
    public int P_ordinary { get { return p_ordinary; } set { p_ordinary = value; } }
    public int P_battle { get { return p_battle; } set { p_battle = value; } }

    /// <summary>
    /// �X�L�����������݂āA�m����˔j�����ꍇ��true��Ԃ��܂��B
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
    [Tooltip("��퓬���̂ݎg���܂��B")]
    ordinary,
    [Tooltip("�퓬���̂ݎg���܂��B")]
    battle,
    [Tooltip("���ł��g���܂��B")]
    always
}