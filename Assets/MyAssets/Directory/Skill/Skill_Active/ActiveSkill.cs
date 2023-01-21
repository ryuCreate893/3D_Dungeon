using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class ActiveSkill : Skill
{
    public override void TrySkill()
    {
        if (charging)
        {
            charging = false;
            if (userStatus.Sp < useSp)
            {
                Debug.Log("何らかの原因でSpが足りなくなりました。");
            }
            else
            {
                userStatus.Sp -= useSp;
                SkillContent();
                Debug.Log(gameObject.name + "を発動しました！");
            }
        }
        else if (UseJudge())
        {
            if (chargeTime != 0)
            {
                userMethod.ActionTimeSet(chargeTime);
                charging = true;
                Debug.Log(gameObject.name + "のチャージを開始します！");
            }
            else
            {
                SkillContent();
                userMethod.ActionTimeSet(freezeTime);
                charging = false;
                Debug.Log(gameObject.name + "を''即時''発動しました！");
            }
        }
    }

    public override void CancelSkill()
    {
        if (charging && damagedCancel)
        {
            charging = false;
            userMethod.ActionTimeSet(0);
        }
    }

    public float GetRange()
    {
        return range;
    }

    /// <summary>
    /// SP状態, 発動確率の２つの条件からスキルの発動を判定します。
    /// </summary>
    /// <returns></returns>
    protected bool UseJudge()
    {
        if (userStatus.Sp >= useSp)
        {
            int rnd = Random.Range(1, 101);
            return probability >= rnd;
        }
        else
        {
            return false;
        }
    }

    [Tooltip("消費スキルポイント量です。")]
    [SerializeField]
    protected int useSp;

    [Tooltip("攻撃の射程です。")]
    [SerializeField]
    protected float range;

    [Tooltip("数値を'0'よりも高くすると「魔法攻撃」判定となります。\n数字が大きいほど魔法感知ができる敵に気がつかれやすくなります。")]
    [SerializeField]
    protected float magic;

    [Tooltip("スキルの発動に掛かる時間です。'0'でも機能させます。")]
    [SerializeField]
    protected float chargeTime;

    [Tooltip("スキルの発動後に掛かる硬直時間です。モーション込みのため'0'では機能させません。")]
    [SerializeField]
    protected float freezeTime;

    [Tooltip("スキルを発動する確率(%)です。\nプレイヤーの場合は基本的に'100'を設定します。")]
    [SerializeField]
    [Range(1, 100)]
    protected int probability;

    [Tooltip("攻撃を受けた場合にスキルチャージをキャンセルするかどうかを設定します。")]
    [SerializeField]
    protected bool damagedCancel;


    [Tooltip("チャージ中かどうかを判定します。")]
    protected bool charging = false;
}
