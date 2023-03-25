using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("キャラクターの基礎能力をセットします。")]
[CreateAssetMenu(menuName = "Create BasicStatus")]
public class CharacterStatus : ScriptableObject
{
    [Header("基礎ステータス")]
    [SerializeField]
    [Tooltip("キャラクターの基礎レベル")]
    private int base_level;
    public int Base_Level { get { return base_level; } }

    [SerializeField]
    [Tooltip("基礎レベル時の体力最大値")]
    private int base_maxHp;
    public int Base_MaxHp { get { return base_maxHp; } }

    [SerializeField]
    [Tooltip("基礎レベル時のスキルポイント")]
    private int base_maxSp;
    public int Base_MaxSp { get { return base_maxSp; } }

    [SerializeField]
    [Tooltip("基礎レベル時の攻撃力")]
    private int base_atk;
    public int Base_Atk { get { return base_atk; } }

    [SerializeField]
    [Tooltip("基礎レベル時の被ダメージ軽減率(%)")]
    private int base_def;
    public int Base_Def { get { return base_def; } }

    [SerializeField]
    [Tooltip("基礎レベル時の移動速度")]
    private float base_speed;
    public float Base_Speed { get { return base_speed; } }

    [SerializeField]
    [Tooltip("基礎レベル時の[Player:必要経験値, Enemy:所持経験値]")]
    private int base_exp;
    public int Base_Exp { get { return base_exp; } }


    [Header("ステータス成長度")]
    [SerializeField, Tooltip("体力の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private int growMaxHp = 0;
    public int GrowMaxHp { get; set; }

    [SerializeField, Tooltip("スキルポイントの成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private int growMaxSp = 0;
    public int GrowMaxSp { get; set; }

    [SerializeField, Tooltip("攻撃力の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private int growAtk = 0;
    public int GrowAtk { get; set; }

    [SerializeField, Tooltip("所持経験値の成長度[ 0 - 100 ](%)")]
    [Range(0, 100)]
    private int growExp = 0;
    public int GrowExp { get; set; }

    [Header("知覚情報")]
    [SerializeField, Range(0, 10)]
    [Tooltip("このキャラクターは視覚で敵を認知できます(ない場合は'0')")]
    private float vision;
    public float Vision { get { return vision; } }

    [SerializeField, Range(0, 10)]
    [Tooltip("このキャラクターは聴覚で敵を認知できます(ない場合は'0')")]
    private float aural;
    public float Aural { get { return aural; } }

    [SerializeField, Range(0, 10)]
    [Tooltip("このキャラクターは魔力で敵を認知できます(ない場合は'0')")]
    private float magic;
    public float Magic { get { return magic; } }

    public int Level { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int MaxSp { get; set; }
    public int Sp { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public float Speed { get; set; }
    public int Exp { get; set; }

    public int AddHp { get; set; } = 0;
    public int AddSp { get; set; } = 0;
    public int AddAtk { get; set; } = 0;
    public int AddDef { get; set; } = 0;
    public float AddSpeed { get; set; } = 0;
}