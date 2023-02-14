using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // *** 位置情報の取得 ***
    /// <summary>
    /// カメラの位置情報
    /// </summary>
    private Transform _transform;
    /// <summary>
    /// プレイヤーの位置情報
    /// </summary>
    private Transform _playerTransform;
    /// <summary>
    /// プレイヤーの位置情報の更新用変数
    /// </summary>
    private Vector3 _playerPos;
    /// <summary>
    /// カメラ→標的の方向ベクトル(基本はプレイヤー)
    /// </summary>
    private Vector3 _targetPos;
    /// <summary>
    /// カメラ・プレイヤー間の距離
    /// </summary>
    private float distance = 3.0f;
    /// <summary>
    /// カメラ・プレイヤー間の角度
    /// </summary>
    private float angle = 30.0f;


    // *** カメラの回転に関わる変数 ***
    /// <summary>
    /// カメラの水平方向の回転・位置情報
    /// </summary>
    private Quaternion _h;
    /// <summary>
    /// カメラの垂直方向の位置情報
    /// </summary>
    private Quaternion _vPos;
    /// <summary>
    /// カメラの垂直方向の回転情報
    /// </summary>
    private Quaternion _vRot;
    /// <summary>
    /// カメラの全体の回転情報
    /// </summary>
    private Quaternion _cameraRot;

    /// <summary>
    /// カメラの高さ制限(低)
    /// </summary>
    private const float vRot_min = -40;
    /// <summary>
    /// カメラの高さ制限(高)
    /// </summary>
    private const float vRot_max = 60;

    /// <summary>
    /// 基本回転速度
    /// </summary>
    private float rotSpeed = 45;
    /// <summary>
    /// 垂直方向の移動の大きさ(0-1)
    /// </summary>
    private const float vRotSpeed = 0.7f;
    /// <summary>
    /// 回転速度の倍率設定
    /// </summary>
    private float speed = 1;


    // *** マウスの位置に関わる変数 ***
    /// <summary>
    /// マウス座標
    /// </summary>
    private Vector3 _mousePos;
    /// <summary>
    /// スクリーンの横幅
    /// </summary>
    private float width = Screen.width;
    /// <summary>
    /// スクリーンの縦幅
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
        // カメラの追従
        _transform.position += _playerTransform.position - _playerPos;
        _playerPos = _playerTransform.position;

        // マウスカーソルの位置確認と移動方向の取得
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            _mousePos = Input.mousePosition;
            if (_mousePos.x <= 0 || width <= _mousePos.x || _mousePos.y <= 0 || height <= _mousePos.y)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                float x = Input.GetAxis("Mouse X"); // 左に移動すると-, 右に移動すると+
                float y = Input.GetAxis("Mouse Y"); // 下に移動すると-, 上に移動すると+

                float vAngle = _cameraRot.eulerAngles.x + (-y * vRotSpeed);
                Debug.Log(_cameraRot.eulerAngles.x);
                if (vAngle <= vRot_min) // 見上げアングルの最高角度(rotationの下限値)
                {
                    Debug.Log("最低高度です。");
                    vAngle = (vRot_min - _cameraRot.eulerAngles.x) / (vAngle - _cameraRot.eulerAngles.x);
                    y = vAngle / vRotSpeed;
                }
                else if(vRot_max <= vAngle)
                {
                    Debug.Log("最高高度です。");
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

                // カメラの位置設定
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
