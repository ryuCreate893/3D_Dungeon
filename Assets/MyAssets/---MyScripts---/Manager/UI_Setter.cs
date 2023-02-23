using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UI_Setter : MonoBehaviour
{
    [SerializeField, Tooltip("UI��\���������L�����o�X��o�^���܂��B")]
    protected Transform canvas;

    /// <summary>
    /// UI�̃Z�b�g���s���܂��B
    /// </summary>
    abstract public void SetWindow();

    /// <summary>
    /// UI�̏������s���܂��B
    /// </summary>
    abstract public void EraseWindow();
}