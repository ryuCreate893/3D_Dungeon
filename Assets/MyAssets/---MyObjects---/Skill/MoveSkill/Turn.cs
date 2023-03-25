using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : ActiveSkill
{
    public override void SkillContent()
    {
        user.My_vel = Vector3.zero;
        int rnd = Random.Range(-180, 181);
        user.My_rot = Quaternion.AngleAxis(rnd, user_transform.up);
        user.Action_time = freeze_time;

        if (rnd < 0)
        {
            Debug.Log(user.gameObject.name + "は左に" + rnd + "度回転した。");
        }
        else
        {
            Debug.Log(user.gameObject.name + "は右に" + rnd + "度回転した。");
        }
    }
}
