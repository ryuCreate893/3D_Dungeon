/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character_Status
{
    [Header("��b�X�e�[�^�X")]
    [SerializeField, Tooltip("��b�X�e�[�^�X�̐ݒ�")]
    private BasicStatus basic;
    public BasicStatus Basic { get { return basic; } }

    [Header("�����X�e�[�^�X")]
    [SerializeField, Tooltip("�̗͂̐����x[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growMaxHp = 0;
    public float GrowMaxHp { get; set; }
    public float FloatMaxHp { get; set; }

    [SerializeField, Tooltip("�X�L���|�C���g�̐����x[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growMaxSp = 0;
    public float GrowMaxSp { get; set; }
    public float FloatMaxSp { get; set; }

    [SerializeField, Tooltip("�U���͂̐����x[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growAtk = 0;
    public float GrowAtk { get; set; }
    public float FloatAtk { get; set; }

    [SerializeField, Tooltip("�����o���l�̐����x[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growExp = 0;
    public float GrowExp { get; set; }
    public float FloatExp { get; set; }


    [Header("�O���X�e�[�^�X")]
    [SerializeField, NonEditable]
    private int level;
    public int Level { get; set; }

    [SerializeField, NonEditable]
    private int maxHp;
    public int MaxHp { get; set; }

    [SerializeField, NonEditable]
    private int hp;
    public int Hp { get; set; }

    [SerializeField, NonEditable]
    private int maxSp;
    public int MaxSp { get; set; }

    [SerializeField, NonEditable]
    private int sp;
    public int Sp { get; set; }

    [SerializeField, NonEditable]
    private int atk;
    public int Atk { get; set; }

    [SerializeField, NonEditable]
    private int def;
    public int Def { get; set; }

    [SerializeField, NonEditable]
    private int speed;
    public int Speed { get; set; }

    [SerializeField, NonEditable]
    private int exp;
    public int Exp { get; set; }

    public int AddHp { get; set; } = 0;
    public int AddSp { get; set; } = 0;
    public int AddAtk { get; set; } = 0;
    public int AddDef { get; set; } = 0;
    public int AddSpeed { get; set; } = 0;
}
*/