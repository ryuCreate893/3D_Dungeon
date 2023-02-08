using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。

[Tooltip("スキルに'チャージ処理'を実装します。")]
[System.Serializable]
abstract class C_Skill_variable : ActiveSkill
{
    [SerializeField, Tooltip("スキルの発動に掛かる時間")]
    private float chargeTime;
    [SerializeField, Tooltip("ダメージを受けたらチャージをキャンセルする")]
    private bool damagedCancel;

    /// <summary>
    /// スキルの発動に掛かる時間
    /// </summary>
    public float ChargeTime { get { return chargeTime; } }

    /// <summary>
    /// チャージ中かどうかを判定
    /// </summary>
    public bool isCharge { get; set; }

  /// <summary>
  /// ダメージを受けたらチャージをキャンセルする
  /// </summary>
    public override void DamagedCancel()
    {
        if (damagedCancel && isCharge)
        {
            CancelSkill();
        }
    }

    public void CancelSkill()
    {
        isCharge = false;
        user.SetChargeSkill(-1);
        user.SetActionTime(0);
        Debug.Log("チャージがキャンセルされた…。");
    }
}