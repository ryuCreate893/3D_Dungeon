using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // *** �ʒu���̎擾 ***
    /// <summary>
    /// �J�������g�̈ʒu���
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// �J�����̉�]���
    /// </summary>
    private Quaternion _cameraRotation;
    /// <summary>
    /// �v���C���[�̈ʒu���
    /// </summary>
    private Transform _playerTransform;
    /// <summary>
    /// �v���C���[�̈ʒu���X�V�p
    /// </summary>
    private Vector3 _playerPosition;
    /// <summary>
    /// �����̃J�����E�v���C���[�Ԃ̋���
    /// </summary>
    private float setDistance = 3.0f;
    /// <summary>
    /// �����̃J�����E�v���C���[�Ԃ̊p�x
    /// </summary>
    private float angle = 30.0f;

    // *** �J�����̉�]�Ɋւ��ϐ� ***
    /// <summary>
    /// ��{��]���x
    /// </summary>
    private float rotateSpeed = 45;
    /// <summary>
    /// ��]���x�̔{���ݒ�
    /// </summary>
    private float speed = 1;
    /// <summary>
    /// X��(�c����)�̉�]�̑傫��(0-1)
    /// </summary>
    private const float rotate_X = 0.7f;
    /// <summary>
    /// Y��(������)�̉�]�̑傫��(0-1)
    /// </summary>
    private const float rotate_Y = 1.0f;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float minRotate_X = -60;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float maxRotate_X = 60;

    // *** �}�E�X�̈ʒu�Ɋւ��ϐ� ***
    /// <summary>
    /// �}�E�X���W
    /// </summary>
    private Vector3 mousePos;
    /// <summary>
    /// �X�N���[���̉���
    /// </summary>
    private float width = Screen.width;
    /// <summary>
    /// �X�N���[���̗���
    /// </summary>
    private float height = Screen.height;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _playerTransform = Player.playerInstance.GetComponent<Transform>();
        _playerPosition = _playerTransform.position;

        _transform.position = _playerTransform.forward * -setDistance;
        _transform.LookAt(_playerPosition);
        Quaternion qxPos = Quaternion.AngleAxis(angle, _playerTransform.right);
        Quaternion qxRot = Quaternion.AngleAxis(angle * rotate_X, _playerTransform.right);
        _transform.position = qxPos * _transform.position;
        _transform.rotation = qxRot * _transform.rotation;
        _cameraRotation = _transform.rotation;
    }

    private void Update()
    {
        _transform.position += _playerTransform.position - _playerPosition;
        _playerPosition = _playerTransform.position;
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            mousePos = Input.mousePosition;
            if (mousePos.x <= 0 || width <= mousePos.x || mousePos.y <= 0 || height <= mousePos.y)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                float x = Input.GetAxis("Mouse Y") * rotate_X * speed;
                float y = Input.GetAxis("Mouse X") * rotate_Y * speed;
                //Debug.Log(x + ", " + y);
                // �㉺�ړ��̐ݒ�
                if (_transform.rotation.x + x <= minRotate_X) // �Œፂ�x��艺�ɉ�]����ꍇ
                {
                    Debug.Log("�Œፂ�x�ł��B");
                    x = minRotate_X - _transform.rotation.x;
                }
                else if (_transform.rotation.x + x >= maxRotate_X) // �ō����x����ɉ�]����ꍇ
                {
                    Debug.Log("�ō����x�ł��B");
                    x = maxRotate_X - _transform.rotation.x;
                }
                Quaternion qxPos = Quaternion.AngleAxis(x, _playerTransform.right);
                Quaternion qxRot = Quaternion.Lerp(_transform.rotation, qxPos, rotate_X);

                // ���ړ��̐ݒ�
                Quaternion qy = Quaternion.AngleAxis(y, _playerTransform.up);

                // �J�����̈ʒu�ݒ�
                Vector3 _horizontalPosition = (qxPos * qy) * _transform.position;
                _transform.position = Vector3.Slerp(_transform.position, _horizontalPosition, rotateSpeed * speed * Time.deltaTime);

                // �J�����̉�]�ݒ�
                _cameraRotation = (qxRot * qy) * _cameraRotation;
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _cameraRotation, rotateSpeed * speed * Time.deltaTime);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
