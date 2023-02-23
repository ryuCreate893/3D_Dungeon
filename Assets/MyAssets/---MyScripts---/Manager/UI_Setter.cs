using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UI_Setter : MonoBehaviour
{
    [SerializeField, Tooltip("UIを表示したいキャンバスを登録します。")]
    protected Transform canvas;

    /// <summary>
    /// UIのセットを行います。
    /// </summary>
    abstract public void SetWindow();

    /// <summary>
    /// UIの消去を行います。
    /// </summary>
    abstract public void EraseWindow();
}