using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : ActiveSkill
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
        int rnd = Random.Range(-180, 181);
        Quaternion quaternion = Quaternion.AngleAxis(rnd, userTransform.up);
        user._characterRotation = quaternion;
        userTransform.rotation = Quaternion.RotateTowards(userTransform.rotation, quaternion, userStatus.Speed * Time.deltaTime);
        user._actionTime = freezeTime;

        if (rnd < 0)
        {
            Debug.Log(user.gameObject.name + "�͍���" + rnd + "�x��]�����B");
        }
        else
        {
            Debug.Log(user.gameObject.name + "�͉E��" + rnd + "�x��]�����B");
        }
    }
}
