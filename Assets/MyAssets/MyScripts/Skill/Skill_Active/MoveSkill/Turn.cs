using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Turn : NonTargetingSkill
{
    private Transform userTransform;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userTransform = user.GetComponent<Transform>();
    }

    public override void SkillContent()
    {
        int rnd = Random.Range(-180, 181);
        Quaternion quaternion = Quaternion.AngleAxis(rnd, userTransform.up);
        user.SetRotation(quaternion);
        userTransform.rotation = Quaternion.RotateTowards(userTransform.rotation, quaternion, userStatus.Speed * Time.deltaTime);
        user.SetActionTime(freezeTime);

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