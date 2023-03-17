using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : Character
{
    public static Player playerInstance;

    [Header("スキル情報")]
    [SerializeField, Tooltip("アクティブスキルの一覧と判定保有")]
    private List<PlayerActiveSkill> active_skill;
    [SerializeField, Tooltip("ムーブスキルの一覧と判定保有")]
    private List<PlayerActiveSkill> move_skill;
    [SerializeField, Tooltip("パッシブスキルの一覧")]
    private List<PassiveSkill> passive_skill;

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
    private int max_action_count = 1;
    /// <summary>
    /// 残り移動アクション回数
    /// </summary>
    private int action_count = 1;

    // *** 経験値情報 ***
    public int Exp { get; set; } = 0; // 現在の経験値(必要な経験値 = "status.Exp")
    private float next_max_exp = 1.1f; // レベルアップ時、次に必要な経験値を計算する

    // *** コライダー情報 ***
    /// <summary>
    /// 音を発する範囲を設定します。
    /// </summary>
    public float Sound_radius { get; set; } = 3.0f;
    /// <summary>
    /// 音を発している状態を検知し、trueの場合はSound_radiusの半径までコライダーを拡大させ、falseの場合は0までコライダーを縮小させます。
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
            // 初期位置が決まっている場合は設定する(チュートリアル用マップなど)
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
        // アクション時間の管理
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
                Action?.Invoke(); // チャージスキルの発動
            }
            else if (Input.GetButtonDown("SlowMove") && isGround)
            {
                isSlow = !isSlow; // 移動方法の切り替え
            }
            else if (action_count > 0)
            {
                InputCheck(move_skill); // 対応したボタンのスキルを使用
                if (Action_time <= 0) InputCheck(active_skill);
            }
        }

        // 進行方向の決定
        if (isGetAxis)
        {
            SetVelocity();

            // y軸を軸としたカメラの回転を取得(Player専用)
            Quaternion horizontal_rot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

            // カメラの方向を考慮したキャラクターの方向ベクトルを作成
            Velocity = horizontal_rot * Velocity;

            // 回転が起きる場合はチャージ中のスキル解除・y軸を軸としたキャラクターの回転を取得
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
        // スキル未使用かつ 地面にいる
        if (Action_time <= 0 && isGround)
        {
            // 移動中の場合
            if (Velocity.magnitude > 0.1f)
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
        int exp_value = Exp; // UIセット用
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
    /// UIの再設定を行います(D_Manager.Liftから呼び出し)
    /// </summary>
    public void ResetPlayerStatusUI()
    {
        SetPlayerStatusUI_Max();
        SetPlayerStatusUI_Value();
    }

    /// <summary>
    /// 現在の最大ステータスをUIに反映します。シーン切り替え時や、レベルアップ時などに呼び出します。
    /// </summary>
    private void SetPlayerStatusUI_Max()
    {
        UI_Manager.UIInstance.Ps.Hp.SetSlider_MaxValue(status.MaxHp);
        UI_Manager.UIInstance.Ps.Sp.SetSlider_MaxValue(status.MaxSp);
        UI_Manager.UIInstance.Ps.Exp.SetSlider_MaxValue(status.Exp);
    }

    /// <summary>
    /// 現在のステータスをUIに反映します。差分を0にするので、シーン切り替え時のみ呼び出します。
    /// </summary>
    private void SetPlayerStatusUI_Value()
    {
        UI_Manager.UIInstance.Ps.Hp.SetSlider_Value(status.Hp);
        UI_Manager.UIInstance.Ps.Sp.SetSlider_Value(status.Sp);
        UI_Manager.UIInstance.Ps.Exp.SetSlider_Value(Exp);
    }
}