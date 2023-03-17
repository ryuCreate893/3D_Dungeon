using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** �V�[���ꗗ ***
    private string pause_scene = "PauseScene";
    private string info_scene = "InformationScene";

    // *** �R���|�[�l���g�ꗗ ***
    private Transform camera_transform;
    private AudioListener camera_sound;

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
    /// �V�[���J�ڒ��A������󂯕t���Ȃ��悤�ɂ��܂��B
    /// </summary>
    private bool transition = false;

    /// <summary>
    /// �V�[���J�ڒ��ȊO�̓���̏����ŁA������󂯕t���Ȃ��悤�ɂ��܂�(�C�x���g���Ȃ�)
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

    // *** �|�[�Y�V�[���̐ݒ� ***
    private void CallPauseScene()
    {
        pause = true;
        Time.timeScale = 0;

        UI_Manager.UIInstance.Sf.SetScreenAlpha(0.5f); // �X�N���[���͔�����
        UI_Manager.UIInstance.Sf.SetTextAlpha(1.0f); // �����͕s����
        UI_Manager.UIInstance.Sf.SetTextString("PAUSE"); // �����͕s����
        UI_Manager.UIInstance.ShowCursor();

        camera_sound.enabled = false;
        SceneManager.LoadScene(pause_scene, LoadSceneMode.Additive);
        ChangeMainCamera();
    }

    private IEnumerator ClosePauseScene()
    {
        pause = false;
        transition = true;

        UI_Manager.UIInstance.Sf.SetScreenAlpha(0.5f); // �X�N���[���͔�����
        UI_Manager.UIInstance.Sf.SetTextAlpha(0); // �����͓���
        UI_Manager.UIInstance.HideCursor(); // �}�E�X�J�[�\���̔�\��

        UI_Manager.UIInstance.Sf.SetTextAlpha(0); // �����͓���
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeIn(0.3f));

        SceneManager.UnloadSceneAsync(pause_scene);
        camera_sound.enabled = true;
        transition = false;
        if (!Operate) Time.timeScale = 1;
    }


    // *** �C���t�H���[�V�����V�[���̐ݒ� ***
    private IEnumerator CallInformationScene()
    {
        info = true;
        transition = true;
        Time.timeScale = 0;

        // Information�E�B���h�E�̏����҂�����
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

        // Information�E�B���h�E�̏����҂�����
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.UnloadSceneAsync(info_scene);
        UI_Manager.UIInstance.Sf.SetScreenAlpha(0);
        camera_sound.enabled = true;
        transition = false;
        Time.timeScale = 1;
    }

    // *** ���C���V�[���̐ݒ� ***
    /// <summary>
    /// ���C���V�[�����Đݒ肵�܂�("D_Manager"���Ăяo��)
    /// </summary>
    public IEnumerator SetNewMainScene(string scene_name, string text)
    {
        Time.timeScale = 0;
        transition = true;

        // �t�F�[�h�A�E�g
        UI_Manager.UIInstance.DestroyExternalUI();
        UI_Manager.UIInstance.HideCursor();
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeOut(0.5f));

        // �V�[���̐؂�ւ��ƃI�u�W�F�N�g�z�u
        SceneManager.LoadScene(scene_name);
        GetMainCamera();
        D_Manager.gameInstance.SetFloor();
        yield return new WaitForSecondsRealtime(0.5f);

        // �}�b�v���̕\��
        UI_Manager.UIInstance.Sf.SetTextString(text);
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.TextFadeOut(0.25f));
        yield return new WaitForSecondsRealtime(1.0f);

        // �t�F�[�h�C��
        StartCoroutine(UI_Manager.UIInstance.Sf.TextFadeIn(0.5f));
        yield return StartCoroutine(UI_Manager.UIInstance.Sf.ScreenFadeIn(0.5f));
        GetMainCamera();

        Operate = false;
        transition = false;
        Time.timeScale = 1;
    }

    // *** �V�[���̍X�V���ɌĂяo�����\�b�h ***
    /// <summary>
    /// ���C���J������"Transform", "AudioListener"�R���|�[�l���g���擾���܂��B
    /// </summary>
    private void GetMainCamera()
    {
        camera_transform = Camera.main.gameObject.GetComponent<Transform>();
        camera_sound = Camera.main.gameObject.GetComponent<AudioListener>();
    }

    /// <summary>
    /// ���C���J������"Transform", "AudioListener"��ǉ��V�[���̃��C���J�����ɃR�s�[���܂��B
    /// </summary>
    private void ChangeMainCamera()
    {
        Camera.main.gameObject.transform.position = camera_transform.position;
        Camera.main.gameObject.transform.rotation = camera_transform.rotation;
    }
}