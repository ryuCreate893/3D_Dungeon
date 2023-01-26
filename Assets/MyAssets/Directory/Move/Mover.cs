using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Mover : MonoBehaviour
{
    // *** ���̃X�N���v�g�̓A�^�b�`���܂���B ***

    [Tooltip("�ړ����@���I�΂��m��(%)�ł��B")]
    [SerializeField, Range(1, 100)]
    public int probability;

    /// <summary>
    /// �A�N�V��������"true"�����āA�A���œ����A�N�V�����͍s���Ȃ��悤�ɂ��܂��B
    /// </summary>
    private bool isActioned;

    /// <summary>
    /// ���̈ړ����@�����L�����N�^�[�{�̂ł��B
    /// </summary>
    protected GameObject user;

    /// <summary>
    /// �L�����N�^�[�̈ړ������ł��B
    /// </summary>
    protected Vector3 velocity;

    public void SetMove(GameObject character)
    {
        user = character;
    }

    public bool UseJudge()
    {
        if (!isActioned)
        {
            int rnd = Random.Range(1, 101);
            if (probability >= rnd)
            {
                SetVelocity();
                isActioned = false;
                return true;
            }
        }
        return false;
    }

    abstract public Vector3 SetVelocity();
}
