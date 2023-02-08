using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class TargetingSkill_C : ActiveSkill
{
    private T_Skill_variable t_Skill;
    private C_Skill_variable c_Skill;

    public override void TrySkill()
    {
        if (UseSpJudge())
        {
            if (t_Skill.RangeJudge())
            {
                if (c_Skill.isCharge)
                {
                    SkillContent();
                    user.SetActionTime(freezeTime);
                    user.SetChargeSkill(-1);
                }
                else
                {
                    c_Skill.isCharge = true;
                    user.SetActionTime(c_Skill.ChargeTime);
                    //user.SetChargeTime();
                }
            }
        }
    }
}