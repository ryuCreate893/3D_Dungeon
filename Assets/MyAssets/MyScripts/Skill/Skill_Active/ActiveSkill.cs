using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。

// スキルの種別(Enemyで使用)
public enum SkillType
{
    [Tooltip("非戦闘時のみ使えます。")]
    ordinary,
    [Tooltip("戦闘時のみ使えます。")]
    battle,
    [Tooltip("いつでも使えます。")]
    always
}
abstract class ActiveSkill : Skill
{
    // *** スキル発動条件データ ***
    [Header("スキル本体データ")]
    [SerializeField, Tooltip("消費スキルポイント量")]
    protected int useSp;
    [SerializeField, Tooltip("スキルの発動後に掛かる硬直時間)")]
    protected float freezeTime;
    [SerializeField, Tooltip("スキルを使うことができるタイミング")]
    private SkillType skillType;

    /// <summary>
    /// スキルを撃てるかどうかチェックして、可能な場合はスキルを撃ちます。
    /// </summary>
    abstract public void TrySkill();

    /// <summary>
    /// SP残量を確認して、使用量を上回る場合はtrueを返します。
    /// </summary>
    protected bool UseSpJudge()
    {
        return useSp <= userStatus.Sp;
    }

    /// <summary>
    /// ダメージを受けた場合の処理です。チャージスキルの場合はoverrideして実装します。
    /// </summary>
    public virtual void DamagedCancel() { }

    /// <summary>
    /// 敵がスキルタイプをチェックするためのメソッドです。
    /// </summary>
    public SkillType SkillTypeCheck()
    {
        return skillType;
    }
}