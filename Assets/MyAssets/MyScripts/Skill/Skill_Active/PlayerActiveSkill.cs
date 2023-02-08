using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class PlayerActiveSkill
{
    [SerializeField, Tooltip("�X�L���I�u�W�F�N�g��ݒ�")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("�A�N�V�����ɑΉ�����L�[��ݒ�")]
    private string key;

    [SerializeField, Tooltip("true�ɂ���Ɣ������͈ړ����s���Ȃ��Ȃ�܂��B")]
    private bool isGetAxis;

    public ActiveSkill Skill { get { return skill; } }
    public string Key { get { return key; } set { key = value; } }
    public bool IsGetAxis { get { return isGetAxis; } }
}