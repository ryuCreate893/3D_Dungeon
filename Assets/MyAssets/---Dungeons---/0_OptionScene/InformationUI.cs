using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationUI : MonoBehaviour
{
    // *** �L�����o�X�̐ݒ� ***
    [SerializeField, Tooltip("fader_UI_object���Z�b�g���܂��B")]
    private GameObject fade_screen_UI_object;

    [SerializeField, Tooltip("�t�F�[�h�C���E�A�E�g�p�̉�ʂ𕢂��I�u�W�F�N�g")]
    private Image screen;

    [SerializeField, Tooltip("�t�F�[�h�A�E�g���ɕ\������e�L�X�g")]
    private TextMeshProUGUI fade_text;

    /// <summary>
    /// ���S�ɈÓ]����܂Ńt�F�[�h�A�E�g���s���܂��B
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeOutScreen(float time)
    {
        yield return StartCoroutine(FadeOutScreen(time, 1.0f)); // ���S�ɈÂ����鏈��
    }

    /// <summary>
    /// �Ó]�̓x�����𐔒l�Ŏw�肵�ăt�F�[�h�A�E�g���s���܂�(�͈�:0-1, 1�ɋ߂Â����Â�����)
    /// </summary>
    /// <param name="time"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public IEnumerator FadeOutScreen(float time, float add)
    {
        float max_time = time;
        float start = screen.color.a;
        float add_alpha;
        if (start + add > 1)
        {
            add_alpha = 1 - start;
        }
        else
        {
            add_alpha = add;
        }

        while (time > 0)
        {
            yield return null;
            time -= Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, 0) / max_time * add_alpha;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, start + alpha);
        }
    }

    public IEnumerator FadeInScreen(float time)
    {
        yield return StartCoroutine(FadeOutScreen(time, 0)); // ���S�ɖ��邭���鏈��
    }

    public IEnumerator FadeInScreen(float max_time, float sub)
    {
        float time = 0;
        float start = screen.color.a;
        float sub_alpha;
        if (start - sub < 0)
        {
            sub_alpha = start;
        }
        else
        {
            sub_alpha = sub;
        }

        while (time < max_time)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * sub_alpha;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, start - alpha);
        }

        if(screen.color.a == 0)
        {

        }
    }
}