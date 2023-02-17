using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Dash : ActiveSkill
{
    [Header("専用スキル情報")]
    [SerializeField, Tooltip("ダッシュ時の加速倍率")]
    private float dashSpeed;

    private Transform userTransform;
    private Rigidbody userRigidbody;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userTransform = user.GetComponent<Transform>();
        userRigidbody = user.GetComponent<Rigidbody>();
    }

    public override void SkillContent()
    {
        Vector3 v3 = userTransform.forward * userStatus.Speed * dashSpeed;
        user._velocity = v3;
        userRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        Invoke("ReturnFreeze", freezeTime);
    }

    private void ReturnFreeze()
    {
        userRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
