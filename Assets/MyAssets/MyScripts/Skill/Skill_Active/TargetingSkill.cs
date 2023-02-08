using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B
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