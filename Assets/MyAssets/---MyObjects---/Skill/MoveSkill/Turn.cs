using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : ActiveSkill
{
    public override void SkillContent()
    {
        user.Velocity = Vector3.zero;
        int rnd = Random.Range(-180, 181);
        user.Character_rot = Quaternion.AngleAxis(rnd, user_transform.up);
        user.Action_time = freeze_time;

        if (rnd < 0)
        {
            Debug.Log(user.gameObject.name + "�͍���" + rnd + "�x��]�����B");
        }
        else
        {
            Debug.Log(user.gameObject.name + "�͉E��" + rnd + "�x��]�����B");
        }
    }
}
