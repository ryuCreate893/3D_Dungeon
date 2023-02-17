using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Jump : ActiveSkill
{
    [Header("��p�X�L�����")]
    [SerializeField, Tooltip("�W�����v�̍���")]
    private float height;

    private Rigidbody userRigidbody;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userRigidbody = user.GetComponent<Rigidbody>();
    }

    public override void SkillContent()
    {
        userRigidbody.velocity -= new Vector3(0, userRigidbody.velocity.y, 0);
        userRigidbody.AddForce(new Vector3(0, height, 0), ForceMode.Impulse);
    }
}
