using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** ���̃X�N���v�g�͌p���p�̂��߁A�A�^�b�`���Ȃ��ł��������B
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