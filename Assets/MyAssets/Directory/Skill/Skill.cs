using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Skill : MonoBehaviour
{
    // *** このスクリプトは継承用のため、アタッチしないでください。

    /// <summary>
    /// スキル使用者のステータス情報を取得します。
    /// </summary>
    public void UserSet(GameObject character)
    {
        user = character.GetComponent<Character>();
        userStatus = user._current;
        Debug.Log(character.name + "のステータス・メソッドを取得！");
    }

    /// <summary>
    /// スキルの内容を実装します。
    /// </summary>
    protected abstract void SkillContent();

    /// <summary>
    /// 条件を満たす場合はスキルを発動します。
    /// </summary>
    public abstract void TrySkill();

    /// <summary>
    /// スキルを解除します。
    /// </summary>
    public abstract void CancelSkill();

    /// <summary>
    /// スキルを所持しているキャラクターの変数です。
    /// </summary>
    protected Character user;

    /// <summary>
    /// スキルを所持しているキャラクターのステータスを取得します。
    /// </summary>
    protected CurrentStatus userStatus;
}
