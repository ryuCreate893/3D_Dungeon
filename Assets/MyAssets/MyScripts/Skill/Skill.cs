using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B
abstract class Skill : MonoBehaviour
{
    [SerializeField, Tooltip("�X�L������)")]
    private string information;

    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[
    /// </summary>
    protected Character user;
    /// <summary>
    /// �X�L�����������Ă���L�����N�^�[�̃X�e�[�^�X
    /// </summary>
    protected CurrentStatus userStatus;

    /// <summary>
    /// �X�L���g�p�҂̏����擾���܂��B
    /// </summary>
    public virtual void SetSkill(GameObject character)
    {
        user = character.GetComponent<Character>();
        userStatus = user._current;
    }

    /// <summary>
    /// �X�L���𔭓������Ƃ��̓��e���������܂��B
    /// </summary>
    public abstract void SkillContent();
}