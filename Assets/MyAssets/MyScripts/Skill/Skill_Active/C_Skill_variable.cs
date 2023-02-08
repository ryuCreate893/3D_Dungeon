using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B

[Tooltip("�X�L����'�`���[�W����'���������܂��B")]
[System.Serializable]
abstract class C_Skill_variable : ActiveSkill
{
    [SerializeField, Tooltip("�X�L���̔����Ɋ|���鎞��")]
    private float chargeTime;
    [SerializeField, Tooltip("�_���[�W���󂯂���`���[�W���L�����Z������")]
    private bool damagedCancel;

    /// <summary>
    /// �X�L���̔����Ɋ|���鎞��
    /// </summary>
    public float ChargeTime { get { return chargeTime; } }

    /// <summary>
    /// �`���[�W�����ǂ����𔻒�
    /// </summary>
    public bool isCharge { get; set; }

  /// <summary>
  /// �_���[�W���󂯂���`���[�W���L�����Z������
  /// </summary>
    public override void DamagedCancel()
    {
        if (damagedCancel && isCharge)
        {
            CancelSkill();
        }
    }

    public void CancelSkill()
    {
        isCharge = false;
        user.SetChargeSkill(-1);
        user.SetActionTime(0);
        Debug.Log("�`���[�W���L�����Z�����ꂽ�c�B");
    }
}