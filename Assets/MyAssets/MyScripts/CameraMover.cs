using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // *** 位置情報の取得 ***
    /// <summary>
    /// カメラ自身の位置情報
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// カメラの回転情報
    /// </summary>
    private Quaternion _cameraRotation;
    /// <summary>
    /// プレイヤーの位置情報
    /// </summary>
    private Transform _playerTransform;
    /// <summary>
    /// プレイヤーの位置情報更新用
    /// </summary>
    private Vector3 _playerPosition;
    /// <summary>
    /// 初期のカメラ・プレイヤー間の距離
    /// </summary>
    private float setDistance = 3.0f;
    /// <summary>
    /// 初期のカメラ・プレイヤー間の角度
    /// </summary>
    private float angle = 30.0f;

    // *** カメラの回転に関わる変数 ***
    /// <summary>
    /// 基本回転速度
    /// </summary>
    private float rotateSpeed = 45;
    /// <summary>
    /// 回転速度の倍率設定
    /// </summary>
    private float speed = 1;
    /// <summary>
    /// X軸(縦方向)の回転の大きさ(0-1)
    /// </summary>
    private const float rotate_X = 0.7f;
    /// <summary>
    /// Y軸(横方向)の回転の大きさ(0-1)
    /// </summary>
    private const float rotate_Y = 1.0f;
    /// <summary>
    /// カメラの高さ制限(低)
    /// </summary>
    private const float minRotate_X = -60;
    /// <summary>
    /// カメラの高さ制限(高)
    /// </summary>
    private const float maxRotate_X = 60;

    // *** マウスの位置に関わる変数 ***
    /// <summary>
    /// マウス座標
    /// </summary>
    private Vector3 mousePos;
    /// <summary>
    /// スクリーンの横幅
    /// </summary>
    private float width = Screen.width;
    /// <summary>
    /// スクリーンの立幅
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
                // 上下移動の設定
                if (_transform.rotation.x + x <= minRotate_X) // 最低高度より下に回転する場合
                {
                    Debug.Log("最低高度です。");
                    x = minRotate_X - _transform.rotation.x;
                }
                else if (_transform.rotation.x + x >= maxRotate_X) // 最高高度より上に回転する場合
                {
                    Debug.Log("最高高度です。");
                    x = maxRotate_X - _transform.rotation.x;
                }
                Quaternion qxPos = Quaternion.AngleAxis(x, _playerTransform.right);
                Quaternion qxRot = Quaternion.Lerp(_transform.rotation, qxPos, rotate_X);

                // 横移動の設定
                Quaternion qy = Quaternion.AngleAxis(y, _playerTransform.up);

                // カメラの位置設定
                Vector3 _horizontalPosition = (qxPos * qy) * _transform.position;
                _transform.position = Vector3.Slerp(_transform.position, _horizontalPosition, rotateSpeed * speed * Time.deltaTime);

                // カメラの回転設定
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
