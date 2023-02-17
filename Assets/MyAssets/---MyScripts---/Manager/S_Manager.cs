using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �V�[���J�ڂ̖�����S���܂��B
class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    // *** �V�[���ꗗ ***
    private string mainScene;
    private string pauseScene = "PauseScene";
    private string informationScene = "InformationScene";

    /// <summary>
    /// ���C���J�����I�u�W�F�N�g���擾���܂��B
    /// </summary>
    private AudioListener cameraSound;

    /// <summary>
    /// �C���t�H���[�V������ʂƃ��C����ʂ��s�������܂��B
    /// </summary>
    private bool information = false;

    /// <summary>
    /// �|�[�Y��ʂƃ��C����ʂ��s�������܂��B
    /// </summary>
    private bool pause = false;

    /// <summary>
    /// ����̏����̂Ƃ��A������󂯕t���Ȃ��悤�ɂ��܂�(�C�x���g���Ȃ�)
    /// </summary>
    public bool Operate { get; set; } = false;

    /// <summary>
    /// �V�[���J�ڂ̎��ԊԊu
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
        if (transitionTime > 0) // �V�[���J�ڒ��͑�����󂯕t���Ȃ�
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

    // *** �|�[�Y�V�[���̐ݒ� ***
    /// <summary>
    /// �|�[�Y�V�[���̌Ăяo�����s���܂��B
    /// </summary>
    private void CallPauseScene()
    {
        Time.timeScale = 0;
        CursorSetter();
        cameraSound.enabled = false;
        SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// �|�[�Y�V�[�����I�����܂��B
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


    // *** �C���t�H���[�V�����V�[���̐ݒ� ***
    /// <summary>
    /// �C���t�H���[�V�����V�[���̌Ăяo�����s���܂��B
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
    /// �C���t�H���[�V�����V�[�����I�����܂��B
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


    // *** ���C���V�[���̐ݒ� ***
    /// <summary>
    /// �����ɐV�������C���V�[�����w�肵�āA���C���V�[�����Đݒ肵�܂��B
    /// </summary>
    public static IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;

        // �Ó]�`��
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // �}�b�v���`��
        yield return new WaitForSecondsRealtime(1.5f);
        D_Manager.gameInstance.SetFloor();

        // ���]�`��
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1;
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
            Cursor.lockState = CursorLockMode.Confined; // ��ʓ��ł̂݃J�[�\�����ړ�������
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}