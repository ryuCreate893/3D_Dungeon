using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Slider slider_clone;
    [SerializeField]
    private TextMeshProUGUI textL;
    [SerializeField]
    private TextMeshProUGUI textR;

    private int delta = 0;

    public void SetSlider_MaxValue(int value)
    {
        slider.maxValue = value;
        slider_clone.maxValue = value;
        textR.text = value.ToString();
    }

    public void SetSlider_Value(int value)
    {
        slider.value = value;
        slider_clone.value = value;
        textL.text = value.ToString();
        delta = 0;
    }

    private void LateUpdate()
    {
        if (delta > 0)
        {
            Increase();
        }
        else if (delta < 0)
        {
            Decrease();
        }
    }

    /// <summary>
    /// �񕜂���u�Ԃ̏���(�����ɍX�V��̒l������"Player"����Ăяo��)
    /// </summary>
    public void Cure(int value)
    {
        delta += value;
        textL.text = value.ToString();
    }

    /// <summary>
    /// �����u�Ԃ̏���(�����ɍX�V��̒l������"Player"����Ăяo��)
    /// </summary>
    public void Burn(int value)
    {
        if (slider.value > value)
        {
            slider.value = value;
        }
        delta -= value;
        textL.text = value.ToString();
    }

    private void Increase()
    {
        slider.value++;
        slider_clone.value++;
        delta--;
    }

    private void Decrease()
    {
        slider_clone.value--;
        delta++;
    }
}