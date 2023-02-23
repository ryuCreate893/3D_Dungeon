using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* Projectビュー上にcanvas"S_Manager_canvas", GameObject"ScreenFader"を
 * 作成して、ScreenFaderをS_ManagerのコンポーネントとしてInspectorで登録
*/

class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** シーン一覧 ***
    private string main_scene;
    private string pause_scene = "PauseScene";
    private string info_scene = "InformationScene";

    // *** コンポーネント一覧 ***
    /// <summary>
    /// メインカメラのTransformを取得します。
    /// </summary>
    private Transform camera_transform;
    /// <summary>
    /// メインカメラのAudioListnerコンポーネントを取得します。
    /// </summary>
    private AudioListener camera_sound;

    [SerializeField, Tooltip("専用canvas上にある'ScreenFader'をセットします。\nフェードイン・フェードアウトに使用します。")]
    private Image sf;
    [SerializeField, Tooltip("専用canvas上にある'ScreenText'をセットします。\nステージ名のフェードイン・フェードアウトに使用します。")]
    private TextMeshProUGUI st;

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
    /// シーン遷移中の判定を行います。
    /// </summary>
    private bool transition;

    /// <summary>
    /// 特定の条件のとき、操作を受け付けないようにします(イベント中など)
    /// </summary>
    public bool Operate { get; set; } = false;

    private void Awake()
    {
        if (sceneInstance == null)
        {
            CursorSetter();
            sceneInstance = this;
            DontDestroyOnLoad(gameObject);
            main_scene = SceneManager.GetActiveScene().name;
            camera_transform = Camera.main.gameObject.GetComponent<Transform>();
            camera_sound = Camera.main.gameObject.GetComponent<AudioListener>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !transition)
        {
            if (!pause)
            {
                CallPauseScene();
                pause = true;
            }
            else
            {
                StartCoroutine(ClosePauseScene());
                pause = false;
            }
        }
        else if (Input.GetButtonDown("Information") && !transition && !Operate)
        {
            if (!info)
            {
                StartCoroutine(CallInformationScene());
                info = true;
            }
            else
            {
                StartCoroutine(CloseInformationScene());
                info = false;
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
        Operate = true;
        camera_sound.enabled = false;
        CursorSetter();

        sf.color = new Color(0, 0, 0, 0.5f);
        st.text = "PAUSE";
        st.color = new Color(1, 1, 1, 1);

        SceneManager.LoadScene(pause_scene, LoadSceneMode.Additive);
        Camera.main.gameObject.transform.position = camera_transform.position;
        Camera.main.gameObject.transform.rotation = camera_transform.rotation;
    }

    /// <summary>
    /// ポーズシーンを終了します。
    /// </summary>
    private IEnumerator ClosePauseScene()
    {
        transition = true;
        CursorSetter();
        st.color = new Color(0, 0, 0, 0);

        float time = 0.5f;
        yield return StartCoroutine(EraseColor(sf, time));

        SceneManager.UnloadSceneAsync(pause_scene);
        camera_sound.enabled = true;
        Operate = false;
        transition = false;
        Time.timeScale = 1;
    }


    // *** インフォメーションシーンの設定 ***
    /// <summary>
    /// インフォメーションシーンの呼び出しを行います。
    /// </summary>
    private IEnumerator CallInformationScene()
    {
        Time.timeScale = 0;
        transition = true;
        camera_sound.enabled = false;
        CursorSetter();

        float time = 0.5f;
        float half_fade = 0.5f;
        yield return StartCoroutine(AppearColor(sf, time, half_fade));

        camera_sound.enabled = false;
        SceneManager.LoadScene(pause_scene, LoadSceneMode.Additive);
        Camera.main.gameObject.transform.position = camera_transform.position;
        Camera.main.gameObject.transform.rotation = camera_transform.rotation;

        // Informationウィンドウの処理待ち時間
        yield return new WaitForSecondsRealtime(time * 2.0f);
        transition = false;
    }

    /// <summary>
    /// インフォメーションシーンを終了します。
    /// </summary>
    private IEnumerator CloseInformationScene()
    {
        transition = true;
        float time = 1.0f;
        CursorSetter();

        // Informationウィンドウの処理待ち時間
        yield return new WaitForSecondsRealtime(time);

        SceneManager.UnloadSceneAsync(info_scene);
        sf.color = new Color(0, 0, 0, 0);
        camera_sound.enabled = true;
        Time.timeScale = 1;
    }

    // *** メインシーンの設定 ***
    /// <summary>
    /// 引数に新しいメインシーンを指定して、メインシーンを再設定します。
    /// </summary>
    public IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;
        transition = true;
        float time = 0.5f;

        // 暗転処理
        yield return StartCoroutine(AppearColor(sf, time));

        yield return new WaitForSecondsRealtime(time);

        // マップ名描写
        string name = D_Manager.gameInstance.Dungeon.DungeonName;
        int floor = D_Manager.gameInstance.Floor + 1;
        st.text = name + "\n" + floor + "F";
        yield return StartCoroutine(AppearColor(st, time * 0.5f));

        // シーンの切り替え処理
        SceneManager.LoadScene(scene);
        D_Manager.gameInstance.SetFloor();
        yield return new WaitForSecondsRealtime(time * 1.5f);

        // 明転処理
        StartCoroutine(EraseColor(sf, time));
        yield return StartCoroutine(EraseColor(st, time));

        transition = false;
        Time.timeScale = 1;
    }

    private IEnumerator EraseColor(Image obj, float max_time)
    {
        float time = 0;
        float start = obj.color.a;

        while (time < max_time) // alpha値を0に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * start;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start - alpha);
        }
    }
    private IEnumerator EraseColor(TextMeshProUGUI obj, float max_time)
    {
        float time = 0;
        float start = obj.color.a;

        while (time < max_time) // alpha値を0に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * start;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start - alpha);
        }
    }

    private IEnumerator AppearColor(Image obj, float max_time)
    {
        float time = 0;
        float start = obj.color.a;

        while (time < max_time) // alpha値を1に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * (1 - start);
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }
    private IEnumerator AppearColor(TextMeshProUGUI obj, float max_time)
    {
        float time = 1;
        float start = obj.color.a;

        while (time < max_time) // alpha値を0に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * (1 - start);
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }
    /// <summary>
    /// 第3引数にalpha値の変化量を指定して、半フェードアウトなどを行います。
    /// </summary>
    private IEnumerator AppearColor(Image obj, float max_time, float add)
    {
        float time = 0;
        float start = obj.color.a;
        float add_alpha;
        if (start + add > 1)
        {
            add_alpha = 1 - start;
        }
        else
        {
            add_alpha = add;
        }

        while (time < max_time) // alpha値を1に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }
    /// <summary>
    /// 第3引数にalpha値の変化量を指定して、半フェードアウトなどを行います。
    /// </summary>
    private IEnumerator AppearColor(TextMeshProUGUI obj, float max_time, float add)
    {
        float time = 0;
        float start = obj.color.a;
        float add_alpha;
        if (start + add > 1)
        {
            add_alpha = 1 - start;
        }
        else
        {
            add_alpha = add;
        }

        while (time < max_time) // alpha値を1に
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
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
            Cursor.lockState = CursorLockMode.Confined; // 画面内でのみカーソル移動
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}