using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class ActiveSkill : MonoBehaviour
{
    // *** "ActiveSkill_Empty" を 空の "GameObject" にアタッチした上で、
    // *** そのオブジェクトを各キャラのスキルリストに取り付けて使用します。
    // *** このスクリプトは継承用のため、アタッチしないでください。

    /// <summary>
    /// スキル使用者のSP情報を取得します。
    /// </summary>
    public void UserSpSet(GameObject character)
    {
        Debug.Log(character.name + "のSP情報を取得します。");
        userStatus = character.GetComponent<Character>().status.Current;
        Debug.Log("取得完了！[ SP = " + userStatus.MaxSp + " ]");
    }
    /// <summary>
    /// スキルが使用可能かどうかをチェックします。
    /// </summary>
    public abstract float UseSkillCheck();

    /// <summary>
    /// 条件が整っているとき、スキルを使用します。
    /// </summary>
    protected abstract void UseSkill();

    protected enum Type
    {
        [Tooltip("バトル中のみ使用できます。")]
        battle,
        [Tooltip("非戦闘時に使用できます。")]
        normal,
        [Tooltip("いつでも使用できます。")]
        anytime
    }

    [Tooltip("スキルを使用できるタイミングを設定します。")]
    [SerializeField]
    protected Type useType;

    [Tooltip("消費スキルポイント量を設定します。")]
    [SerializeField]
    protected int useSp;

    [Tooltip("スキルの発動から次の行動に移ることができるまでの硬直時間です。")]
    [SerializeField]
    protected int coolTime;

    /// <summary>
    /// スキルを所持しているキャラクターのステータスを取得します。
    /// </summary>
    protected CurrentStatus userStatus;
}
