using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenFaderUI : MonoBehaviour
{
    [SerializeField, Tooltip("canvas��ɂ���'Screen'���Z�b�g���܂�")]
    private Image sc;
    [SerializeField, Tooltip("canvas��ɂ���'ScreenText'���Z�b�g���܂�")]
    private TextMeshProUGUI st;

    /// <summary>
    /// �����x�̕ύX(Image)
    /// </summary>
    public void SetScreenAlpha(float alpha)
    {
        sc.color = new Color(sc.color.r, sc.color.g, sc.color.b, alpha);
    }

    /// <summary>
    /// �����x�̕ύX(TextMeshProUGUI)
    /// </summary>
    public void SetTextAlpha(float alpha)
    {
        st.color = new Color(st.color.r, st.color.g, st.color.b, alpha);
    }

    /// <summary>
    /// �e�L�X�g���e�̕ύX(TextMeshProUGUI)
    /// </summary>
    public void SetTextString(string text)
    {
        st.text = text;
    }


    // *** �t�F�[�h�C���֐� ***
    /// <summary>
    /// ���S�ɖ��邭�Ȃ�܂Ńt�F�[�h�C�����s���܂�(Image)
    /// </summary>
    public IEnumerator ScreenFadeIn(float max_time)
    {
        float min_col = 1; // �ŏ��l�܂Ńt�F�[�h�C��
        yield return StartCoroutine(ScreenFadeIn(max_time, min_col));
    }

    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�ăt�F�[�h�C�����s���܂�(Image)
    /// </summary>
    public IEnumerator ScreenFadeIn(float max_time, float sub)
    {
        float time = 0;
        float start = sc.color.a;
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
            sc.color = new Color(sc.color.r, sc.color.g, sc.color.b, start - alpha);
        }
    }

    /// <summary>
    /// ���S�ɖ��邭�Ȃ�܂Ńt�F�[�h�C�����s���܂�(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeIn(float max_time)
    {
        float min_col = 1; // �ŏ��l�܂Ńt�F�[�h�C��
        yield return StartCoroutine(TextFadeIn(max_time, min_col));
    }

    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�ăt�F�[�h�C�����s���܂�(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeIn(float max_time, float sub)
    {
        float time = 0;
        float start = st.color.a;
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
            st.color = new Color(st.color.r, st.color.g, st.color.b, start - alpha);
        }
    }


    // *** �t�F�[�h�A�E�g�֐� ***
    /// <summary>
    /// ���S�ɈÂ��Ȃ�܂Ńt�F�[�h�A�E�g���s���܂�(Image)
    /// </summary>
    public IEnumerator ScreenFadeOut(float max_time)
    {
        float max_col = 1.0f; // �ő�l�܂Ńt�F�[�h�A�E�g
        yield return StartCoroutine(ScreenFadeOut(max_time, max_col));
    }

    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�ăt�F�[�h�A�E�g���s���܂�(Image)
    /// </summary>
    public IEnumerator ScreenFadeOut(float max_time, float add)
    {
        float time = 0;
        float start = sc.color.a;
        float add_alpha;
        if (start + add > 1)
        {
            add_alpha = 1 - start;
        }
        else
        {
            add_alpha = add;
        }

        while (time < max_time)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            sc.color = new Color(sc.color.r, sc.color.g, sc.color.b, start + alpha);
        }
    }

    /// <summary>
    /// ���S�ɈÂ��Ȃ�܂Ńt�F�[�h�A�E�g���s���܂�(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeOut(float max_time)
    {
        float max_col = 1.0f; // �ő�l�܂Ńt�F�[�h�A�E�g
        yield return StartCoroutine(TextFadeOut(max_time, max_col));
    }

    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�ăt�F�[�h�A�E�g���s���܂�(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeOut(float max_time, float add)
    {
        float time = 0;
        float start = st.color.a;
        float add_alpha;
        if (start + add > 1)
        {
            add_alpha = 1 - start;
        }
        else
        {
            add_alpha = add;
        }

        while (time < max_time)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            st.color = new Color(st.color.r, st.color.g, st.color.b, start + alpha);
        }
    }
}