using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("キャラクターの基礎能力をセットします。")]
[CreateAssetMenu(menuName = "Create Status")]
public class Status : ScriptableObject
{
    public BasicStatus Basic;
    public PerceptionData Perception;
}

[Tooltip("基礎ステータスを設定します。")]
[System.Serializable]
public class BasicStatus
{
    [SerializeField]
    [Tooltip("キャラクターの基礎レベルです。\n整数で設定します。")]
    private int level;
    [SerializeField]
    [Tooltip("基礎レベル時の体力最大値です。\n整数で設定します。")]
    private int maxHp;
    [SerializeField]
    [Tooltip("基礎レベル時のスキルポイントです。\n整数で設定します。")]
    private int maxSp;
    [SerializeField]
    [Tooltip("基礎レベル時の攻撃力です。\n整数で設定します。")]
    private int atk;
    [SerializeField]
    [Tooltip("基礎レベル時の被ダメージ軽減率(%)です。\n整数で設定します。\n100(%)以上も設定できます。")]
    private int def;
    [SerializeField]
    [Tooltip("基礎レベル時の移動速度です。\n整数で設定します。")]
    private int speed;
    [SerializeField]
    [Tooltip("基礎レベル時の所持経験値です。\n整数で設定します。")]
    private int exp;

    public int Level { get { return level; } }
    public int MaxHp { get { return maxHp; } }
    public int MaxSp { get { return maxSp; } }
    public int Atk { get { return atk; } }
    public int Def { get { return def; } }
    public int Speed { get { return speed; } }
    public int Exp { get { return exp; } }
}



[Tooltip("キャラクターの持つ知覚情報を設定します。")]
[System.Serializable]
public class PerceptionData
{
    [SerializeField]
    [Tooltip("このキャラクターは視覚で敵を認知します。")]
    private bool vision;

    [SerializeField]
    [Tooltip("このキャラクターは聴覚で敵を認知します。")]
    private bool aural;

    [SerializeField]
    [Tooltip("このキャラクターは魔力で敵を認知します。")]
    private bool magic;

    public bool Vision { get { return vision; } set { vision = value; } }
    public bool Aural { get { return aural; } set { aural = value; } }
    public bool Magic { get { return magic; } set { magic = value; } }
}

[Tooltip("成長度を設定します。")]
[System.Serializable]
public class GrowthValue
{
    [Tooltip("体力の成長度です。\n範囲 : [ 0 - 100 ](%)")]
    [Range(0, 100)]
    [SerializeField]
    private float maxHp = 0;

    [Tooltip("スキルポイントの成長度です。\n範囲 : [ 0 - 100 ](%)")]
    [Range(0, 100)]
    [SerializeField]
    private float maxSp = 0;

    [Tooltip("攻撃力の成長度です。\n範囲 : [ 0 - 100 ](%)")]
    [Range(0, 100)]
    [SerializeField]
    private float atk = 0;

    [Tooltip("被ダメージ軽減率の成長度です。\n範囲 : [ 0 - 100 ](%)")]
    [Range(0, 100)]
    [SerializeField]
    private float def = 0;

    [Tooltip("移動速度の成長度です。\n範囲 : [ 0 - 100 ](%)")]
    [Range(0, 100)]
    [SerializeField]
    private float speed = 0;

    public float MaxHp { get { return (maxHp + 100) / 100; } }
    public float MaxSp { get { return (maxSp + 100) / 100; } }
    public float Atk { get { return (atk + 100) / 100; } }
    public float Def_percent { get { return (def + 100) / 100; } }
    public float Speed { get { return (speed + 100) / 100; } }
}

[Tooltip("小数点以下の内部ステータスを保有します(inspectorで設定不可)。")]
[System.Serializable]
public class FloatStatus
{
    [SerializeField, NonEditable]
    private float maxHp;
    [SerializeField, NonEditable]
    private float maxSp;
    [SerializeField, NonEditable]
    private float atk;
    [SerializeField, NonEditable]
    private float def;
    [SerializeField, NonEditable]
    private float speed;

    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float MaxSp { get { return maxSp; } set { maxSp = value; } }
    public float Atk { get { return atk; } set { atk = value; } }
    public float Def { get { return def; } set { def = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
}

[Tooltip("フィールド上におけるステータスを保有します(inspectorで設定不可)。")]
[System.Serializable]
public class CurrentStatus
{
    [SerializeField, NonEditable]
    private int level;
    [SerializeField, NonEditable]
    private int maxHp;
    [SerializeField, NonEditable]
    private int hp;
    [SerializeField, NonEditable]
    private int maxSp;
    [SerializeField, NonEditable]
    private int sp;
    [SerializeField, NonEditable]
    private int atk;
    [SerializeField, NonEditable]
    private int def;
    [SerializeField, NonEditable]
    private int speed;
    [SerializeField, NonEditable]
    private int exp;

    public int Level { get { return level; } set { level = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int Hp { get { return hp; } set { hp = value; } }
    public int MaxSp { get { return maxSp; } set { maxSp = value; } }
    public int Sp { get { return sp; } set { sp = value; } }
    public int Atk { get { return atk; } set { atk = value; } }
    public int Def { get { return def; } set { def = value; } }
    public int Speed { get { return speed; } set { speed = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
}