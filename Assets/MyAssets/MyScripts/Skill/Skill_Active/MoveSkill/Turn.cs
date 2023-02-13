using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : ActiveSkill
{
    private Transform userTransform;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userTransform = user.GetComponent<Transform>();
    }

    public override void SkillContent()
    {
        user._velocity = Vector3.zero;
        int rnd = Random.Range(-180, 181);
        user._characterRotation = Quaternion.AngleAxis(rnd, userTransform.up);
        user._actionTime = freezeTime;

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
