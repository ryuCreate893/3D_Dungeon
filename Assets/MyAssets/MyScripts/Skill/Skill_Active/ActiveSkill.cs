using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。

abstract class ActiveSkill : Skill
{
    [SerializeField, Tooltip("消費スキルポイント量")]
    protected int useSp;
    [SerializeField, Tooltip("スキルの射程距離")]
    protected float range;
    [SerializeField, Tooltip("スキルのチャージタイム")]
    protected float chargeTime;
    [SerializeField, Tooltip("スキル発動後の硬直時間")]
    protected float freezeTime;
    [SerializeField, Tooltip("ダメージを受けたときにチャージを解除する")]
    protected bool damagedCancel;
    /// <summary>
    /// スキルをチャージしている状態かどうか判定
    /// </summary>
    protected bool charging;

    /// <summary>
    /// スキルを撃てるかどうかチェックして、可能な場合はスキルを撃ちます。
    /// </summary>
    abstract public void TrySkill();

    /// <summary>
    /// ダメージを受けた場合の処理です。チャージスキルの場合はoverrideして実装します。
    /// </summary>
    public virtual void DamagedCancel() { }
    /// <summary>
    /// スキルの射程を返します。射程を持たない場合は0を返します。
    /// </summary>
    public virtual float GetRange() { return range; }
}