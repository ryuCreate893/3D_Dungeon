using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    public static Player playerInstance;

    [Header("�X�L�����")]
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���̈ꗗ�Ɣ���ۗL")]
    private List<PlayerActiveSkill> activeSkill;
    [SerializeField, Tooltip("���[�u�X�L���̈ꗗ�Ɣ���ۗL")]
    private List<PlayerActiveSkill> moveSkill;
    [SerializeField, Tooltip("�p�b�V�u�X�L���̈ꗗ")]
    private List<PassiveSkill> passiveSkill;

    // *** ���� ***
    /// <summary>
    /// �n�ʂɗ����Ă��邩
    /// </summary>
    private bool isGround = true;
    /// <summary>
    /// �����Ĉړ�(false) / �������ړ�(true)�̐؂�ւ�
    /// </summary>
    private bool isSlow = false;
    /// <summary>
    /// �L�[���͂ɂ��ړ����󂯕t�����Ԃ̊Ǘ�(false = �󂯕t���Ȃ�)
    /// </summary>
    public bool isGetAxis { private get; set; } = true;

    // *** �A�N�V�����\�� ***
    /// <summary>
    /// ���v�ړ��A�N�V������(�n�� + �󒆂œ������)
    /// </summary>
    private int maxActionCount = 1;
    /// <summary>
    /// �c��ړ��A�N�V������
    /// </summary>
    private int actionCount = 1;

    // *** �o���l��� ***
    private int maxExp = 50;
    private float nextExp = 1.1f;

    protected override void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // �A�N�V�������Ԃ��c���Ă���
        if (_actionTime > 0)
        {
            _actionTime -= Time.deltaTime;
            if (isGetAxis) SetVelocity();
        }

        // �A�N�V�������Ԃ��o�߂���
        else
        {
            isGetAxis = true;

            // �X�L�����`���[�W���Ă���
            if (_chargeSkill != -1)
            {
                if (activeSkill[_chargeSkill].Skill.TrySkill())
                {
                    activeSkill[_chargeSkill].Skill.SkillContent();
                    _chargeSkill = -1;
                }
            }

            // �������ړ��̐؂�ւ�
            else if (Input.GetButtonDown("SlowMove") && isGround)
            {
                isSlow = !isSlow;
            }

            // �A�N�V�������g�p�\�ŁA�X�L���ɑΉ�����{�^���������Ă���
            else if (actionCount > 0)
            {
                UseSkill(moveSkill);
                if (_actionTime <= 0) UseSkill(activeSkill);
            }
        }

        // �i�s�����̌���
        if (isGetAxis)
        {
            SetVelocity();

            // y�������Ƃ����J�����̉�]���擾(Player��p)
            Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            // �J�����̕������l�������L�����N�^�[�̕����x�N�g�����쐬
            _velocity = _horizontalRotation * _velocity;

            // ��]���N����ꍇ��y�������Ƃ����L�����N�^�[�̉�]���擾
            if (_velocity.magnitude > 0.1f)
            {
                _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
            }

            // �X�L���̃`���[�W���Ɉړ������ꍇ�̓X�L���̃`���[�W���L�����Z�����܂��B
            if (_chargeSkill != -1 && (isSlow || _velocity.magnitude > 0.1f))
            {
                _chargeSkill = -1;
                _actionTime = 0;
            }

            if (!isSlow)
            {
                _velocity *= _status.Speed;
            }
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, _turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �ړ�����"_velocity"��ݒ�
    /// </summary>
    private void SetVelocity()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        _velocity = new Vector3(_horizontal, 0, _vertical).normalized;
        // �X�L�����g�p���� �n�ʂɂ���
        if (_actionTime <= 0 && isGround)
        {
            // �ړ����̏ꍇ
            if (_velocity.magnitude > 0.1f)
            {
                _animator.SetBool("idle", false);
                _animator.SetBool("slowmove", isSlow);
                _animator.SetBool("running", !isSlow);
            }
            // ��~���̏ꍇ
            else
            {
                _animator.SetBool("idle", true);
            }
        }
    }


    // *** �X�L���̔�������Ɋւ�郁�\�b�h ***
    /// <summary>
    /// �������{�^������g�p����X�L����I�т܂��B
    /// </summary>
    /// <param name="skillList"></param>
    private void UseSkill(List<PlayerActiveSkill> skillList)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (Input.GetButtonDown(skillList[i].Key))
            {
                if (skillList[i].Skill.TrySkill())
                {
                    skillList[i].Skill.SkillContent();
                    actionCount--;
                    isGetAxis = skillList[i].IsGetAxis;
                    isSlow = false;
                    break;
                }
            }
        }
    }

    protected override void DamagedCancel(int n)
    {
        activeSkill[n].Skill.DamagedCancel();
    }

    public override void GetNewSkill(GameObject skill) { }

    protected override void SetActiveSkill()
    {
        for (int i = 0; i < activeSkill.Count; i++)
        {
            activeSkill[i].Skill.SetSkill(gameObject);
        }

        for (int i = 0; i < moveSkill.Count; i++)
        {
            moveSkill[i].Skill.SetSkill(gameObject);
        }
    }

    protected override void SetPassiveSkill()
    {
        for (int i = 0; i < passiveSkill.Count; i++)
        {
            passiveSkill[i].SetSkill(gameObject);
            passiveSkill[i].SkillContent();
        }
    }


    public override void FoundEnemy(GameObject target) { }
    protected override void LoseSightEnemy() { }

    public override void Beat(GameObject target)
    {
        int count = 0;
        _Status.Exp += target.GetComponent<Character>()._Status.Exp;
        while (_Status.Exp >= maxExp)
        {
            _Status.Exp -= maxExp;
            maxExp = (int)(maxExp * nextExp);
            count++;
        }
        if (count > 0) _Status.LevelUp(count);
    }

    protected override void DeathCharacter()
    {
        Time.timeScale = 0;
        S_Manager.sceneInstance.Operate = true;

    }

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
            actionCount--;
            _animator.SetBool("isground", isGround);
        }
    }
}