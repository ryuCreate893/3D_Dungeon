using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class ActiveSkill : Skill
{
    // *** スキル発動条件データ ***
    [SerializeField, Tooltip("消費スキルポイント量")]
    private int useSp;
    [SerializeField, Tooltip("攻撃の射程(0 = 対象自分のみ)")]
    private float range;
    [SerializeField, Tooltip("スキルを使うことができるタイミング")]
    private SkillType skillType;
    // ※skillTypeはEnemyの処理で使うのでプレイヤーは関係しないようにする

    // *** スキル使用中データ ***
    [SerializeField, Tooltip("スキルの発動後に掛かる硬直時間\n(モーション込みのため'0'では機能させません)")]
    protected float freezeTime;

    /// <summary>
    /// スキルを撃てるかどうかチェックして、可能な場合はスキルを撃ちます。
    /// </summary>
    abstract public int TrySkill();

    /// <summary>
    /// ダメージを受けたときにスキルを解除します。
    /// </summary>
    abstract public void DamagedCancel();

    public float GetRange()
    {
        return range;
    }

    public SkillType SkillTypeCheck()
    {
        return skillType;
    }

    /// <summary>
    /// SP残量と射程距離を確認して、条件を満たす場合は"true"を返します。
    /// </summary>
    /// <returns></returns>
    protected bool UseJudge()
    {
        if (useSp <= userStatus.Sp)
        {
            if (range == 0 || userMethod.CheckTargetDistence(range))
            {
                return true;
            }
        }
        return false;
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

[Tooltip("スキルに'チャージ処理'を実装します。")]
[System.Serializable]
public class ChargeSkill
{
    [SerializeField, Tooltip("スキルの発動に掛かる時間")]
    private float chargeTime;
    [SerializeField, Tooltip("ダメージを受けたらチャージをキャンセルする")]
    private bool damagedCancel;

    private bool charging = false;

    /// <summary>
    /// スキルの発動に掛かる時間
    /// </summary>
    public float ChargeTime { get { return chargeTime; } }

    /// <summary>
    /// ダメージを受けたらチャージをキャンセルする
    /// </summary>
    public bool DamagedCancel { get { return damagedCancel; } }

    /// <summary>
    /// チャージ中かどうかを判定します。
    /// </summary>
    public bool Charging { get { return charging; } set { charging = value; } }
}

/*
 ★確率は外部(enemyクラス)で実装したいので実装しません
 Statusのようなオブジェクト作成Scriptable?でできるかもしれない
 [SerializeField, Tooltip("スキルを発動する確率(%)\n(プレイヤーの場合は設定しません)")]
 [Range(0, 100)]
 private int probability = 100;

 別継承クラスとして実装？
 [SerializeField, Tooltip("数値を'0'よりも高くすると「魔法攻撃」判定となります。\n数字が大きいほど魔法感知ができる敵に気がつかれやすくなります。")]
 protected float magicColliderSize = 0;
*/


