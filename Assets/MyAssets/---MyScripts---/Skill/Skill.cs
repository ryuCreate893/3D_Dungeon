using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class Skill : MonoBehaviour
{
    [SerializeField, Tooltip("スキル説明)")]
    private string information;

    /// <summary>
    /// スキルを所持しているキャラクター
    /// </summary>
    protected Character user;
    /// <summary>
    /// スキルを所持しているキャラクターのステータス
    /// </summary>
    protected CharacterStatus user_status;
    /// <summary>
    /// スキルを所持しているキャラクターの位置情報
    /// </summary>
    protected Transform user_transform;
    /// <summary>
    /// スキルを所持しているキャラクターのアニメーション設定
    /// </summary>
    protected Animator user_animation;


    /// <summary>
    /// スキル使用者の情報を取得します。
    /// </summary>
    public virtual void SetSkill(GameObject character)
    {
        user = character.GetComponent<Character>();
        user_status = user.Status;
        user_transform = user.GetComponent<Transform>();
        user_animation = user.GetComponent<Animator>();
    }

    /// <summary>
    /// スキルを発動したときの内容を実装します。
    /// </summary>
    public abstract void SkillContent();
}