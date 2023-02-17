using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// シーン遷移の役割を担います。
class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** シーン一覧 ***
    private string mainScene;
    private string pauseScene = "PauseScene";
    private string informationScene = "InformationScene";

    /// <summary>
    /// メインカメラオブジェクトを取得します。
    /// </summary>
    private AudioListener cameraSound;

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
    public bool Operate { get; set; } = false;

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
            mainScene = SceneManager.GetActiveScene().name;
            DontDestroyOnLoad(gameObject);
            cameraSound = Camera.main.gameObject.GetComponent<AudioListener>();
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
                    CallPauseScene();
                }
                else
                {
                    StartCoroutine(ClosePauseScene());
                }
            }
            else if (Input.GetButtonDown("Information") && !Operate)
            {
                if (!information)
                {
                    StartCoroutine(CallInformationScene());
                }
                else
                {
                    StartCoroutine(CloseInformationScene());
                }
            }
        }
    }

    // *** ポーズシーンの設定 ***
    /// <summary>
    /// ポーズシーンの呼び出しを行います。
    /// </summary>
    private void CallPauseScene()
    {
        Time.timeScale = 0;
        CursorSetter();
        cameraSound.enabled = false;
        SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// ポーズシーンを終了します。
    /// </summary>
    private IEnumerator ClosePauseScene()
    {
        float time = 0.25f;
        transitionTime = time;
        CursorSetter();
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(pauseScene);
        cameraSound.enabled = true;
    }


    // *** インフォメーションシーンの設定 ***
    /// <summary>
    /// インフォメーションシーンの呼び出しを行います。
    /// </summary>
    private IEnumerator CallInformationScene()
    {
        Time.timeScale = 0;
        float time = 0.5f;
        transitionTime = time;
        yield return new WaitForSecondsRealtime(time);
        CursorSetter();
        cameraSound.enabled = false;
        SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// インフォメーションシーンを終了します。
    /// </summary>
    private IEnumerator CloseInformationScene()
    {
        float time = 0.75f;
        transitionTime = time;
        CursorSetter();
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(informationScene);
        cameraSound.enabled = true;
    }


    // *** メインシーンの設定 ***
    /// <summary>
    /// 引数に新しいメインシーンを指定して、メインシーンを再設定します。
    /// </summary>
    public static IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;

        // 暗転描写
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // マップ名描写
        yield return new WaitForSecondsRealtime(1.5f);
        D_Manager.gameInstance.SetFloor();

        // 明転描写
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1;
    }

    /// <summary>
    /// ダンジョンの最奥部に到達したとき専用のメインシーンを設定します。
    /// </summary>
    public static IEnumerator SetLastMainScene(string scene)
    {
        Time.timeScale = 0;

        // 暗転描写
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // マップ名描写
        yield return new WaitForSecondsRealtime(2.0f);

        // 明転描写
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1;
    }

    /// <summary>
    /// シーン切り替え時にカーソルの表示設定を行います。
    /// </summary>
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
}