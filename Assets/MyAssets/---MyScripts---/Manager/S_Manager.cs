using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* Project�r���[���canvas"S_Manager_canvas", GameObject"ScreenFader"��
 * �쐬���āAScreenFader��S_Manager�̃R���|�[�l���g�Ƃ���Inspector�œo�^
*/

class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** �V�[���ꗗ ***
    private string main_scene;
    private string pause_scene = "PauseScene";
    private string info_scene = "InformationScene";

    // *** �R���|�[�l���g�ꗗ ***
    /// <summary>
    /// ���C���J������Transform���擾���܂��B
    /// </summary>
    private Transform camera_transform;
    /// <summary>
    /// ���C���J������AudioListner�R���|�[�l���g���擾���܂��B
    /// </summary>
    private AudioListener camera_sound;

    [SerializeField, Tooltip("��pcanvas��ɂ���'ScreenFader'���Z�b�g���܂��B\n�t�F�[�h�C���E�t�F�[�h�A�E�g�Ɏg�p���܂��B")]
    private Image sf;
    [SerializeField, Tooltip("��pcanvas��ɂ���'ScreenText'���Z�b�g���܂��B\n�X�e�[�W���̃t�F�[�h�C���E�t�F�[�h�A�E�g�Ɏg�p���܂��B")]
    private TextMeshProUGUI st;

    // *** ��ԊǗ��p�ϐ��ꗗ ***
    /// <summary>
    /// �C���t�H���[�V������ʂƃ��C����ʂ��s�������܂��B
    /// </summary>
    private bool info = false;

    /// <summary>
    /// �|�[�Y��ʂƃ��C����ʂ��s�������܂��B
    /// </summary>
    private bool pause = false;

    /// <summary>
    /// �V�[���J�ڒ��̔�����s���܂��B
    /// </summary>
    private bool transition;

    /// <summary>
    /// ����̏����̂Ƃ��A������󂯕t���Ȃ��悤�ɂ��܂�(�C�x���g���Ȃ�)
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

    // *** �|�[�Y�V�[���̐ݒ� ***
    /// <summary>
    /// �|�[�Y�V�[���̌Ăяo�����s���܂��B
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
    /// �|�[�Y�V�[�����I�����܂��B
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


    // *** �C���t�H���[�V�����V�[���̐ݒ� ***
    /// <summary>
    /// �C���t�H���[�V�����V�[���̌Ăяo�����s���܂��B
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

        // Information�E�B���h�E�̏����҂�����
        yield return new WaitForSecondsRealtime(time * 2.0f);
        transition = false;
    }

    /// <summary>
    /// �C���t�H���[�V�����V�[�����I�����܂��B
    /// </summary>
    private IEnumerator CloseInformationScene()
    {
        transition = true;
        float time = 1.0f;
        CursorSetter();

        // Information�E�B���h�E�̏����҂�����
        yield return new WaitForSecondsRealtime(time);

        SceneManager.UnloadSceneAsync(info_scene);
        sf.color = new Color(0, 0, 0, 0);
        camera_sound.enabled = true;
        Time.timeScale = 1;
    }

    // *** ���C���V�[���̐ݒ� ***
    /// <summary>
    /// �����ɐV�������C���V�[�����w�肵�āA���C���V�[�����Đݒ肵�܂��B
    /// </summary>
    public IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;
        transition = true;
        float time = 0.5f;

        // �Ó]����
        yield return StartCoroutine(AppearColor(sf, time));

        yield return new WaitForSecondsRealtime(time);

        // �}�b�v���`��
        string name = D_Manager.gameInstance.Dungeon.DungeonName;
        int floor = D_Manager.gameInstance.Floor + 1;
        st.text = name + "\n" + floor + "F";
        yield return StartCoroutine(AppearColor(st, time * 0.5f));

        // �V�[���̐؂�ւ�����
        SceneManager.LoadScene(scene);
        D_Manager.gameInstance.SetFloor();
        yield return new WaitForSecondsRealtime(time * 1.5f);

        // ���]����
        StartCoroutine(EraseColor(sf, time));
        yield return StartCoroutine(EraseColor(st, time));

        transition = false;
        Time.timeScale = 1;
    }

    private IEnumerator EraseColor(Image obj, float max_time)
    {
        float time = 0;
        float start = obj.color.a;

        while (time < max_time) // alpha�l��0��
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

        while (time < max_time) // alpha�l��0��
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

        while (time < max_time) // alpha�l��1��
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

        while (time < max_time) // alpha�l��0��
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * (1 - start);
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }
    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�āA���t�F�[�h�A�E�g�Ȃǂ��s���܂��B
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

        while (time < max_time) // alpha�l��1��
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }
    /// <summary>
    /// ��3������alpha�l�̕ω��ʂ��w�肵�āA���t�F�[�h�A�E�g�Ȃǂ��s���܂��B
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

        while (time < max_time) // alpha�l��1��
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Min(time, max_time) / max_time * add_alpha;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, start + alpha);
        }
    }

    /// <summary>
    /// �_���W�����̍ŉ����ɓ��B�����Ƃ���p�̃��C���V�[����ݒ肵�܂��B
    /// </summary>
    public static IEnumerator SetLastMainScene(string scene)
    {
        Time.timeScale = 0;

        // �Ó]�`��
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // �}�b�v���`��
        yield return new WaitForSecondsRealtime(2.0f);

        // ���]�`��
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1;
    }

    /// <summary>
    /// �V�[���؂�ւ����ɃJ�[�\���̕\���ݒ���s���܂��B
    /// </summary>
    private void CursorSetter()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined; // ��ʓ��ł̂݃J�[�\���ړ�
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}