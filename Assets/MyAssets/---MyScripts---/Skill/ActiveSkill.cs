using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class ActiveSkill : Skill
{
    [SerializeField, Tooltip("消費スキルポイント量")]
    protected int use_sp;
    [SerializeField, Tooltip("スキル発動後の硬直時間")]
    protected float freeze_time;
    [SerializeField, Tooltip("スキルのチャージタイム")]
    protected float charge_time;
    [SerializeField, Tooltip("スキルの射程距離")]
    protected float range;
    [SerializeField, Tooltip("ダメージを受けたときにチャージを解除するか")]
    protected bool damaged_cancel;

    /// <summary>
    /// スキルをチャージしている状態かどうか判定
    /// </summary>
    public bool Charging { get; set; } = false;
    public float Range { get { return range; } }


    /// <summary>
    /// スキルの発動チェックと発動を行います。チャージが必要な場合は専用メソッドをキャラクターのデリゲートに保有させます。
    /// </summary>
    public bool TrySkill()
    {
        // SP残量のチェック
        if (use_sp > user.Status.Sp)
        {
            Debug.Log(gameObject.name + "を放つにはSPが足りない！");
            return false;
        }

        // 射程のチェック
        if (range > 0 && user.Focus.magnitude > range)
        {
            Debug.Log(gameObject.name + "は射程が足りなかった！");
            return false;
        }

        // チャージ状態のチェック
        else if (charge_time > 0 && !Charging)
        {
            user.isCharge = true;
            user.Action_time = charge_time;
            user.Action += Action;
            user.Action_cancel += ChargeCancel;
            if (damaged_cancel) user.Damaged += ChargeCancel;
            Debug.Log(gameObject.name + "のチャージを開始！");
            return false;
        }

        user.Status.Sp -= use_sp;
        user.Action_time = freeze_time;
        Debug.Log(gameObject.name + "を発動します！");
        return true;
    }

    // *** チャージ中にデリゲートに保有させる専用メソッド ***
    /// <summary>
    /// チャージ中のみ呼び出す「スキル発動」専用メソッド
    /// </summary>
    public void Action()
    {
        if (TrySkill())
        {
            SkillContent();
        }
        user.isCharge = false;
        user.Action -= Action;
        user.Action_cancel -= ChargeCancel;
        if (damaged_cancel) user.Damaged -= ChargeCancel;
    }

    /// <summary>
    /// チャージ中のみ呼び出す「スキルキャンセル」専用メソッド
    /// </summary>
    public void ChargeCancel()
    {
        user.isCharge = false;
        user.Action -= Action;
        user.Action_cancel -= ChargeCancel;
        if (damaged_cancel) user.Damaged -= ChargeCancel;
        user.Action_time = 0;
        Debug.Log(gameObject.name + "は解除された…。");
    }
}