using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : Character
{

    protected float rooteen; // 行動の間隔(スキル、移動などで設定)
    protected bool found; // プレイヤーの知覚状態(true=気づいている)

    protected virtual void Update()
    {
        if (0 < rooteen)
        {
            rooteen -= Time.deltaTime;
        }
        else
        {
            rooteen = UseSkill();
            // スキル(攻撃含む)が使われなかった場合
            if (rooteen <= 0)
            {
                // 移動ルーチンを起動
                rooteen = Move();
            }
        }
    }

    public float UseSkill()
    {
        float f = 0; // 次の行動まで"f"フレームだけ間隔を空ける
        for (int i = 0; i < activeSkill.Count; i++)
        {
            f = activeSkill[i].UseSkillCheck();
            if (f != 0) return f;
        }
        return f;
    }

    public float Move()
    {
        return 3; // ランダムでいろんな方向に動く処理を実装予定
    }
}