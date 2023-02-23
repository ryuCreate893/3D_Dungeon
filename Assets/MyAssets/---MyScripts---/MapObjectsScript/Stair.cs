using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* �����g�ł���"window"��p�ӂ��A����ɂ��̎q�I�u�W�F�N�g�Ƃ���
 * �Ԃ������̂�"�K�i"�ł��邱�Ƃ��������"information_window"��
 * "�i��" "�i�܂Ȃ�"�̑I������\������"select_window"���쐬����
 * "select_window"�I�u�W�F�N�g�ɂ͂����yes, no�̃I�u�W�F�N�g��
 * �q�I�u�W�F�N�g�Ƃ��ăZ�b�g���A���̎q�I�u�W�F�N�g����"Stair"��
 * SelectYes, SelectNo�̃��\�b�h���Ăяo��
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
