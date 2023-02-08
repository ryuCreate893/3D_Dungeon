using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。

[Tooltip("スキルに'ターゲッティング処理'を実装します。")]
[System.Serializable]
abstract class T_Skill_variable : ActiveSkill
{
    [SerializeField, Tooltip("スキルの射程距離")]
    private float range;
    public float Range { get { return range; } set { range = value; } }

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        user.SetMaxRange(range);
    }
    /// <summary>
    /// 標的の座標(標的がない場合"transform.forward * range"を標的とする)を確認して、Range内ならばtrueを返す
    /// </summary>
    public bool RangeJudge()
    {
        float r = (user.GetTargetPosition(range) - user.GetMyPosition()).magnitude;
        return r <= range * range;
    }
}
