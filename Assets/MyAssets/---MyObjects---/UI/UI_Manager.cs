using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI�ƃ}�E�X�J�[�\���̊Ǘ����s���܂��B
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager UIInstance;

    // �ŉ��wUI
    [SerializeField, Tooltip("StatusCanvas �I�u�W�F�N�g")]
    private Transform status_canvas;
    [SerializeField, Tooltip("PlayerStatusUI ���\�b�h")]
    private PlayerStatusUIList ps;
    public PlayerStatusUIList Ps { get { return ps; } }

    // ����UI
    [SerializeField, Tooltip("ScreenCanvas �I�u�W�F�N�g")]
    private Transform screen_canvas;
    [SerializeField, Tooltip("ScreenFaderUI ���\�b�h")]
    private ScreenFaderUI sf;
    public ScreenFaderUI Sf { get { return sf; } }

    // �őO��UI(�O������Ăяo�����󂯂ĕ\������UI)
    [SerializeField, Tooltip("ExternalCanvas �I�u�W�F�N�g")]
    private Transform external_canvas;
    private GameObject external_UI;

    private void Awake()
    {
        if (UIInstance == null)
        {
            UIInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetExternalUI(GameObject UI)
    {
        Time.timeScale = 0;
        S_Manager.sceneInstance.Operate = true;
        external_UI = Instantiate(UI, external_canvas);
    }

    public void DestroyExternalUI()
    {
        Destroy(external_UI);
        S_Manager.sceneInstance.Operate = false;
        Time.timeScale = 1;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
