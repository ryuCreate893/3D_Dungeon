using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PauseScene : MonoBehaviour
{
    [SerializeField, Tooltip("��pcanvas��ɂ���'ScreenText'���Z�b�g���܂��B\n'PAUSE'�̕��������x��ҏW���܂��B")]
    private Color st_col;

    private float max_time = 1.5f;
    private float time = 0;
    private bool reverse = false;

    private void Update()
    {
        if (st_col.a > 0) // ���S�ɓ����ȏꍇ��S_Manager�ŏ������s�킹��
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