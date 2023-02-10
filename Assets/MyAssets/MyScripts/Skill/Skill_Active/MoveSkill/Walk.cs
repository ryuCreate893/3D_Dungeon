using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Walk : ActiveSkill
{
    /* private UseSP_valiable S_judge; // SP消費処理
     * private Targeting_valiable T_judge; // ターゲッティング処理
     * private Charge_valiable C_judge; // チャージ処理
     */
    [SerializeField, Tooltip("ダッシュ時の加速倍率")]
    private float dashSpeed;
    [SerializeField, Tooltip("ダッシュに掛かる時間(freezeTime > dashTime)")]
    private float dashTime;

    private Transform userTransform;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userTransform = user.GetComponent<Transform>();
    }

    public override void TrySkill()
    {
        SkillContent();
        user._actionTime = freezeTime;
    }

    public override void SkillContent()
    {
        Vector3 v3 = userTransform.forward * userStatus.Speed;
        user._velocity = v3;
        user._actionTime = freezeTime;
        Debug.Log(user.gameObject.name + "は前に歩き始めた。");
    }
}
