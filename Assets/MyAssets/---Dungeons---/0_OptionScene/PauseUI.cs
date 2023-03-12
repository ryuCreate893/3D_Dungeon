using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // *** キャンバスの設定 ***
    private float time = 1.0f;
    private float max_time = 1.0f;
    private bool isFadeIn = true;
    private bool isPause = false;

    private void Update()
    {
        // ポーズ画面でポーズボタンを押したとき、ポーズ中の処理を行わないようにします。
        if (Input.GetButtonDown("Pause") && !isPause)
        {
            isPause = true;
        }
    }

    private void LateUpdate()
    {
        if (!isPause)
        {
            float alpha;
            if (isFadeIn)
            {
                time -= Time.unscaledDeltaTime;
                alpha = Mathf.Max(0, time) / max_time;
                if (alpha == 0)
                {
                    time = 0;
                    isFadeIn = false;
                }
            }
            else
            {
                time += Time.unscaledDeltaTime;
                alpha = Mathf.Min(max_time, time) / max_time;
                if (alpha == max_time)
                {
                    time = max_time;
                    isFadeIn = true;
                }
            }
            UI_Manager.UIInstance.Sf.SetTextAlpha(alpha);
        }
    }
}