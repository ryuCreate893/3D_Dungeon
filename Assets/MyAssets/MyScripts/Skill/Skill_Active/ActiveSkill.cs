using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。

abstract class ActiveSkill : Skill
{
    [SerializeField, Tooltip("スキル発動後の硬直時間")]
    protected float freezeTime;

    [Header("スキルSP使用情報")]
    [SerializeField, Tooltip("消費スキルポイント量")]
    protected int useSp;

    [Header("スキル射程情報")]
    [SerializeField, Tooltip("スキルの射程距離")]
    protected float range;

    [Header("スキルチャージ情報")]
    [SerializeField, Tooltip("スキルのチャージタイム")]
    protected float chargeTime;
    [SerializeField, Tooltip("ダメージを受けたときにチャージを解除する")]
    protected bool damagedCancel;

    /// <summary>
    /// スキルをチャージしている状態かどうか判定
    /// </summary>
    public bool Charging { get; set; } = false;


    /// <summary>
    /// スキルを撃てるかどうかチェックし、可能な場合はSpとActionTimeを設定してtrueを返します。
    /// </summary>
    public bool TrySkill()
    {
        // SP残量のチェック
        if(useSp > user._Status.Sp)
        {
            Debug.Log(gameObject.name + "を放つにはSPが足りない！");
            return false;
        }

        // 射程のチェック
        if(range > 0 && !user.CheckTargetDistence(range))
        {
            Debug.Log(gameObject.name + "は射程が足りなかった！");
            return false;
        }

        // チャージ状態のチェック
        else if(chargeTime > 0 && !Charging)
        {
            Charging = true;
            user._actionTime = chargeTime;
            Debug.Log(gameObject.name + "のチャージを開始！");
            return false;
        }

        user._Status.Sp -= useSp;
        user._actionTime = freezeTime;
        Debug.Log(gameObject.name + "を発動します！");
        return true;
    }


    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    public void DamagedCancel()
    {
        if (damagedCancel && Charging) ChargeCancel();
    }

    /// <summary>
    /// チャージスキルをキャンセルします。
    /// </summary>
    public void ChargeCancel()
    {
        Charging = false;
        user._actionTime = 0;
        user._chargeSkill = -1;
        Debug.Log(gameObject.name + "は解除された…。");
    }


    /// <summary>
    /// スキルの射程を返します。射程を持たない場合は0を返します。
    /// </summary>
    public float GetRange()
    {
        return range;
    }
}