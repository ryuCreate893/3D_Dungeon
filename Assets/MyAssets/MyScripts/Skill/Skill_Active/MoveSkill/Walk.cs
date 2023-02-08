using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Walk : NonTargetingSkill
{
    private Transform userTransform;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userTransform = user.GetComponent<Transform>();
    }

    public override void SkillContent()
    {
        Vector3 v3 = userTransform.forward * userStatus.Speed;
        user.SetVelocity(v3);
        user.SetActionTime(freezeTime);
        Debug.Log(user.gameObject.name + "ÇÕëOÇ…ï‡Ç´énÇﬂÇΩÅB");
    }
}