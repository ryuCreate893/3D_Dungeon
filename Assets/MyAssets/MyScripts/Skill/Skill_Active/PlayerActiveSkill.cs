using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class PlayerActiveSkill
{
    [SerializeField, Tooltip("スキルオブジェクトを設定")]
    private ActiveSkill skill;

    [SerializeField, Tooltip("アクションに対応するキーを設定")]
    private string key;

    [SerializeField, Tooltip("trueにすると発動中は移動を行えなくなります。")]
    private bool isGetAxis;

    public ActiveSkill Skill { get { return skill; } }
    public string Key { get { return key; } set { key = value; } }
    public bool IsGetAxis { get { return isGetAxis; } }
}