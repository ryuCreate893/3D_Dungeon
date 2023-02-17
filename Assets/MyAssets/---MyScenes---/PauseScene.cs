using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ポーズシーンでやること
 * 1.ポーズシーンのCanvasに半透明の黒背景を置いておく
 * 2.PAUSE文字のカラーを設定する
 * 3.このスクリプトをアタッチする
 */

class PauseScene : MonoBehaviour
{
    private float maxTime = 1.5f;
    private float time = 0;
    private bool reverse = false;

    [SerializeField]
    private GameObject text;

    private void Awake()
    {
        time = maxTime;
    }

    private void Update()
    {
        if (text != null)
        {
            if (reverse)
            {
                time -= Time.unscaledDeltaTime;
                if (time <= 0)
                {
                    reverse = !reverse;
                    time = 0;
                }
            }
            else
            {
                time += Time.unscaledDeltaTime;
                if (time >= maxTime)
                {
                    reverse = !reverse;
                    time = maxTime;
                }
            }
            // 色を変えます。
        }
    }
}