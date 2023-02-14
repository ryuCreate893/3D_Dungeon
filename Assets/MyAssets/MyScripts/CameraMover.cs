using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // *** �ʒu���̎擾 ***
    /// <summary>
    /// �J�����̈ʒu���
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// �v���C���[�̈ʒu���
    /// </summary>
    private Transform _playerTransform;
    /// <summary>
    /// �v���C���[�̈ʒu���̍X�V�p�ϐ�
    /// </summary>
    private Vector3 _playerPos;
    /// <summary>
    /// �J�������W�I�̕����x�N�g��(��{�̓v���C���[)
    /// </summary>
    private Vector3 _targetPos;
    /// <summary>
    /// �J�����E�v���C���[�Ԃ̋���
    /// </summary>
    private float distance = 3.0f;
    /// <summary>
    /// �J�����E�v���C���[�Ԃ̊p�x
    /// </summary>
    private float angle = 30.0f;


    // *** �J�����̉�]�Ɋւ��ϐ� ***
    /// <summary>
    /// �J�����̐��������̉�]�E�ʒu���
    /// </summary>
    private Quaternion _h;
    /// <summary>
    /// �J�����̐��������̈ʒu���
    /// </summary>
    private Quaternion _vPos;
    /// <summary>
    /// �J�����̐��������̉�]���
    /// </summary>
    private Quaternion _vRot;
    /// <summary>
    /// �J�����̑S�̂̉�]���
    /// </summary>
    private Quaternion _cameraRot;

    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float vRot_min = -40;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float vRot_max = 60;

    /// <summary>
    /// ��{��]���x
    /// </summary>
    private float rotSpeed = 45;
    /// <summary>
    /// ���������̈ړ��̑傫��(0-1)
    /// </summary>
    private const float vRotSpeed = 0.7f;
    /// <summary>
    /// ��]���x�̔{���ݒ�
    /// </summary>
    private float speed = 1;


    // *** �}�E�X�̈ʒu�Ɋւ��ϐ� ***
    /// <summary>
    /// �}�E�X���W
    /// </summary>
    private Vector3 _mousePos;
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
        _playerPos = _playerTransform.position;
        _targetPos = _playerPos;

        _transform.position = _playerTransform.forward * -distance;
        _transform.LookAt(_playerPos);
        _vPos = Quaternion.AngleAxis(angle, _transform.right);
        _vRot = Quaternion.AngleAxis(angle * vRotSpeed, _transform.right);
        _transform.position = _vPos * _transform.position;
        _transform.rotation = _vRot * _transform.rotation;
        _cameraRot = _transform.rotation;
    }

    private void Update()
    {
        // �J�����̒Ǐ]
        _transform.position += _playerTransform.position - _playerPos;
        _playerPos = _playerTransform.position;

        // �}�E�X�J�[�\���̈ʒu�m�F�ƈړ������̎擾
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            _mousePos = Input.mousePosition;
            if (_mousePos.x <= 0 || width <= _mousePos.x || _mousePos.y <= 0 || height <= _mousePos.y)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                float x = Input.GetAxis("Mouse X"); // ���Ɉړ������-, �E�Ɉړ������+
                float y = Input.GetAxis("Mouse Y"); // ���Ɉړ������-, ��Ɉړ������+

                float vAngle = _cameraRot.eulerAngles.x + (-y * vRotSpeed);
                Debug.Log(_cameraRot.eulerAngles.x);
                if (vAngle <= vRot_min) // ���グ�A���O���̍ō��p�x(rotation�̉����l)
                {
                    Debug.Log("�Œፂ�x�ł��B");
                    vAngle = (vRot_min - _cameraRot.eulerAngles.x) / (vAngle - _cameraRot.eulerAngles.x);
                    y = vAngle / vRotSpeed;
                }
                else if(vRot_max <= vAngle)
                {
                    Debug.Log("�ō����x�ł��B");
                    Debug.Log(vAngle);
                    vAngle = (vRot_max - _cameraRot.eulerAngles.x) / (vAngle - _cameraRot.eulerAngles.x);
                    Debug.Log(vAngle);
                    y = vAngle / vRotSpeed;
                }
                else
                {
                    vAngle = -y * vRotSpeed;
                }

                _cameraRot = _cameraRot * Quaternion.Euler(vAngle, x, 0);
                Quaternion xPos = Quaternion.AngleAxis(x, _playerTransform.up);
                Quaternion yPos = Quaternion.AngleAxis(y, _transform.right);

                // �J�����̈ʒu�ݒ�
                _transform.position = Vector3.Slerp(_transform.position, xPos * yPos * _transform.position, rotSpeed * speed * Time.deltaTime);
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _cameraRot, rotSpeed * speed * Time.deltaTime);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void SetTarget(Vector3 v3)
    {

    }
}
