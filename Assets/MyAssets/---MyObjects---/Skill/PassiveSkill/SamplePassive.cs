using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class SamplePassive : PassiveSkill
{
    public override void SkillContent()
    {
        user_status.AddAtk += 100;
        user_status.Atk += user_status.AddAtk;
    }

    public override void Relieve()
    {
        int n = user_status.AddAtk;
        user_status.Atk -= n;
        user_status.AddAtk -= n;
    }

    protected override void OnDisable()
    {
        Relieve();
    }
}