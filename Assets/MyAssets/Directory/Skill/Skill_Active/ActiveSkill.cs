using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class ActiveSkill : Skill
{
    // *** このスクリプトは継承用のため、アタッチしないでください。

    [Tooltip("消費スキルポイント量です。")]
    private int useSp;

    [SerializeField, Tooltip("スキルの発動に掛かる時間です。'0'でも機能させます。")]
    private float chargeTime;

    [SerializeField, Tooltip("スキルの発動後に掛かる硬直時間です。モーション込みのため'0'では機能させません。")]
    private float freezeTime;

    [SerializeField, Tooltip("スキルを発動する確率(%)です。\nプレイヤーの場合は基本的に'100'を設定します。")]
    [Range(1, 100)]
    private int probability = 1;

    [SerializeField, Tooltip("攻撃を受けた場合にスキルチャージをキャンセルするかどうかを設定します。")]
    private bool damagedCancel;

    [SerializeField, Tooltip("攻撃の射程です。'0'の場合は自分に使用するスキルです。")]
    private float range;

    [SerializeField, Tooltip("数値を'0'よりも高くすると「魔法攻撃」判定となります。\n数字が大きいほど魔法感知ができる敵に気がつかれやすくなります。")]
    protected float magicColliderSize = 0;

    [SerializeField, Tooltip("スキルを使うことができるタイミングを設定します。")]
    private SkillType skillType;


    /// <summary>
    /// チャージ中かどうかを判定します。
    /// </summary>
    private bool charging = false;

    /// <summary>
    /// チャージ中かどうかを判定します。
    /// </summary>
    private Transform target;

    /// <summary>
    /// 発動条件を満たしたらスキルをチャージします。
    /// チャージ時間がない場合は即発動を試みます。
    /// </summary>
    public virtual int ChargeSkill(int number)
    {
        int rnd = Random.Range(1, 101);
        if (probability >= rnd)
        {
            if (chargeTime == 0)
            {
                TrySkill();
            }
            else if (UseJudge())
            {
                charging = true;
                user.ActionTimeSet(chargeTime);
                Debug.Log("スキル:" + gameObject.name + "をチャージ！");
                return number;
            }
        }
        return -1;
    }

    public override void TrySkill()
    {
        if (UseJudge())
        {
            userStatus.Sp -= useSp;
            charging = false;
            user.ActionTimeSet(freezeTime);
            SkillContent();
            Debug.Log("スキル:" + gameObject.name + "を発動！");
        }
        else
        {
            CancelSkill();
        }
    }

    private bool UseJudge()
    {
        if (useSp <= userStatus.Sp)
        {
            if (range == 0)
            {
                return true;
            }
            else
            {
                if (user.CheckTargetDistence(range))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void DamagedCancel()
    {
        if (charging && damagedCancel)
        {
            CancelSkill();
        }
    }

    public override void CancelSkill()
    {
        charging = false;
        user.ActionTimeSet(0);
        Debug.Log("スキルがキャンセルされた…。");
    }

    public float GetRange()
    {
        return range;
    }

    public SkillType SkillTypeCheck()
    {
        return skillType;
    }
}

public enum SkillType
{
    ordinary,
    battle,
    always
}
