using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : ActiveSkill
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
        int rnd = Random.Range(-180, 181);
        Quaternion quaternion = Quaternion.AngleAxis(rnd, userTransform.up);
        user._characterRotation = quaternion;
        userTransform.rotation = Quaternion.RotateTowards(userTransform.rotation, quaternion, userStatus.Speed * Time.deltaTime);
        user._actionTime = freezeTime;

        if (rnd < 0)
        {
            Debug.Log(user.gameObject.name + "は左に" + rnd + "度回転した。");
        }
        else
        {
            Debug.Log(user.gameObject.name + "は右に" + rnd + "度回転した。");
        }
    }
}
