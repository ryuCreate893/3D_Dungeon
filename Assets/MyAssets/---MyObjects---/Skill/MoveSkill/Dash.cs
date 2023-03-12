using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Dash : ActiveSkill
{
    [Header("専用スキル情報")]
    [SerializeField, Tooltip("ダッシュ時の加速倍率")]
    private float dash_speed;

    private Rigidbody user_rigidbody;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        user_rigidbody = user.GetComponent<Rigidbody>();
    }

    public override void SkillContent()
    {
        Vector3 v3 = user_transform.forward * user_status.Speed * dash_speed;
        user.Velocity = v3;
        user_rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        Invoke("ReturnFreeze", freeze_time);
    }

    private void ReturnFreeze()
    {
        user_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}
