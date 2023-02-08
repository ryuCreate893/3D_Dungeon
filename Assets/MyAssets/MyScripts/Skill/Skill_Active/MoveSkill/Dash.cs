using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Dash : NonTargetingSkill
{
    [SerializeField, Tooltip("�_�b�V�����̉����{��")]
    private float dashSpeed;
    [SerializeField, Tooltip("�_�b�V���Ɋ|���鎞��(freezeTime > dashTime)")]
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