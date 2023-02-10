using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
    [Header("基礎ステータス")]
    [SerializeField, Tooltip("基礎ステータスの設定")]
    private BasicStatus Basic;

    [Header("内部ステータス")]
    [SerializeField, Tooltip("体力の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growMaxHp = 0;
    public float GrowMaxHp { get; set; }
    public float FloatMaxHp { get; set; }

    [SerializeField, Tooltip("スキルポイントの成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growMaxSp = 0;
    public float GrowMaxSp { get; set; }
    public float FloatMaxSp { get; set; }

    [SerializeField, Tooltip("攻撃力の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growAtk = 0;
    public float GrowAtk { get; set; }
    public float FloatAtk { get; set; }

    [SerializeField, Tooltip("被ダメージ軽減率の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growDef = 0;
    public float GrowDef { get; set; }
    public float FloatDef { get; set; }

    [SerializeField, Tooltip("移動速度の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growSpeed = 0;
    public float GrowSpeed { get; set; }
    public float FloatSpeed { get; set; }

    [SerializeField, Tooltip("所持経験値の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private float growExp = 0;
    public float GrowExp { get; set; }
    public float FloatExp { get; set; }


    [Header("外部ステータス")]
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

    /// <summary>
    /// 出現時の能力を設定します。
    /// </summary>
    public void SpawnStatus(int n)
    {
        Level = Basic.Level;
        FloatMaxHp = Basic.MaxHp;
        FloatMaxSp = Basic.MaxSp;
        FloatAtk = Basic.Atk;
        FloatDef = Basic.Def;
        FloatSpeed = Basic.Speed;
        FloatExp = Basic.Exp;

        if (n > 0)
        {
            LevelUp(n);
        }
        else if (n < 0)
        {
            LevelDown(n);
        }
        else
        {
            SetStatus();
        }
    }

    public void LevelUp(int n)
    {
        for (int i = 0; i < n; i++)
        {
            level++;
            FloatMaxHp *= ((GrowMaxHp + 100) / 100);
            FloatMaxSp *= ((GrowMaxSp + 100) / 100);
            FloatAtk *= ((GrowAtk + 100) / 100);
            FloatDef *= ((GrowDef + 100) / 100);
            FloatSpeed *= ((GrowSpeed + 100) / 100);
            FloatExp *= ((GrowExp + 100) / 100);
        }
        SetStatus();
    }

    public void LevelDown(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (level > 0)
            {
                level--;
                FloatMaxHp /= ((GrowMaxHp + 100) / 100);
                FloatMaxSp /= ((GrowMaxSp + 100) / 100);
                FloatAtk /= ((GrowAtk + 100) / 100);
                FloatDef /= ((GrowDef + 100) / 100);
                FloatSpeed /= ((GrowSpeed + 100) / 100);
                FloatExp /= ((GrowExp + 100) / 100);
            }
        }
        SetStatus();
    }

    public void SetStatus()
    {
        MaxHp = (int)FloatMaxHp + AddHp;
        Hp = MaxHp;
        MaxSp = (int)FloatMaxSp + AddSp;
        Sp = MaxSp;
        Atk = (int)FloatAtk + AddAtk;
        Def = (int)FloatDef + AddDef;
        Speed = (int)FloatSpeed + AddSpeed;
        Exp = (int)FloatExp;
    }
}