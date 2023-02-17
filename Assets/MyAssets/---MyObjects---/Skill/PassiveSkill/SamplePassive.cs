using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class SamplePassive : PassiveSkill
{
    public override void SkillContent()
    {
        userStatus.AddAtk += (int)(userStatus.FloatAtk * 1.5f);
        userStatus.Atk += userStatus.AddAtk;
    }

    public override void Relieve()
    {
        int n = userStatus.AddAtk;
        userStatus.Atk -= n;
        userStatus.AddAtk -= n;
    }

    protected override void OnDisable()
    {
        Relieve();
    }
}