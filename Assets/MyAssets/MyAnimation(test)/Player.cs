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

        // �_�b�V������̃`�F�b�N(���̑�����󂯕t���Ȃ����)
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

            // ***�C���v�b�g���̎擾***
            // �W�����v
            if (Input.GetButtonDown("Jump") && actionCount > 0 && TimeCheck())
            {
                isSlow = false; // ���Ⴊ�ݏ�Ԃ͉��������
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _rigidbody.AddForce(new Vector3(0, _height, 0), ForceMode.Impulse);
                _animator.SetTrigger("jumping");
                actionCount--;
                _actionTime = jump_Freeze;
            }

            // �_�b�V��
            else if (Input.GetButtonDown("Dash") && _moveMagnitude > 0.1f && actionCount > 0 && TimeCheck())
            {
                isSlow = false; // ���Ⴊ�ݏ�Ԃ͉��������
                isDash = true;
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _velocity *= dashSpeed;
                _animator.SetTrigger("dash");
                actionCount--;
                _actionTime = dashTime;
            }

            // �������ړ�
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
    /// �_�b�V�����̏������s���܂��B
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
    /// �_�b�V����̍d���������s���܂��B
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
    /// �v���C���[�̕�����ݒ肵�܂��B
    /// </summary>
    /// <returns></returns>
    private void SetPlayerAngle()
    {
        // �v���C���[�̓��͕������擾
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        // ���݂̃v���C���[�̌������擾
        Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // �v���C���[�̈ړ������Ƒ傫�����v�Z
        _velocity = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;
        _moveMagnitude = _velocity.magnitude;

        // �ړ�������ꍇ�Ay���𒆐S�Ƃ����v���C���[�̉�]���v�Z
        if (_moveMagnitude > 0.5f)
        {
            _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
        }

        // ���݂̌�������ړ���̌����܂ŉ�]������
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

    // *** ���� ***
    [Tooltip("�n�ʂɗ����Ă��邩�ǂ����𔻒肵�܂��B")]
    [SerializeField]
    private bool isGround = false;

    [Tooltip("������蓮���Ă��邩�ǂ����𔻒肵�܂��B")]
    [SerializeField]
    private bool isSlow = false;

    [Tooltip("�_�b�V����Ԃ𔻒肵�܂��B")]
    [SerializeField]
    private bool isDash = false;

    [Tooltip("�_�b�V���̍d����Ԃ𔻒肵�܂��B")]
    [SerializeField]
    private bool isDashed = false;


    // *** �\�� ***
    [Tooltip("�v���C���[�̃W�����v�͂ł��B")]
    [SerializeField]
    private float _height = 5.0f;

    [Tooltip("�v���C���[�̃_�b�V�����̉����{���ł��B")]
    [SerializeField]
    private float dashSpeed = 1.4f;

    [Tooltip("�v���C���[��1��̃_�b�V���Ɋ|���鎞�Ԃł��B")]
    [SerializeField]
    private float dashTime = 0.4f;

    [Tooltip("�v���C���[�̍��v�W�����v�񐔂ł��B")]
    [SerializeField]
    private int maxActionCount = 1;

    [Tooltip("�v���C���[�̎c��W�����v�\�񐔂ł��B")]
    [SerializeField]
    private int actionCount;


    // *** �d������ ***
    [Tooltip("�A�����ăW�����v�ł���Ԋu�ł��B")]
    [SerializeField]
    private float jump_Freeze = 0.2f;

    [Tooltip("�_�b�V����̍d�����Ԃł��B")]
    [SerializeField]
    private float dash_Freeze = 0.2f;
}