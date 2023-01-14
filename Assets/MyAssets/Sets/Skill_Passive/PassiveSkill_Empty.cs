using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PassiveSkill_Empty : PassiveSkill
{
    protected override void UseSkill()
    {
        // このパッシブスキルがついているキャラクターは常時攻撃力が1.5倍になります。
        userStatus.Atk = (int)(userStatus.Atk * 1.5);
        Debug.Log("パッシブスキルを発動！");
    }

    protected override void OnDisable()
    {
        // このパッシブスキルが解除されたキャラクターは攻撃力が元に戻ります。
        userStatus.Atk = (int)(userStatus.Atk / 1.5);
        Debug.Log("パッシブスキルを解除…。");
    }
}
