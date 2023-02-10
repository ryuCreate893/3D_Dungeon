using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Jump : ActiveSkill
{
    /* private UseSP_valiable S_judge; // SP�����
     * private Targeting_valiable T_judge; // �^�[�Q�b�e�B���O����
     * private Charge_valiable C_judge; // �`���[�W����
     */
    [SerializeField, Tooltip("�W�����v�̍���")]
    private float height;

    private Rigidbody userRigidbody;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userRigidbody = user.GetComponent<Rigidbody>();
    }

    public override void TrySkill()
    {
        SkillContent();
        user._actionTime = freezeTime;
    }

    public override void SkillContent()
    {
        userRigidbody.velocity -= new Vector3(0, userRigidbody.velocity.y, 0);
        userRigidbody.AddForce(new Vector3(0, height, 0), ForceMode.Impulse);
    }
}
