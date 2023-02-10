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

    // *** �ړ��������͗p�ϐ� ***
    /// <summary>
    /// ���ړ�
    /// </summary>
    private float _horizontal;
    /// <summary>
    /// ���s���ړ�
    /// </summary>
    private float _vertical;

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
        if (isGetAxis) // isGetAxis = true�̂Ƃ��A�������͂��󂯕t����
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _velocity = new Vector3(_horizontal, 0, _vertical).normalized;
        }

        if (_actionTime > 0) // ���̃X�L���𔭓��ł��Ȃ����
        {
            _actionTime -= Time.deltaTime;
        }
        else
        {
            isGetAxis = true; // ����̃X�L���g�p���ȊO�̓L�[�ړ�������

            if (_chargeSkill != -1) // �`���[�W���Ă���X�L���̔���
            {
                activeSkill[_chargeSkill].Skill.TrySkill();
            }
            else if (Input.GetButtonDown("SlowMove") && isGround) // �������ړ��̐؂�ւ�
            {
                isSlow = !isSlow;
            }
            else if (actionCount > 0) // �X�L���ɑΉ�����{�^���������Ă����ꍇ�̏���
            {
                UseSkill(moveSkill);
                if (_actionTime <= 0) UseSkill(activeSkill);
                if (_actionTime > 0)
                {
                    isSlow = false;
                }
            }
        }

        if (isGetAxis) // �i�s�����̌���(isGetAxis��false�̏ꍇ�͕������Œ�)
        {
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
        else
        {
            _characterRotation = _transform.rotation;
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, 720 * Time.deltaTime);
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
                skillList[i].Skill.TrySkill();
                if (_actionTime > 0)
                {
                    actionCount--;
                    Debug.Log(actionCount);
                    isGetAxis = skillList[i].IsGetAxis;
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
            actionCount--;
            _animator.SetBool("isground", isGround);
        }
    }
}