using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationUI : MonoBehaviour
{
    // *** キャンバスの設定 ***
    [SerializeField, Tooltip("fader_UI_objectをセットします。")]
    private GameObject fade_screen_UI_object;

    [SerializeField, Tooltip("フェードイン・アウト用の画面を覆うオブジェクト")]
    private Image screen;

    [SerializeField, Tooltip("フェードアウト中に表示するテキスト")]
    private TextMeshProUGUI fade_text;

    /// <summary>
    /// 完全に暗転するまでフェードアウトを行います。
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeOutScreen(float time)
    {
        yield return StartCoroutine(FadeOutScreen(time, 1.0f)); // 完全に暗くする処理
    }

    /// <summary>
    /// 暗転の度合いを数値で指定してフェードアウトを行います(範囲:0-1, 1に近づく程暗くする)
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
        yield return StartCoroutine(FadeOutScreen(time, 0)); // 完全に明るくする処理
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