using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PassiveSkill : Skill
{
    [SerializeField]
    private float atkUP = 1.5f;

    protected override void SkillContent()
    {
        // 例:攻撃力を永続的に増加させます。
        userStatus.Atk = (int)(userStatus.Atk * atkUP);
    }

    public override void CancelSkill()
    {
        // 例:スキルが解除されると攻撃力が元に戻ります。
        userStatus.Atk = (int)(userStatus.Atk / atkUP);
    }

    public override void TrySkill()
    {
        // 無条件発動のパッシブスキルです。
        SkillContent();
    }

    private void OnDisable()
    {
        CancelSkill();
    }
}
