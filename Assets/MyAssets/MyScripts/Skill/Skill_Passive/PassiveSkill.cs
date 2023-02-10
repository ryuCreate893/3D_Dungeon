using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *** このスクリプトは継承用のため、アタッチしないでください。
abstract class PassiveSkill : Skill
{

    abstract public void Relieve();
    abstract protected void OnDisable();
}