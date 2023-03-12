using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public static Stair stairInstance;

    // *** �L�����o�X�̐ݒ� ***
    [SerializeField, Tooltip("Stair�ɐG�ꂽ�Ƃ��ɕ\������UI���Z�b�g���܂��B")]
    private GameObject stair_UI;

    private void Awake()
    {
        stairInstance = this; // �ŏ���Stair�̂݃C���X�^���X�ɓo�^(D_Manager��Stair�̗L���𔻒�)
    }

    // �v���C���[��Stair�I�u�W�F�N�g�ɐG�ꂽ�ꍇ�ɃE�B���h�E���Ăяo���܂��B
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UI_Manager.UIInstance.SetExternalUI(stair_UI);
            UI_Manager.UIInstance.ShowCursor();
        }
    }

    // "yes"�������ꂽ�ꍇ�͎��̊K�w�Ɉړ����܂��B
    public void SelectYes()
    {
        D_Manager.gameInstance.Lift();
    }

    // "no"�������ꂽ�ꍇ�͉��������ɃE�B���h�E����܂��B
    public void SelectNo()
    {
        UI_Manager.UIInstance.DestroyExternalUI();
        UI_Manager.UIInstance.HideCursor();
    }
}
