using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B
abstract class NonTargetingSkill_C : ActiveSkill
{
    private C_Skill_variable c_Skill;

    public override void TrySkill()
    {
        if (UseSpJudge())
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