using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public static Stair stairInstance;

    // *** キャンバスの設定 ***
    [SerializeField, Tooltip("Stairに触れたときに表示するUIをセットします。")]
    private GameObject stair_UI;

    private void Awake()
    {
        stairInstance = this; // 最初のStairのみインスタンスに登録(D_ManagerでStairの有無を判定)
    }

    // プレイヤーがStairオブジェクトに触れた場合にウィンドウを呼び出します。
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UI_Manager.UIInstance.SetExternalUI(stair_UI);
            UI_Manager.UIInstance.ShowCursor();
        }
    }

    // "yes"が押された場合は次の階層に移動します。
    public void SelectYes()
    {
        D_Manager.gameInstance.Lift();
    }

    // "no"が押された場合は何もせずにウィンドウを閉じます。
    public void SelectNo()
    {
        UI_Manager.UIInstance.DestroyExternalUI();
        UI_Manager.UIInstance.HideCursor();
    }
}
