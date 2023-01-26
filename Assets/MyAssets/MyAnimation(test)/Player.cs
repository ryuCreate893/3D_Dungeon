using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    // *** ���� ***
    [SerializeField, Tooltip("�n�ʂɗ����Ă��邩�ǂ����𔻒肵�܂��B")]
    private bool isGround = false;

    [SerializeField, Tooltip("����, ����(���Ⴊ��)�̐؂�ւ��𔻒肵�܂��B")]
    private bool isSlow = false;

    [SerializeField, Tooltip("�_�b�V�������ǂ����𔻒肵�܂��B")]
    private bool isDash = false;

    [SerializeField, Tooltip("�_�b�V����̍d�������ǂ����𔻒肵�܂��B")]
    private bool isDashed = false;


    // *** �\�� ***
    [SerializeField, Tooltip("�v���C���[�̃W�����v�͂ł��B")]
    private float _height = 5.0f;

    [SerializeField, Tooltip("�v���C���[�̃_�b�V�����̉����{���ł��B")]
    private float dashSpeed = 1.4f;

    [SerializeField, Tooltip("�v���C���[��1��̃_�b�V���Ɋ|���鎞�Ԃł��B")]
    private float dashTime = 0.4f;

    [SerializeField, Tooltip("�v���C���[�̍��v�W�����v�񐔂ł��B")]
    private int maxActionCount = 1;

    [SerializeField, Tooltip("�v���C���[�̎c��W�����v�\�񐔂ł��B")]
    private int actionCount;


    // *** �d������ ***
    [SerializeField, Tooltip("�A�����ăW�����v�ł���Ԋu�ł��B")]
    private float jump_Freeze = 0.2f;

    [SerializeField, Tooltip("�_�b�V����̍d�����Ԃł��B")]
    private float dash_Freeze = 0.2f;

    private float _horizontal; // ���ړ�
    private float _vertical; // ���ړ�

    private void Update()
    {
        if (!TimeCheck(_actionTime)) _actionTime -= Time.deltaTime;

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
            // �v���C���[�̓��͕������擾���A�ړ������Ɖ�]���v�Z����
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _velocity = new Vector3(_horizontal, 0, _vertical).normalized;
            float moveMagnitude = _velocity.magnitude;
            SetCharacterAngle(_velocity);

            // ***�C���v�b�g���̎擾***
            // �W�����v
            if (Input.GetButtonDown("Jump") && actionCount > 0 && TimeCheck(_actionTime))
            {
                isSlow = false; // ���Ⴊ�ݏ�Ԃ͉��������
                _rigidbody.velocity -= new Vector3(0, _rigidbody.velocity.y, 0);
                _rigidbody.AddForce(new Vector3(0, _height, 0), ForceMode.Impulse);
                _animator.SetTrigger("jumping");
                actionCount--;
                _actionTime = jump_Freeze;
            }
            // �_�b�V��
            else if (Input.GetButtonDown("Dash") && moveMagnitude > 0.1f && actionCount > 0 && TimeCheck(_actionTime))
            {
                isSlow = false; // ���Ⴊ�ݏ�Ԃ͉��������
                isDash = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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
    /// �_�b�V�����̏������s���܂��B
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
    /// �_�b�V����̍d���������s���܂��B
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
        // y�������Ƃ����J�����̉�]���擾(Player��p)
        Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // �J�����̕������l�������L�����N�^�[�̕����x�N�g�����쐬
        _velocity = _horizontalRotation * v3look;

        // ��]���N����ꍇ��y�������Ƃ����L�����N�^�[�̉�]���擾
        if (_velocity.magnitude > 0.1f)
        {
            _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
        }

        // ���݂̌�������ړ���̌����܂ŉ�]������
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