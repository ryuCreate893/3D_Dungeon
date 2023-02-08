using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// シーン遷移の役割を担います。
class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    [SerializeField, Tooltip("メインシーン名をセットします。")]
    private string mainScene;
    private string pauseScene = "PauseScene";
    private string informationScene = "InformationScene";

    /// <summary>
    /// インフォメーション画面とメイン画面を行き来します。
    /// </summary>
    private bool information = false;

    /// <summary>
    /// ポーズ画面とメイン画面を行き来します。
    /// </summary>
    private bool pause = false;

    /// <summary>
    /// 特定の条件のとき、操作を受け付けないようにします(イベント中など)
    /// </summary>
    private bool operate = false;

    /// <summary>
    /// シーン遷移の時間間隔
    /// </summary>
    private float maxTransitionTime = 0.5f;

    /// <summary>
    /// シーン遷移の時間間隔
    /// </summary>
    private float transitionTime = 0;

    private void Awake()
    {
        if (sceneInstance == null)
        {
            CursorSetter();
            sceneInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (transitionTime > 0) // シーン遷移中は操作を受け付けない
        {
            transitionTime -= Time.unscaledDeltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (!pause)
                {
                    StartCoroutine(CallOptionScene(pauseScene));
                }
                else
                {
                    StartCoroutine(ReturnMainScene(pauseScene));
                }
            }
            else if (Input.GetButtonDown("Information") && !operate)
            {
                if (!information)
                {
                    StartCoroutine(CallOptionScene(informationScene));
                }
                else
                {
                    StartCoroutine(ReturnMainScene(informationScene));
                }
            }
        }
    }

    private void CursorSetter()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined; // 画面内でのみカーソルを移動させる
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// 引数にインフォメーションシーン, ポーズシーンのどちらかを指定して、メインシーンから一時的に切り替えます。
    /// </summary>
    private IEnumerator CallOptionScene(string scene)
    {
        transitionTime = maxTransitionTime;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(transitionTime);
        CursorSetter(); // シーンが切り替わる直前にカーソルを表示させる
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// オプションシーンからメインシーンに戻ります。
    /// </summary>
    private IEnumerator ReturnMainScene(string scene)
    {
        transitionTime = maxTransitionTime;

        yield return new WaitForSecondsRealtime(transitionTime);
        CursorSetter(); // シーンが切り替わった直後にカーソルを消す
        SceneManager.UnloadSceneAsync(scene);
        Time.timeScale = 1;
    }

    /// <summary>
    /// 引数に新しいメインシーンを指定して、メインシーンを再設定します。
    /// </summary>
    public static IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;

        // 暗転描写
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // メインシーン再設定後の明転描写
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1;
    }
}