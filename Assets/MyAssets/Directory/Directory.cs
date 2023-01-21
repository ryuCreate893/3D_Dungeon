using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directory : MonoBehaviour
{
    /// <summary>
    /// �G�̖��ӂł��B
    /// </summary>
    public Directory_Enemy[] _enemy;

    /// <summary>
    /// �A�C�e���̖��ӂł��B
    /// </summary>
    public Directory_Item[] _item;

    /// <summary>
    /// �X�L���̖��ӂł��B
    /// </summary>
    public Directory_Skill[] _skill;
}

[Tooltip("�G�̖��ӂ̓��e��ݒ肵�܂��B")]
[System.Serializable]
public class Directory_Enemy
{
    [SerializeField]
    [Tooltip("�G�̖��Ӕԍ��ł��B")]
    private int _number;
    [SerializeField]
    [Tooltip("�G�̖��O�ł��B")]
    private string _name;
    [SerializeField]
    [Tooltip("�G�̊O���ł��B")]
    private Sprite _sprite;
    [SerializeField]
    [Tooltip("�G�̊�b�\�͂ł��B")]
    private Status _status;
    [SerializeField]
    [Tooltip("�G�̐����ł��B")]
    private string _information;
    [SerializeField]
    [Tooltip("�����ς݂��ǂ������`�F�b�N���܂��B")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public Sprite _Sprite { get { return _sprite; } }
    public Status _Status { get { return _status; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}

[Tooltip("�A�C�e���̖��ӂ̓��e��ݒ肵�܂��B")]
[System.Serializable]
public class Directory_Item
{
    [SerializeField]
    [Tooltip("�A�C�e���̖��Ӕԍ��ł��B")]
    private int _number;
    [SerializeField]
    [Tooltip("�A�C�e���̖��O�ł��B")]
    private string _name;
    [SerializeField]
    [Tooltip("�A�C�e���̊O���ł��B")]
    private Sprite _sprite;
    [SerializeField]
    [Tooltip("�A�C�e���̐����ł��B")]
    private string _information;
    [SerializeField]
    [Tooltip("�����ς݂��ǂ������`�F�b�N���܂��B")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public Sprite _Sprite { get { return _sprite; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}

[Tooltip("�X�L���̖��ӂ̓��e��ݒ肵�܂��B")]
[System.Serializable]
public class Directory_Skill
{
    private enum type
    {
        active,
        passive
    }

    [SerializeField]
    [Tooltip("�X�L���̖��Ӕԍ��ł��B")]
    private int _number;
    [SerializeField]
    [Tooltip("�X�L���̖��O�ł��B")]
    private string _name;
    [SerializeField]
    [Tooltip("�X�L���̎�ނł��B(0 = �A�N�e�B�u�X�L��, 1 = �p�b�V�u�X�L��)")]
    private type _type;
    [SerializeField]
    [Tooltip("�X�L���̐����ł��B")]
    private string _information;
    [SerializeField]
    [Tooltip("�����ς݂��ǂ������`�F�b�N���܂��B")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public int _Type { get { return (int)_type; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}
