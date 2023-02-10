using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Walk : ActiveSkill
{
    /* private UseSP_valiable S_judge; // SP�����
     * private Targeting_valiable T_judge; // �^�[�Q�b�e�B���O����
     * private Charge_valiable C_judge; // �`���[�W����
     */
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
        Debug.Log(user.gameObject.name + "�͑O�ɕ����n�߂��B");
    }
}
