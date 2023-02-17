using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Walk : ActiveSkill
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
        user._velocity = v3;
        user._actionTime = freezeTime;
        Debug.Log(user.gameObject.name + "�͑O�ɕ����n�߂��B");
        float rnd = Random.Range(0.5f, 1.0f);
        Invoke("Stop", freezeTime * rnd);
    }

    private void Stop()
    {
        user._velocity = Vector3.zero;
        Debug.Log(user.gameObject.name + "�͗����~�܂����B");
    }
}
