using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    public static Player playerInstance;

    [Header("スキル情報")]
    [SerializeField, Tooltip("アクティブスキルの一覧と判定保有")]
    private List<PlayerActiveSkill> activeSkill;
    [SerializeField, Tooltip("ムーブスキルの一覧と判定保有")]
    private List<PlayerActiveSkill> moveSkill;
    [SerializeField, Tooltip("パッシブスキルの一覧")]
    private List<PassiveSkill> passiveSkill;

    // *** 判定 ***
    /// <summary>
    /// 地面に立っているか
    /// </summary>
    private bool isGround = true;
    /// <summary>
    /// 走って移動(false) / ゆっくり移動(true)の切り替え
    /// </summary>
    private bool isSlow = false;
    /// <summary>
    /// キー入力による移動を受け付ける状態の管理(false = 受け付けない)
    /// </summary>
    public bool isGetAxis { private get; set; } = true;

    // *** アクション能力 ***
    /// <summary>
    /// 合計移動アクション回数(地上 + 空中で動ける回数)
    /// </summary>
    private int maxActionCount = 1;
    /// <summary>
    /// 残り移動アクション回数
    /// </summary>
    private int actionCount = 1;

    // *** 経験値情報 ***
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
        // アクション時間が残っている
        if (_actionTime > 0)
        {
            _actionTime -= Time.deltaTime;
            if (isGetAxis) SetVelocity();
        }

        // アクション時間が経過した
        else
        {
            isGetAxis = true;

            // スキルをチャージしている
            if (_chargeSkill != -1)
            {
                if (activeSkill[_chargeSkill].Skill.TrySkill())
                {
                    activeSkill[_chargeSkill].Skill.SkillContent();
                    _chargeSkill = -1;
                }
            }

            // ゆっくり移動の切り替え
            else if (Input.GetButtonDown("SlowMove") && isGround)
            {
                isSlow = !isSlow;
            }

            // アクションが使用可能で、スキルに対応するボタンを押している
            else if (actionCount > 0)
            {
                UseSkill(moveSkill);
                if (_actionTime <= 0) UseSkill(activeSkill);
            }
        }

        // 進行方向の決定
        if (isGetAxis)
        {
            SetVelocity();

            // y軸を軸としたカメラの回転を取得(Player専用)
            Quaternion _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            // カメラの方向を考慮したキャラクターの方向ベクトルを作成
            _velocity = _horizontalRotation * _velocity;

            // 回転が起きる場合はy軸を軸としたキャラクターの回転を取得
            if (_velocity.magnitude > 0.1f)
            {
                _characterRotation = Quaternion.LookRotation(_velocity, Vector3.up);
            }

            // スキルのチャージ中に移動した場合はスキルのチャージをキャンセルします。
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
    /// 移動方向"_velocity"を設定
    /// </summary>
    private void SetVelocity()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        _velocity = new Vector3(_horizontal, 0, _vertical).normalized;
        // スキル未使用かつ 地面にいる
        if (_actionTime <= 0 && isGround)
        {
            // 移動中の場合
            if (_velocity.magnitude > 0.1f)
            {
                _animator.SetBool("idle", false);
                _animator.SetBool("slowmove", isSlow);
                _animator.SetBool("running", !isSlow);
            }
            // 停止中の場合
            else
            {
                _animator.SetBool("idle", true);
            }
        }
    }


    // *** スキルの発動判定に関わるメソッド ***
    /// <summary>
    /// 押したボタンから使用するスキルを選びます。
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