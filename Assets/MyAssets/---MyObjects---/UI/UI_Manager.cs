using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UIとマウスカーソルの管理を行います。
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager UIInstance;

    // 最下層UI
    [SerializeField, Tooltip("StatusCanvas オブジェクト")]
    private Transform status_canvas;
    [SerializeField, Tooltip("PlayerStatusUI メソッド")]
    private PlayerStatusUIList ps;
    public PlayerStatusUIList Ps { get { return ps; } }

    // 中間UI
    [SerializeField, Tooltip("ScreenCanvas オブジェクト")]
    private Transform screen_canvas;
    [SerializeField, Tooltip("ScreenFaderUI メソッド")]
    private ScreenFaderUI sf;
    public ScreenFaderUI Sf { get { return sf; } }

    // 最前面UI(外部から呼び出しを受けて表示するUI)
    [SerializeField, Tooltip("ExternalCanvas オブジェクト")]
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
