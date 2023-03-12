using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Walk : ActiveSkill
{
    public override void SkillContent()
    {
        user.Action_time = freeze_time;
        Debug.Log(user.gameObject.name + "は前に歩き始めた。");
        StartCoroutine("Walking");
    }

    private IEnumerator Walking()
    {
        Vector3 v3 = user_transform.forward * user_status.Speed;
        float time = Random.Range(0.5f, 1.0f) * freeze_time;
        user.Velocity = v3;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        user.Velocity = Vector3.zero;
        Debug.Log(user.gameObject.name + "は立ち止まった。");
    }
}
