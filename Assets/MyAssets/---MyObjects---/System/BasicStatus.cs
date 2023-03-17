using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("キャラクターの基礎能力をセットします。")]
[CreateAssetMenu(menuName = "Create BasicStatus")]
public class BasicStatus : ScriptableObject
{
    [SerializeField]
    [Tooltip("キャラクターの基礎レベル")]
    private int level;
    public int Level { get { return level; } }

    [SerializeField]
    [Tooltip("基礎レベル時の体力最大値")]
    private int maxHp;
    public int MaxHp { get { return maxHp; } }

    [SerializeField]
    [Tooltip("基礎レベル時のスキルポイント")]
    private int maxSp;
    public int MaxSp { get { return maxSp; } }

    [SerializeField]
    [Tooltip("基礎レベル時の攻撃力")]
    private int atk;
    public int Atk { get { return atk; } }

    [SerializeField]
    [Tooltip("基礎レベル時の被ダメージ軽減率(%)")]
    private int def;
    public int Def { get { return def; } }

    [SerializeField]
    [Tooltip("基礎レベル時の移動速度")]
    private int speed;
    public int Speed { get { return speed; } }

    [SerializeField]
    [Tooltip("基礎レベル時の所持経験値")]
    private int exp;
    public int Exp { get { return exp; } }


    [SerializeField]
    [Tooltip("このキャラクターは視覚で敵を認知します。")]
    private bool vision;
    public bool Vision { get { return vision; } }

    [SerializeField]
    [Tooltip("このキャラクターは聴覚で敵を認知します。")]
    private bool aural;
    public bool Aural { get { return aural; } }

    [SerializeField]
    [Tooltip("このキャラクターは魔力で敵を認知します。")]
    private bool magic;
    public bool Magic { get { return magic; } }
}