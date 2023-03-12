using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Jump : ActiveSkill
{
    [Header("��p�X�L�����")]
    [SerializeField, Tooltip("�W�����v�̍���")]
    private float height;

    private Rigidbody user_rigidbody;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        user_rigidbody = user.GetComponent<Rigidbody>();
    }

    public override void SkillContent()
    {
        user_rigidbody.velocity -= new Vector3(0, user_rigidbody.velocity.y, 0);
        user_rigidbody.AddForce(new Vector3(0, height, 0), ForceMode.Impulse);
    }
}
