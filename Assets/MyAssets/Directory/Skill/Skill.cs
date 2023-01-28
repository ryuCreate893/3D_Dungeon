using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class Skill : MonoBehaviour
{
    /// <summary>
    /// スキルを所持しているキャラクターのメソッド
    /// </summary>
    protected Character userMethod;
    /// <summary>
    /// スキルを所持しているキャラクターのステータス
    /// </summary>
    protected CurrentStatus userStatus;
    /// <summary>
    /// キャラクターの持つスキル番号
    /// </summary>
    protected int skill_number;

    /// <summary>
    /// スキル使用者のステータス情報を取得します。
    /// </summary>
    public virtual void UserSet(GameObject character, int i)
    {
        userMethod = character.GetComponent<Character>();
        userStatus = userMethod._current;
        skill_number = i;
        Debug.Log(character.name + "は" + gameObject.name + "を覚えた！");
    }

    /// <summary>
    /// スキルを発動します。(スキルの内容はこの中ですべて実装)
    /// </summary>
    public abstract void SkillContent();
}
