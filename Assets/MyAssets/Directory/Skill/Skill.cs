using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B
abstract class Skill : MonoBehaviour
{
    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃��\�b�h
    /// </summary>
    protected Character userMethod;
    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃X�e�[�^�X
    /// </summary>
    protected CurrentStatus userStatus;
    /// <summary>
    /// �L�����N�^�[�̎��X�L���ԍ�
    /// </summary>
    protected int skill_number;

    /// <summary>
    /// �X�L���g�p�҂̃X�e�[�^�X�����擾���܂��B
    /// </summary>
    public virtual void UserSet(GameObject character, int i)
    {
        userMethod = character.GetComponent<Character>();
        userStatus = userMethod._current;
        skill_number = i;
        Debug.Log(character.name + "��" + gameObject.name + "���o�����I");
    }

    /// <summary>
    /// �X�L���𔭓����܂��B(�X�L���̓��e�͂��̒��ł��ׂĎ���)
    /// </summary>
    public abstract void SkillContent();
}
