using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Dash : NonTargetingSkill
{
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

    public override void SkillContent()
    {
        Vector3 v3 = userTransform.forward * userStatus.Speed * dashSpeed;
        user.SetVelocity(v3);
    }
}