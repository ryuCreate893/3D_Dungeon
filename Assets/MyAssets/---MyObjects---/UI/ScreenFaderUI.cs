using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenFaderUI : MonoBehaviour
{
    [SerializeField, Tooltip("canvas上にある'Screen'をセットします")]
    private Image sc;
    [SerializeField, Tooltip("canvas上にある'ScreenText'をセットします")]
    private TextMeshProUGUI st;

    /// <summary>
    /// 透明度の変更(Image)
    /// </summary>
    public void SetScreenAlpha(float alpha)
    {
        sc.color = new Color(sc.color.r, sc.color.g, sc.color.b, alpha);
    }

    /// <summary>
    /// 透明度の変更(TextMeshProUGUI)
    /// </summary>
    public void SetTextAlpha(float alpha)
    {
        st.color = new Color(st.color.r, st.color.g, st.color.b, alpha);
    }

    /// <summary>
    /// テキスト内容の変更(TextMeshProUGUI)
    /// </summary>
    public void SetTextString(string text)
    {
        st.text = text;
    }


    // *** フェードイン関数 ***
    /// <summary>
    /// 完全に明るくなるまでフェードインを行います(Image)
    /// </summary>
    public IEnumerator ScreenFadeIn(float max_time)
    {
        float min_col = 1; // 最小値までフェードイン
        yield return StartCoroutine(ScreenFadeIn(max_time, min_col));
    }

    /// <summary>
    /// 第3引数にalpha値の変化量を指定してフェードインを行います(Image)
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
    /// 完全に明るくなるまでフェードインを行います(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeIn(float max_time)
    {
        float min_col = 1; // 最小値までフェードイン
        yield return StartCoroutine(TextFadeIn(max_time, min_col));
    }

    /// <summary>
    /// 第3引数にalpha値の変化量を指定してフェードインを行います(TextMeshProUGUI)
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


    // *** フェードアウト関数 ***
    /// <summary>
    /// 完全に暗くなるまでフェードアウトを行います(Image)
    /// </summary>
    public IEnumerator ScreenFadeOut(float max_time)
    {
        float max_col = 1.0f; // 最大値までフェードアウト
        yield return StartCoroutine(ScreenFadeOut(max_time, max_col));
    }

    /// <summary>
    /// 第3引数にalpha値の変化量を指定してフェードアウトを行います(Image)
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
    /// 完全に暗くなるまでフェードアウトを行います(TextMeshProUGUI)
    /// </summary>
    public IEnumerator TextFadeOut(float max_time)
    {
        float max_col = 1.0f; // 最大値までフェードアウト
        yield return StartCoroutine(TextFadeOut(max_time, max_col));
    }

    /// <summary>
    /// 第3引数にalpha値の変化量を指定してフェードアウトを行います(TextMeshProUGUI)
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