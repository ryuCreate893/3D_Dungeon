using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �V�[���J�ڂ̖�����S���܂��B
class S_Manager : MonoBehaviour
{
    public static S_Manager sceneInstance;

    [SerializeField, Tooltip("���C���V�[�������Z�b�g���܂��B")]
    private string mainScene;
    private string pauseScene = "PauseScene";
    private string informationScene = "InformationScene";

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
    private bool operate = false;

    /// <summary>
    /// �V�[���J�ڂ̎��ԊԊu
    /// </summary>
    private float maxTransitionTime = 0.5f;

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
            DontDestroyOnLoad(gameObject);
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
            Cursor.lockState = CursorLockMode.Confined; // ��ʓ��ł̂݃J�[�\�����ړ�������
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// �����ɃC���t�H���[�V�����V�[��, �|�[�Y�V�[���̂ǂ��炩���w�肵�āA���C���V�[������ꎞ�I�ɐ؂�ւ��܂��B
    /// </summary>
    private IEnumerator CallOptionScene(string scene)
    {
        transitionTime = maxTransitionTime;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(transitionTime);
        CursorSetter(); // �V�[�����؂�ւ�钼�O�ɃJ�[�\����\��������
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// �I�v�V�����V�[�����烁�C���V�[���ɖ߂�܂��B
    /// </summary>
    private IEnumerator ReturnMainScene(string scene)
    {
        transitionTime = maxTransitionTime;

        yield return new WaitForSecondsRealtime(transitionTime);
        CursorSetter(); // �V�[�����؂�ւ��������ɃJ�[�\��������
        SceneManager.UnloadSceneAsync(scene);
        Time.timeScale = 1;
    }

    /// <summary>
    /// �����ɐV�������C���V�[�����w�肵�āA���C���V�[�����Đݒ肵�܂��B
    /// </summary>
    public static IEnumerator SetNewMainScene(string scene)
    {
        Time.timeScale = 0;

        // �Ó]�`��
        yield return new WaitForSecondsRealtime(1.0f);
        SceneManager.LoadScene(scene);

        // ���C���V�[���Đݒ��̖��]�`��
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1;
    }
}