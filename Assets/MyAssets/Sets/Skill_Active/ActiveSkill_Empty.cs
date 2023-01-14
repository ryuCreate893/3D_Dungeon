using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ActiveSkill_Empty : ActiveSkill
{
    public override float UseSkillCheck()
    {
        // スキルを撃てるかどうかの条件を記述(useType, 攻撃範囲など)
        if (useSp <= userStatus.Sp)
        {
            UseSkill();
            return useSp;
        }

        // スキルを撃つ条件が整わなかった場合は0を返す
        Debug.Log("スキルを撃てませんでした…。");
        return 0;
    }

    protected override void UseSkill()
    {
        Debug.Log("保有SP = [ " + userStatus.Sp + " ]");
        userStatus.Sp -= useSp;
        Debug.Log("スキルを撃ちました！(消費SP = [" + useSp + " ]");
        Debug.Log("保有SP = [ " + userStatus.Sp + " ]");

    }
}
