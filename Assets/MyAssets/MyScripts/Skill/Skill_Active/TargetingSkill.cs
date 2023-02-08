using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class TargetingSkill : ActiveSkill
{
    private T_Skill_variable t_Skill;

    public override void TrySkill()
    {
        if (UseSpJudge())
        {
            if (t_Skill.RangeJudge())
            {
                SkillContent();
                user.SetActionTime(freezeTime);
            }
        }
    }
}