using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** シーン一覧 ***
    private string pause_scene = "PauseScene";
    private string info_scene = "InformationScene";

    // *** コンポーネント一覧 ***
    private Transform camera_transform;
    private AudioListener camera_sound;

    // *** 状態管理用変数一覧 ***
    /// <summary>
    /// インフォメーション画面とメイン画面を行き来します。
    /// </summary>
    private bool info = false;

    /// <summary>
    /// ポーズ画面とメイン画面を行き来します。
    /// </summary>
    private bool pause = false;

    /// <summary>
    /// シーン遷移中、操作を受け付けないようにします。
    /// </summary>
    private bool transition = false;

    /// <summary>
    /// シーン遷移中以外の特定の条件で、操作を受け付けないようにします(イベント中など)
    /// </summary>
    public bool Operate { get; set; } = false;

    private void Awake()
    {
        if (sceneInstance == null)
        {
            sceneInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UI_Manager.UIInstance.HideCursor();
        GetMainCamera();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !transition)
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
        else if (Input.GetButtonDown("Information") && !transition && !Operate)
        {
            if (!info)
            {
                StartCoroutine(CallInformationScene());
            }
            else
            {
                StartCoroutine(CloseInformationScene());
            }
        }
    }

    // *** ポーズシーンの設定 ***
    private void CallPauseScene()
    {
        pause = true;
        Time.timeScale = 0;

        UI_Manager.UIInstance.Sf.SetScreenAlpha(0.5f); // スクリーンは半透明
        UI_Manager.UIInstance.Sf.SetTextAlpha(1.0f); // 文字は不透明
        UI_Manager.UIInstance.Sf.SetTextString("PAUSE"); // 文字は不透明
        UI_Manager.UIInstance.ShowCursor();

        camera_sound.enabled = false;
        SceneManager.LoadScene(pause_scene, LoadSceneMode.Additive);
        ChangeMainCamera();
    }

    private IEnumerator ClosePauseScene()
    {
        pause = false;
        transition = true;

        UI_Manager.UIInstance.Sf.SetScreenAlpha(0.5f); // スクリーンは半透明
        UI_Manager.UIInstance.Sf.SetTextAlpha(0); // 文字は透明
        UI_Manager.UIInstance.HideCursor(); // マウスカーソルの非表示

        UI_Manager.UIInstance.Sf.SetTextAlpha(0); // 文字は透明
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeIn(0.3f));

        SceneManager.UnloadSceneAsync(pause_scene);
        camera_sound.enabled = true;
        transition = false;
        if (!Operate) Time.timeScale = 1;
    }


    // *** インフォメーションシーンの設定 ***
    private IEnumerator CallInformationScene()
    {
        info = true;
        transition = true;
        Time.timeScale = 0;

        // Informationウィンドウの処理待ち時間
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeOut(0.5f, 0.5f));
        camera_sound.enabled = false;
        SceneManager.LoadScene(pause_scene, LoadSceneMode.Additive);
        ChangeMainCamera();
        yield return new WaitForSecondsRealtime(1.0f);

        UI_Manager.UIInstance.ShowCursor();
        transition = false;
    }

    private IEnumerator CloseInformationScene()
    {
        info = false;
        transition = true;
        UI_Manager.UIInstance.HideCursor();

        // Informationウィンドウの処理待ち時間
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.UnloadSceneAsync(info_scene);
        UI_Manager.UIInstance.Sf.SetScreenAlpha(0);
        camera_sound.enabled = true;
        transition = false;
        Time.timeScale = 1;
    }

    // *** メインシーンの設定 ***
    /// <summary>
    /// メインシーンを再設定します("D_Manager"より呼び出し)
    /// </summary>
    public IEnumerator SetNewMainScene(string scene_name, string text)
    {
        Time.timeScale = 0;
        transition = true;

        // フェードアウト
        UI_Manager.UIInstance.DestroyExternalUI();
        UI_Manager.UIInstance.HideCursor();
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeOut(0.5f));

        // シーンの切り替えとオブジェクト配置
        SceneManager.LoadScene(scene_name);
        GetMainCamera();
        D_Manager.gameInstance.SetFloor();
        yield return new WaitForSecondsRealtime(0.5f);

        // マップ名の表示
        UI_Manager.UIInstance.Sf.SetTextString(text);
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.TextFadeOut(0.25f));
        yield return new WaitForSecondsRealtime(1.0f);

        // フェードイン
        StartCoroutine(UI_Manager.UIInstance.Sf.TextFadeIn(0.5f));
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeIn(0.5f));
        GetMainCamera();

        Operate = false;
        transition = false;
        Time.timeScale = 1;
    }

    // *** シーンの更新時に呼び出すメソッド ***
    /// <summary>
    /// メインカメラの"Transform", "AudioListener"コンポーネントを取得します。
    /// </summary>
    private void GetMainCamera()
    {
        camera_transform = Camera.main.gameObject.GetComponent<Transform>();
        camera_sound = Camera.main.gameObject.GetComponent<AudioListener>();
    }

    /// <summary>
    /// メインカメラの"Transform", "AudioListener"を追加シーンのメインカメラにコピーします。
    /// </summary>
    private void ChangeMainCamera()
    {
        Camera.main.gameObject.transform.position = camera_transform.position;
        Camera.main.gameObject.transform.rotation = camera_transform.rotation;
    }
}