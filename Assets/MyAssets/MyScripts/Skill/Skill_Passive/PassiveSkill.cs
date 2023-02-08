using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class PassiveSkill : Skill
{
    protected FloatStatus userFloat;

    public override void SetSkill(GameObject character)
    {
        base.SetSkill(character);
        userFloat = character.GetComponent<FloatStatus>();
    }

    abstract public void Relieve();
    abstract protected void OnDisable();
}