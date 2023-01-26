using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Mover : MonoBehaviour
{
    // *** このスクリプトはアタッチしません。 ***

    [Tooltip("移動方法が選ばれる確率(%)です。")]
    [SerializeField, Range(1, 100)]
    public int probability;

    /// <summary>
    /// アクション時に"true"を入れて、連続で同じアクションは行われないようにします。
    /// </summary>
    private bool isActioned;

    /// <summary>
    /// この移動方法を持つキャラクター本体です。
    /// </summary>
    protected GameObject user;

    /// <summary>
    /// キャラクターの移動方向です。
    /// </summary>
    protected Vector3 velocity;

    public void SetMove(GameObject character)
    {
        user = character;
    }

    public bool UseJudge()
    {
        if (!isActioned)
        {
            int rnd = Random.Range(1, 101);
            if (probability >= rnd)
            {
                SetVelocity();
                isActioned = false;
                return true;
            }
        }
        return false;
    }

    abstract public Vector3 SetVelocity();
}
