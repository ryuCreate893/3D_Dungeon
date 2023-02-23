using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Transform _transform;

    // *** カメラ座標設定用のオブジェクト ***
    private GameObject posAssist;
    private Transform _pos;

    // *** カメラ照準設定用のオブジェクト ***
    private GameObject rotAssist;
    private Transform _rot;

    // *** プレイヤーのコンポーネントと変数 ***
    private Transform _playerTransform;
    private Vector3 pPos;

    // *** プレイヤーとカメラの位置関係を決定する変数 ***
    /// <summary>
    /// カメラ・プレイヤー間の距離
    /// </summary>
    private float distance = 3.0f;
    /// <summary>
    /// カメラ・プレイヤー間の角度
    /// </summary>
    private float angle = 20.0f;

    // *** カメラの回転を決定する変数 ***
    /// <summary>
    /// 基本回転速度
    /// </summary>
    private float rotSpeed = 180;
    /// <summary>
    /// 回転速度の倍率設定
    /// </summary>
    private float speed = 1;


    // *** 縦軸の回転制限 ***
    /// <summary>
    /// 垂直方向の回転の大きさ(0-1)
    /// </summary>
    private const float vRotSpeed = 0.5f;
    /// <summary>
    /// カメラの高さ制限(低)
    /// </summary>
    private const float vRot_min = 5;
    /// <summary>
    /// カメラの高さ制限(高)
    /// </summary>
    private const float vRot_max = 20;


    // *** マウスの位置に関わる変数 ***
    /// <summary>
    /// マウス座標
    /// </summary>
    private Vector3 mPos;
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
        // カメラの追従
        Vector3 deltaPos = _playerTransform.position - pPos;
        _pos.position += deltaPos;
        _rot.position += deltaPos;
        pPos = _playerTransform.position;

        // マウスカーソルの位置確認
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            mPos = Input.mousePosition;
            // マウスが画面外に移動した場合は中心に戻す
            if (mPos.x <= 0 || width <= mPos.x || mPos.y <= 0 || height <= mPos.y)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                float x = Input.GetAxis("Mouse X") * speed; // 左移動:'-', 右移動:'+'
                float y = Input.GetAxis("Mouse Y") * speed * vRotSpeed; // 下移動:'-', 上移動:'+'

                // 水平方向の回転
                _pos.RotateAround(pPos, _playerTransform.up, x);
                _rot.RotateAround(pPos, _playerTransform.up, x);

                // 垂直方向の回転
                float vRot = _rot.rotation.eulerAngles.x - y;
                if (vRot < vRot_min) // 回転後のxの角度がvRot_minを下回った場合(y < 0)
                {
                    vRot -= vRot_min; // 超過した角度を計算(vRot < 0)
                    y -= vRot; // 縦回転の修正
                }
                else if (vRot > vRot_max) // 回転後のxの角度がvRot_maxを上回った場合(y > 0)
                {
                    vRot -= vRot_max; // 超過した角度を計算(vRot > 0)
                    y -= vRot; // 縦回転の修正
                }
                _pos.RotateAround(pPos, _pos.right, y / vRotSpeed);
                _rot.RotateAround(pPos, _rot.right, y);

                // カメラの位置設定
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