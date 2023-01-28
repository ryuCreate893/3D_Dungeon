using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SampleSkill_Charge : ActiveSkill
{
    [SerializeField, Tooltip("このスキルはチャージを必要とします。")]
    private ChargeSkill chargeSkill;

    public override void SkillContent()
    {
        // スキルの内容を記述します。
        Debug.Log(gameObject.name + "を発動！");
    }

    public override int TrySkill()
    {
        if (UseJudge())
        {
            if (chargeSkill.Charging)
            {
                SkillContent();
                userMethod.ActionTimeSet(freezeTime);
                chargeSkill.Charging = false;
            }
            else
            {
                userMethod.ActionTimeSet(chargeSkill.ChargeTime);
                chargeSkill.Charging = true;
                Debug.Log(gameObject.name + "のチャージを開始！");
                return skill_number;
            }
        }
        else
        {
            Debug.Log(gameObject.name + "は発動に失敗した…。");
            CancelSkill();
        }
        return -1;
    }

    public override void DamagedCancel()
    {
        if (chargeSkill.Charging && chargeSkill.DamagedCancel)
        {
            CancelSkill();
        }
    }

    public int CancelSkill()
    {
        chargeSkill.Charging = false;
        userMethod.ActionTimeSet(0);
        return -1;
    }
}