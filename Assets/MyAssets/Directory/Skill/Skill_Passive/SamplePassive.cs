using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class SamplePassive : PassiveSkill
{
    public override void SkillContent()
    {
        userFloat.Atk *= 1.5f;
        userStatus.Atk = (int)userFloat.Atk;
    }

    public override void Relieve()
    {
     userFloat.Atk /= 1.5f;
     userStatus.Atk = (int) userFloat.Atk;
    }

    protected override void OnDisable()
    {
        Relieve();
    }
}