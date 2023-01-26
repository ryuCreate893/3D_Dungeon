using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Move : Mover
{
    /// <summary>
    /// キャラクターが取得する移動ベクトル用の変数です。
    /// </summary>
    

    public override Vector3 SetVelocity()
    {
        return user.transform.forward;
    }
}
