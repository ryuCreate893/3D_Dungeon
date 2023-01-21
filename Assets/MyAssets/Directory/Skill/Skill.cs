using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Skill : MonoBehaviour
{
    // *** 各スキルの"空スクリプト"にスキルを実装、空の "GameObject" にアタッチして
    // *** そのオブジェクトを各キャラのスキルリストに取り付けて使用します。
    // *** このスクリプトは継承用のため、アタッチしないでください。

    /// <summary>
    /// スキル使用者のステータス情報を取得します。
    /// </summary>
    public void UserSet(GameObject character)
    {
        userTransform = character.GetComponent<Transform>();
        userMethod = character.GetComponent<Character>();
        userStatus = character.GetComponent<CurrentStatus>();
        Debug.Log(gameObject.name + "が" + character.name + "のステータスを取得しました！");
    }

    /// <summary>
    /// スキルの内容を実装します。
    /// </summary>
    public abstract void SkillContent();

    /// <summary>
    /// スキルが発動できるかどうかをチェックします。
    /// </summary>
    public abstract void TrySkill();

    /// <summary>
    /// スキルを解除します。
    /// </summary>
    public abstract void CancelSkill();

    /// <summary>
    /// スキルを所持しているキャラクター本体の座標用変数です。
    /// </summary>
    protected Transform userTransform;

    /// <summary>
    /// スキルを所持しているキャラクターのメソッド用変数です。
    /// </summary>
    protected Character userMethod;

    /// <summary>
    /// スキルを所持しているキャラクターのステータスを取得します。
    /// </summary>
    protected CurrentStatus userStatus;
}
