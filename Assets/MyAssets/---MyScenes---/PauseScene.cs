using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PauseScene : MonoBehaviour
{
    [SerializeField, Tooltip("専用canvas上にある'ScreenText'をセットします。\n'PAUSE'の文字透明度を編集します。")]
    private Color st_col;

    private float max_time = 1.5f;
    private float time = 0;
    private bool reverse = false;

    private void Update()
    {
        if (st_col.a > 0) // 完全に透明な場合はS_Managerで処理を行わせる
        {
            if (reverse)
            {
                time += Time.unscaledDeltaTime;
                if (time > max_time)
                {
                    reverse = !reverse;
                    time = max_time;
                }
            }
            else
            {
                time -= Time.unscaledDeltaTime;
                if (time < 0)
                {
                    reverse = !reverse;
                    time = 0;
                }
            }

            float alpha = Mathf.Max(1 / 255, time / max_time);
            st_col = new Color(st_col.r, st_col.g, st_col.b, alpha);
        }
    }
}