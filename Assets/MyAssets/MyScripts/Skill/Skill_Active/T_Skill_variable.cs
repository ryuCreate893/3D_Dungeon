using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B

[Tooltip("�X�L����'�^�[�Q�b�e�B���O����'���������܂��B")]
[System.Serializable]
abstract class T_Skill_variable : ActiveSkill
{
    [SerializeField, Tooltip("�X�L���̎˒�����")]
    private float range;
    public float Range { get { return range; } set { range = value; } }

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        user.SetMaxRange(range);
    }
    /// <summary>
    /// �W�I�̍��W(�W�I���Ȃ��ꍇ"transform.forward * range"��W�I�Ƃ���)���m�F���āARange���Ȃ��true��Ԃ�
    /// </summary>
    public bool RangeJudge()
    {
        float r = (user.GetTargetPosition(range) - user.GetMyPosition()).magnitude;
        return r <= range * range;
    }
}
