using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    public static Player playerInstance;

    [Header("�X�L�����")]
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���̈ꗗ�Ɣ���ۗL")]
    private List<PlayerActiveSkill> active_skill;
    [SerializeField, Tooltip("���[�u�X�L���̈ꗗ�Ɣ���ۗL")]
    private List<PlayerActiveSkill> move_skill;
    [SerializeField, Tooltip("�p�b�V�u�X�L���̈ꗗ")]
    private List<PassiveSkill> passive_skill;

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
    private int max_action_count = 1;
    /// <summary>
    /// �c��ړ��A�N�V������
    /// </summary>
    private int action_count = 1;

    // *** �o���l��� ***
    public int Exp { get; set; } = 0; // ���݂̌o���l(�K�v�Ȍo���l = "status.Exp")
    private float next_max_exp = 1.1f; // ���x���A�b�v���A���ɕK�v�Ȍo���l���v�Z����

    // *** �R���C�_�[��� ***
    /// <summary>
    /// ���𔭂���͈͂�ݒ肵�܂��B
    /// </summary>
    public float Sound_radius { get; set; } = 3.0f;
    /// <summary>
    /// ���𔭂��Ă����Ԃ����m���Atrue�̏ꍇ��Sound_radius�̔��a�܂ŃR���C�_�[���g�傳���Afalse�̏ꍇ��0�܂ŃR���C�_�[���k�������܂��B
    /// </summary>
    public bool Sound_produce { get; set; } = false;
    private float run_sound_radius = 3.0f;
    private float walk_sound_radius = 0.5f;

    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
            turn_speed *= 3;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �����ʒu�����܂��Ă���ꍇ�͐ݒ肷��(�`���[�g���A���p�}�b�v�Ȃ�)
            Transform start_point = playerInstance.GetComponent<Transform>();
            start_point.position = transform.position;
            start_point.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        ResetPlayerStatusUI();
    }

    private void Update()
    {
        // �A�N�V�������Ԃ̊Ǘ�
        if (Action_time > 0)
        {
            Action_time -= Time.deltaTime;
            if (isGetAxis) SetVelocity();
        }
        else
        {
            isGetAxis = true;

            if (isCharge)
            {
                Action?.Invoke(); // �`���[�W�X�L���̔���
            }
            else if (Input.GetButtonDown("SlowMove") && isGround)
            {
                isSlow = !isSlow; // �ړ����@�̐؂�ւ�
            }
            else if (action_count > 0)
            {
                InputCheck(move_skill); // �Ή������{�^���̃X�L�����g�p
                if (Action_time <= 0) InputCheck(active_skill);
            }
        }

        // �i�s�����̌���
        if (isGetAxis)
        {
            SetVelocity();

            // y�������Ƃ����J�����̉�]���擾(Player��p)
            Quaternion horizontal_rot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            // �J�����̕������l�������L�����N�^�[�̕����x�N�g�����쐬
            Velocity = horizontal_rot * Velocity;

            // ��]���N����ꍇ�̓`���[�W���̃X�L�������Ey�������Ƃ����L�����N�^�[�̉�]���擾
            if (Velocity.magnitude > 0.1f)
            {
                Action_cancel?.Invoke();
                Character_rot = Quaternion.LookRotation(Velocity, Vector3.up);
            }

            if (!isSlow)
            {
                Velocity *= status.Speed;
            }
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Character_rot, turn_speed * Time.deltaTime);
    }

    private void SetVelocity()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Velocity = new Vector3(horizontal, 0, vertical).normalized;
        // �X�L�����g�p���� �n�ʂɂ���
        if (Action_time <= 0 && isGround)
        {
            // �ړ����̏ꍇ
            if (Velocity.magnitude > 0.1f)
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
    private void InputCheck(List<PlayerActiveSkill> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (Input.GetButtonDown(list[i].Key))
            {
                if (list[i].Skill.TrySkill())
                {
                    action_count--;
                    isSlow = false;
                    isGetAxis = list[i].IsGetAxis;
                    list[i].Skill.SkillContent();
                    break;
                }
                else if (isCharge)
                {
                    break;
                }
            }
        }
    }

    public override void GetNewSkill(GameObject skill) { }

    protected override void SetActiveSkill()
    {
        for (int i = 0; i < active_skill.Count; i++)
        {
            active_skill[i].Skill.SetSkill(gameObject);
        }

        for (int i = 0; i < move_skill.Count; i++)
        {
            move_skill[i].Skill.SetSkill(gameObject);
        }
    }

    protected override void SetPassiveSkill()
    {
        for (int i = 0; i < passive_skill.Count; i++)
        {
            passive_skill[i].SetSkill(gameObject);
            passive_skill[i].SkillContent();
        }
    }


    public override void FoundEnemy(GameObject target) { }
    protected override void LoseSightEnemy() { }

    public override void Beat(int get_exp)
    {
        int exp_value = Exp; // UI�Z�b�g�p
        Exp += get_exp;
        while (status.Exp <= Exp)
        {
            Exp -= status.Exp;
            status.FloatExp *= next_max_exp;
            status.Exp = (int)status.FloatExp;

            LevelUp(1);
            SetPlayerStatusUI_Max();
            UI_Manager.UIInstance.Ps.Hp.Cure(status.MaxHp);
            UI_Manager.UIInstance.Ps.Sp.Cure(status.MaxSp);
        }

        if (Exp > exp_value)
        {
            UI_Manager.UIInstance.Ps.Exp.Cure(Exp);
        }
        else if (Exp < exp_value)
        {
            UI_Manager.UIInstance.Ps.Exp.Burn(Exp);
        }
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
            action_count = max_action_count;
            _animator.SetBool("isground", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            action_count--;
            _animator.SetBool("isground", false);
        }
    }

    /// <summary>
    /// UI�̍Đݒ���s���܂�(D_Manager.Lift����Ăяo��)
    /// </summary>
    public void ResetPlayerStatusUI()
    {
        SetPlayerStatusUI_Max();
        SetPlayerStatusUI_Value();
    }

    /// <summary>
    /// ���݂̍ő�X�e�[�^�X��UI�ɔ��f���܂��B�V�[���؂�ւ�����A���x���A�b�v���ȂǂɌĂяo���܂��B
    /// </summary>
    private void SetPlayerStatusUI_Max()
    {
        UI_Manager.UIInstance.Ps.Hp.SetSlider_MaxValue(status.MaxHp);
        UI_Manager.UIInstance.Ps.Sp.SetSlider_MaxValue(status.MaxSp);
        UI_Manager.UIInstance.Ps.Exp.SetSlider_MaxValue(status.Exp);
    }

    /// <summary>
    /// ���݂̃X�e�[�^�X��UI�ɔ��f���܂��B������0�ɂ���̂ŁA�V�[���؂�ւ����̂݌Ăяo���܂��B
    /// </summary>
    private void SetPlayerStatusUI_Value()
    {
        UI_Manager.UIInstance.Ps.Hp.SetSlider_Value(status.Hp);
        UI_Manager.UIInstance.Ps.Sp.SetSlider_Value(status.Sp);
        UI_Manager.UIInstance.Ps.Exp.SetSlider_Value(Exp);
    }
}