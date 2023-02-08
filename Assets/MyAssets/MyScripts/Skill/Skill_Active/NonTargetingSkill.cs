using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class NonTargetingSkill : ActiveSkill
{
    public override void TrySkill()
    {
        if (UseSpJudge())
        {
            SkillContent();
            user.SetActionTime(freezeTime);
        }
    }
}