using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* *** 継承できるクラスの種類 ***
 * NonTargetingSkill   ... 標的を持たず、チャージもしないスキルです。
 * NonTargetingSkill_C ... 標的を持たず、チャージが必要なスキルです。
 * TargetingSkill      ... 標的を持ち、チャージはしないスキルです。
 * TargetingSkill_C    ... 標的を持ち、チャージも必要なスキルです。
 */

class SampleSkill : TargetingSkill_C//: 【継承できるクラスの種類】から選択します。
{
    public override void SkillContent()
    {

    }
}
