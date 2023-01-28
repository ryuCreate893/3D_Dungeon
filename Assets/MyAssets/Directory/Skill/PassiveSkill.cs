using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class PassiveSkill : Skill
{
    protected FloatStatus userFloat;

    public override void UserSet(GameObject character, int i)
    {
        userFloat = character.GetComponent<FloatStatus>();
        base.UserSet(character, i);
    }

    abstract public void Relieve();
    abstract protected void OnDisable();
}