using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* �|�[�Y�V�[���ł�邱��
 * 1.�|�[�Y�V�[����Canvas�ɔ������̍��w�i��u���Ă���
 * 2.PAUSE�����̃J���[��ݒ肷��
 * 3.���̃X�N���v�g���A�^�b�`����
 */

class PauseScene : MonoBehaviour
{
    private float maxTime = 1.5f;
    private float time = 0;
    private bool reverse = false;

    [SerializeField]
    private GameObject text;

    private void Awake()
    {
        time = maxTime;
    }

    private void Update()
    {
        if (text != null)
        {
            if (reverse)
            {
                time -= Time.unscaledDeltaTime;
                if (time <= 0)
                {
                    reverse = !reverse;
                    time = 0;
                }
            }
            else
            {
                time += Time.unscaledDeltaTime;
                if (time >= maxTime)
                {
                    reverse = !reverse;
                    time = maxTime;
                }
            }
            // �F��ς��܂��B
        }
    }
}