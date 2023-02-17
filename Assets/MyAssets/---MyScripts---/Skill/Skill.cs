using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class Skill : MonoBehaviour
{
    [Header("基本スキル情報")]
    [SerializeField, Tooltip("スキル説明)")]
    private string information;

    /// <summary>
    /// スキルを所持しているキャラクター
    /// </summary>
    protected Character user;
    /// <summary>
    /// スキルを所持しているキャラクターのステータス
    /// </summary>
    protected CharacterStatus userStatus;

    /// <summary>
    /// スキル使用者の情報を取得します。
    /// </summary>
    public virtual void SetSkill(GameObject character)
    {
        user = character.GetComponent<Character>();
        userStatus = user._Status;
    }

    /// <summary>
    /// スキルを発動したときの内容を実装します。
    /// </summary>
    public abstract void SkillContent();
}