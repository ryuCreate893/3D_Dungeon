using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    /// <summary>
    /// �J�������g�̈ʒu���
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// �v���C���[�̈ʒu���
    /// </summary>
    private Transform _playerTransform;
    /// <summary>
    /// �J�����ƃv���C���[�̑��ΓI�ȋ���
    /// </summary>
    private Vector3 offset_v = new Vector3(0, 1.5f, 2.5f);
    /// <summary>
    /// �J�����̉�]
    /// </summary>
    private Quaternion offset_q;
    /// <summary>
    /// �J�������v���C���[�̕����x�N�g��
    /// </summary>
    private Vector3 v3look;

    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float minRotate_X = -60;
    /// <summary>
    /// �J�����̍�������(��)
    /// </summary>
    private const float maxRotate_X = 60;
    /// <summary>
    /// �J�����̈ړ����x(��)
    /// </summary>
    private const float rotate_X = 0.5f;
    /// <summary>
    /// �J�����̈ړ����x(�c)
    /// </summary>
    private const float rotate_Y = 0.8f;


    private void Start()
    {
        _transform = GetComponent<Transform>();
        _playerTransform = Player.playerInstance.GetComponent<Transform>();
        //_transform.Rotate(10, 0, 0);
        //_transform.eulerAngles = new Vector3(_transform.eulerAngles.x + 10, _transform.eulerAngles.y, _transform.eulerAngles.z);
    }

    private void Update()
    {
        //_transform.position = _playerTransform.position + offset_v;
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            float x = Input.GetAxis("Mouse X") * rotate_X;
            float y = Input.GetAxis("Mouse Y") * rotate_Y;

            // �㉺�ړ�
            _transform.RotateAround(_playerTransform.position, _playerTransform.right, y);
            //_transform.rotation = Mathf.Min(_transform.rotation.y, maxRotate_X);
            //_transform.rotation.y = Mathf.Max(_transform.rotation.y, minRotate_X);

            // ���ړ�
            _transform.RotateAround(_playerTransform.position, _playerTransform.up, x);
        }

        v3look = VectorSet();
        //_transform.LookAt(_playerTransform);
        
        _transform.rotation = Quaternion.LookRotation(v3look, _playerTransform.up);
    }

    /// <summary>
    /// �c��]��}�����������x�N�g����Ԃ��܂��B
    /// </summary>
    private Vector3 VectorSet()
    {
        Vector3 v3 = _playerTransform.position - _transform.position;
        v3.y *= 0.5f;
        return v3;
    }
}
