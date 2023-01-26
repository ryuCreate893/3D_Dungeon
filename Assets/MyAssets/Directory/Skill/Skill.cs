using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Skill : MonoBehaviour
{
    // *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B

    /// <summary>
    /// �X�L���g�p�҂̃X�e�[�^�X�����擾���܂��B
    /// </summary>
    public void UserSet(GameObject character)
    {
        user = character.GetComponent<Character>();
        userStatus = user._current;
        Debug.Log(character.name + "�̃X�e�[�^�X�E���\�b�h���擾�I");
    }

    /// <summary>
    /// �X�L���̓��e���������܂��B
    /// </summary>
    protected abstract void SkillContent();

    /// <summary>
    /// �����𖞂����ꍇ�̓X�L���𔭓����܂��B
    /// </summary>
    public abstract void TrySkill();

    /// <summary>
    /// �X�L�����������܂��B
    /// </summary>
    public abstract void CancelSkill();

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̕ϐ��ł��B
    /// </summary>
    protected Character user;

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃X�e�[�^�X���擾���܂��B
    /// </summary>
    protected CurrentStatus userStatus;
}
