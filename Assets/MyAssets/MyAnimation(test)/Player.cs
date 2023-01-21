using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    private float _horizontal;
    private float _vertical;
    private float _moveMagnitude;

    private void Update()
    {
        if (!TimeCheck()) _actionTime -= Time.deltaTime;

        // ダッシュ判定のチェック(他の操作を受け付けない状態)
        if (isDash)
        {
            Dash();
        }
        else if (isDashed)
        {
            Dash_Freeze();
        }
        else
        {
            SetPlayerAngle();

            // ***インプット情報の取得***
            // ジャンプ
            if (Input.GetButtonDown("Jump") && actionCount > 0 && TimeCheck())
            {
                isSlow = false; // しゃがみ状態は解除される
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _rigidbody.AddForce(new Vector3(0, _height, 0), ForceMode.Impulse);
                _animator.SetTrigger("jumping");
                actionCount--;
                _actionTime = jump_Freeze;
            }

            // ダッシュ
            else if (Input.GetButtonDown("Dash") && _moveMagnitude > 0.1f && actionCount > 0 && TimeCheck())
            {
                isSlow = false; // しゃがみ状態は解除される
                isDash = true;
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _velocity *= dashSpeed;
                _animator.SetTrigger("dash");
                actionCount--;
                _actionTime = dashTime;
            }

            // ゆっくり移動
            else if (Input.GetButtonDown("SlowMove") && isGround)
            {
                isSlow = !isSlow;
            }

            if (!isSlow)
            {
                _velocity *= _current.Speed;
            }

            _animator.SetBool("slowmove", isSlow);
            _animator.SetBool("running", !isSlow);
            if (_moveMagnitude < 0.1f)
            {
                _animator.SetBool("idle", true);
            }
            else
            {
                _animator.SetBool("idle", false);
            }
        }
    }

    private void FixedUpdate()
    {
        _velocity += new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.velocity = _velocity;
    }

    /// <summary>
    /// ダッシュ中の処理を行います。
    /// </summary>
    /// <returns></returns>
    private void Dash()
    {
        _rigidbody.velocity = _velocity;
        if (TimeCheck())
        {
            isDash = false;
            _actionTime = dash_Freeze;
            if (isGround)
            {
                isDashed = true;
            }
        }
    }

    /// <summary>
    /// ダッシュ後の硬直処理を行います。
    /// </summary>
    /// <returns></returns>
    private void Dash_Freeze()
    {
        _velocity = Vector3.zero;
        _rigidbody.velocity = _velocity;
        if (TimeCheck())
        {
            isDashed = false;
        }
    }

    /// <summary>
    /// プレイヤーの方向を設定します。
    /// </summary>
    /// <returns></returns>
    private void SetPlayerAngle()
    {
        // プレイヤーの入力方向を取得
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        // 現在のプレイヤーの向きを取得
        Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // プレイヤーの移動方向と大きさを計算
        _velocity = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;
        _moveMagnitude = _velocity.magnitude;

        // 移動させる場合、y軸を中心としたプレイヤーの回転を計算
        if (_moveMagnitude > 0.5f)
        {
            _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
        }

        // 現在の向きから移動後の向きまで回転させる
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, 600 * Time.deltaTime);
    }

    public override void FoundEnemy() { }
    public override void LoseSightEnemy() { }
    public override void Beat() { }
    protected override void DeathCharacter() { }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            actionCount = maxActionCount;
            _animator.SetBool("isground", isGround);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            _animator.SetBool("isground", isGround);
        }
    }

    // *** 判定 ***
    [Tooltip("地面に立っているかどうかを判定します。")]
    [SerializeField]
    private bool isGround = false;

    [Tooltip("ゆっくり動いているかどうかを判定します。")]
    [SerializeField]
    private bool isSlow = false;

    [Tooltip("ダッシュ状態を判定します。")]
    [SerializeField]
    private bool isDash = false;

    [Tooltip("ダッシュの硬直状態を判定します。")]
    [SerializeField]
    private bool isDashed = false;


    // *** 能力 ***
    [Tooltip("プレイヤーのジャンプ力です。")]
    [SerializeField]
    private float _height = 5.0f;

    [Tooltip("プレイヤーのダッシュ時の加速倍率です。")]
    [SerializeField]
    private float dashSpeed = 1.4f;

    [Tooltip("プレイヤーが1回のダッシュに掛ける時間です。")]
    [SerializeField]
    private float dashTime = 0.4f;

    [Tooltip("プレイヤーの合計ジャンプ回数です。")]
    [SerializeField]
    private int maxActionCount = 1;

    [Tooltip("プレイヤーの残りジャンプ可能回数です。")]
    [SerializeField]
    private int actionCount;


    // *** 硬直時間 ***
    [Tooltip("連続してジャンプできる間隔です。")]
    [SerializeField]
    private float jump_Freeze = 0.2f;

    [Tooltip("ダッシュ後の硬直時間です。")]
    [SerializeField]
    private float dash_Freeze = 0.2f;
}