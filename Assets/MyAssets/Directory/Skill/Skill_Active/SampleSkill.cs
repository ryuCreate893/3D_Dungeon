using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SampleSkill : ActiveSkill
{
    public override void SkillContent()
    {
        // スキルの内容を記述します。
        Debug.Log(gameObject.name + "を発動！");
    }

    public override int TrySkill()
    {
        if (UseJudge())
        {
            SkillContent();
            userMethod.ActionTimeSet(freezeTime);
        }
        else
        {
            Debug.Log(gameObject.name + "は発動に失敗した…。");
        }
        return -1;
    }

    public override void DamagedCancel(){}
}