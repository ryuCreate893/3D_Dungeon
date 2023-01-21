using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Skill : MonoBehaviour
{
    // *** �e�X�L����"��X�N���v�g"�ɃX�L���������A��� "GameObject" �ɃA�^�b�`����
    // *** ���̃I�u�W�F�N�g���e�L�����̃X�L�����X�g�Ɏ��t���Ďg�p���܂��B
    // *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B

    /// <summary>
    /// �X�L���g�p�҂̃X�e�[�^�X�����擾���܂��B
    /// </summary>
    public void UserSet(GameObject character)
    {
        userTransform = character.GetComponent<Transform>();
        userMethod = character.GetComponent<Character>();
        userStatus = character.GetComponent<CurrentStatus>();
        Debug.Log(gameObject.name + "��" + character.name + "�̃X�e�[�^�X���擾���܂����I");
    }

    /// <summary>
    /// �X�L���̓��e���������܂��B
    /// </summary>
    public abstract void SkillContent();

    /// <summary>
    /// �X�L���������ł��邩�ǂ������`�F�b�N���܂��B
    /// </summary>
    public abstract void TrySkill();

    /// <summary>
    /// �X�L�����������܂��B
    /// </summary>
    public abstract void CancelSkill();

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�{�̂̍��W�p�ϐ��ł��B
    /// </summary>
    protected Transform userTransform;

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃��\�b�h�p�ϐ��ł��B
    /// </summary>
    protected Character userMethod;

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃X�e�[�^�X���擾���܂��B
    /// </summary>
    protected CurrentStatus userStatus;
}
