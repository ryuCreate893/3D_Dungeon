using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // *** �L�����o�X�̐ݒ� ***
    private float time = 1.0f;
    private float max_time = 1.0f;
    private bool isFadeIn = true;
    private bool isPause = false;

    private void Update()
    {
        // �|�[�Y��ʂŃ|�[�Y�{�^�����������Ƃ��A�|�[�Y���̏������s��Ȃ��悤�ɂ��܂��B
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