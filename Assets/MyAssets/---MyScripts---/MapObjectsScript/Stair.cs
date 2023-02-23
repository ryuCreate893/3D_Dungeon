using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 文字枠である"window"を用意し、さらにその子オブジェクトとして
 * ぶつかったのが"階段"であることを説明する"information_window"と
 * "進む" "進まない"の選択肢を表示する"select_window"を作成する
 * "select_window"オブジェクトにはさらにyes, noのオブジェクトを
 * 子オブジェクトとしてセットし、その子オブジェクトから"Stair"の
 * SelectYes, SelectNoのメソッドを呼び出す
 */
public class Stair : UI_Setter
{
    [SerializeField]
    private GameObject select_window;
    private GameObject select_window_obj;
    [SerializeField]
    private GameObject information_window;
    private GameObject information_window_obj;

    public override void SetWindow()
    {
        Time.timeScale = 0;
        select_window_obj = Instantiate(select_window, canvas);
        information_window_obj = Instantiate(information_window, canvas);
    }

    public override void EraseWindow()
    {
        Destroy(select_window_obj);
        Destroy(information_window_obj);
        Time.timeScale = 1;
    }

    public void SelectYes()
    {
        EraseWindow();
        Debug.Log("a");
        D_Manager.gameInstance.Lift();
    }

    public void SelectNo()
    {
        EraseWindow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetWindow();
        }
    }
}
