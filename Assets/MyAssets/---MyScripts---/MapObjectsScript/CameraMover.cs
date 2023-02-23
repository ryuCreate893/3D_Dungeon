using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Transform _transform;

    // *** �J�������W�ݒ�p�̃I�u�W�F�N�g ***
    private GameObject posAssist;
    private Transform _pos;

    // *** �J�����Ə��ݒ�p�̃I�u�W�F�N�g ***
    private GameObject rotAssist;
    private Transform _rot;

    // *** �v���C���[�̃R���|�[�l���g�ƕϐ� ***
    private Transform _playerTransform;
    private Vector3 pPos;

    // *** �v���C���[�ƃJ�����̈ʒu�֌W�����肷��ϐ� ***
    /// <summary>
    /// �J�����E�v���C���[�Ԃ̋���
    /// </summary>
    private float distance = 3.0f;
    /// <summary>
    /// �J�����E�v���C���[�Ԃ̊p�x
    /// </summary>
    private float angle = 20.0f;

    // *** �J�����̉�]�����肷��ϐ� ***
    /// <summary>
    /// ��{��]���x
    /// </summary>
    private float rotSpeed = 180;
    /// <summary>
    /// ��]���x�̔{���ݒ�
    /// </summary>
    private float speed = 1;


    // *** �c���̉�]���� ***
    /// <summary>
    /// ���������̉�]�̑傫��(0-1)
    /// </summary>
    private const float vRotSpeed = 0.5f;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float vRot_min = 5;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float vRot_max = 20;


    // *** �}�E�X�̈ʒu�Ɋւ��ϐ� ***
    /// <summary>
    /// �}�E�X���W
    /// </summary>
    private Vector3 mPos;
    /// <summary>
    /// �X�N���[���̉���
    /// </summary>
    private float width = Screen.width;
    /// <summary>
    /// �X�N���[���̏c��
    /// </summary>
    private float height = Screen.height;


    private void Start()
    {
        _transform = GetComponent<Transform>();
        _playerTransform = Player.playerInstance.GetComponent<Transform>();
        pPos = _playerTransform.position;

        _transform.position = pPos + _playerTransform.forward * -distance;
        _transform.LookAt(pPos);

        posAssist = new GameObject("PositionAssist");
        _pos = posAssist.GetComponent<Transform>();
        _pos.position = _transform.position;
        _pos.rotation = _transform.rotation;
        _pos.RotateAround(pPos, _pos.right, angle);

        rotAssist = new GameObject("RotationAssist");
        _rot = rotAssist.GetComponent<Transform>();
        _rot.position = _transform.position;
        _rot.rotation = _transform.rotation;
        _rot.RotateAround(pPos, _rot.right, angle * vRotSpeed);

        _transform.position = _pos.position;
        _transform.rotation = _rot.rotation;
    }

    private void Update()
    {
        // �J�����̒Ǐ]
        Vector3 deltaPos = _playerTransform.position - pPos;
        _pos.position += deltaPos;
        _rot.position += deltaPos;
        pPos = _playerTransform.position;

        // �}�E�X�J�[�\���̈ʒu�m�F
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            mPos = Input.mousePosition;
            // �}�E�X����ʊO�Ɉړ������ꍇ�͒��S�ɖ߂�
            if (mPos.x <= 0 || width <= mPos.x || mPos.y <= 0 || height <= mPos.y)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                float x = Input.GetAxis("Mouse X") * speed; // ���ړ�:'-', �E�ړ�:'+'
                float y = Input.GetAxis("Mouse Y") * speed * vRotSpeed; // ���ړ�:'-', ��ړ�:'+'

                // ���������̉�]
                _pos.RotateAround(pPos, _playerTransform.up, x);
                _rot.RotateAround(pPos, _playerTransform.up, x);

                // ���������̉�]
                float vRot = _rot.rotation.eulerAngles.x - y;
                if (vRot < vRot_min) // ��]���x�̊p�x��vRot_min����������ꍇ(y < 0)
                {
                    vRot -= vRot_min; // ���߂����p�x���v�Z(vRot < 0)
                    y -= vRot; // �c��]�̏C��
                }
                else if (vRot > vRot_max) // ��]���x�̊p�x��vRot_max���������ꍇ(y > 0)
                {
                    vRot -= vRot_max; // ���߂����p�x���v�Z(vRot > 0)
                    y -= vRot; // �c��]�̏C��
                }
                _pos.RotateAround(pPos, _pos.right, y / vRotSpeed);
                _rot.RotateAround(pPos, _rot.right, y);

                // �J�����̈ʒu�ݒ�
                _transform.position = _pos.position;
                _transform.rotation = _rot.rotation;
                //_transform.rotation = Quaternion.RotateTowards(_transform.rotation, _rot.rotation, rotSpeed * speed * Time.deltaTime);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}