using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class PassiveSkill : MonoBehaviour
{
    // *** "PassiveSkill_Empty" を 空の "GameObject" にアタッチした上で、
    // *** そのオブジェクトを各キャラのスキルリストに取り付けて使用します。
    // *** このスクリプトは継承用のため、アタッチしないでください。

    /// <summary>
    /// スキル使用者のステータスを取得します。
    /// </summary>
    public void UserStatusSet(GameObject character)
    {
        Debug.Log(character.name + "のステータスを取得します。");
        userStatus = character.GetComponent<CurrentStatus>();
        UseSkill();
    }

    /// <summary>
    /// パッシブスキルを発動します。
    /// </summary>
    protected abstract void UseSkill();

    /// <summary>
    /// パッシブスキルを解除します。
    /// </summary>
    protected abstract void OnDisable();

    /// <summary>
    /// スキルを所持しているキャラクターのステータスを取得します。
    /// </summary>
    protected CurrentStatus userStatus;
}
