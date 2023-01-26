using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    // *** 判定 ***
    [SerializeField, Tooltip("地面に立っているかどうかを判定します。")]
    private bool isGround = false;

    [SerializeField, Tooltip("走り, 歩き(しゃがみ)の切り替えを判定します。")]
    private bool isSlow = false;

    [SerializeField, Tooltip("ダッシュ中かどうかを判定します。")]
    private bool isDash = false;

    [SerializeField, Tooltip("ダッシュ後の硬直中かどうかを判定します。")]
    private bool isDashed = false;


    // *** 能力 ***
    [SerializeField, Tooltip("プレイヤーのジャンプ力です。")]
    private float _height = 5.0f;

    [SerializeField, Tooltip("プレイヤーのダッシュ時の加速倍率です。")]
    private float dashSpeed = 1.4f;

    [SerializeField, Tooltip("プレイヤーが1回のダッシュに掛ける時間です。")]
    private float dashTime = 0.4f;

    [SerializeField, Tooltip("プレイヤーの合計ジャンプ回数です。")]
    private int maxActionCount = 1;

    [SerializeField, Tooltip("プレイヤーの残りジャンプ可能回数です。")]
    private int actionCount;


    // *** 硬直時間 ***
    [SerializeField, Tooltip("連続してジャンプできる間隔です。")]
    private float jump_Freeze = 0.2f;

    [SerializeField, Tooltip("ダッシュ後の硬直時間です。")]
    private float dash_Freeze = 0.2f;

    private float _horizontal; // 横移動
    private float _vertical; // 奥移動

    private void Update()
    {
        if (!TimeCheck(_actionTime)) _actionTime -= Time.deltaTime;

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
            // プレイヤーの入力方向を取得し、移動方向と回転を計算する
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _velocity = new Vector3(_horizontal, 0, _vertical).normalized;
            float moveMagnitude = _velocity.magnitude;
            SetCharacterAngle(_velocity);

            // ***インプット情報の取得***
            // ジャンプ
            if (Input.GetButtonDown("Jump") && actionCount > 0 && TimeCheck(_actionTime))
            {
                isSlow = false; // しゃがみ状態は解除される
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _rigidbody.AddForce(new Vector3(0, _height, 0), ForceMode.Impulse);
                _animator.SetTrigger("jumping");
                actionCount--;
                _actionTime = jump_Freeze;
            }
            // ダッシュ
            else if (Input.GetButtonDown("Dash") && moveMagnitude > 0.1f && actionCount > 0 && TimeCheck(_actionTime))
            {
                isSlow = false; // しゃがみ状態は解除される
                isDash = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
            if (moveMagnitude < 0.1f)
            {
                _animator.SetBool("idle", true);
            }
            else
            {
                _animator.SetBool("idle", false);
            }
        }
    }

    /// <summary>
    /// ダッシュ中の処理を行います。
    /// </summary>
    /// <returns></returns>
    private void Dash()
    {
        _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
        if (TimeCheck(_actionTime))
        {
            isDash = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        if (TimeCheck(_actionTime))
        {
            isDashed = false;
        }
    }

    protected override void SetCharacterAngle(Vector3 v3look)
    {
        // y軸を軸としたカメラの回転を取得(Player専用)
        Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // カメラの方向を考慮したキャラクターの方向ベクトルを作成
        _velocity = _horizontalRotation * v3look;

        // 回転が起きる場合はy軸を軸としたキャラクターの回転を取得
        if (_velocity.magnitude > 0.1f)
        {
            _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
        }

        // 現在の向きから移動後の向きまで回転させる
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, 1200 * Time.deltaTime);
    }

    public override void FoundEnemy(GameObject target) { }
    protected override void LoseSightEnemy() { }
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
}