using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : Character
{
    [Header("スキル情報")]
    [SerializeField, Tooltip("アクティブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> active_skill;
    [SerializeField, Tooltip("ムーブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> move_skill;
    [SerializeField, Tooltip("パッシブスキルの一覧")]
    private List<PassiveSkill> passive_skill;


    /// <summary>
    /// 敵対していない状態におけるムーブスキルの発動確率(基本90%)
    /// </summary>
    public int move_judge { get; protected set; } = 90;
    /// <summary>
    /// プレイヤーを攻撃し始める距離
    /// </summary>
    public float atk_distance { get; protected set; } = 0;
    /// <summary>
    /// プレイヤーを見失う距離
    /// </summary>
    public float lose_distance { get; protected set; } = 5.0f;
    /// <summary>
    /// プレイヤーを追跡する時間
    /// </summary>
    public float max_tracking_time { get; protected set; } = 5.0f;
    /// <summary>
    /// プレイヤーを追跡する時間の経過状況
    /// </summary>
    private float tracking_time = 0;
    /// <summary>
    /// 敵がプレイヤーを知覚しているかどうかを判定(true=気づいている)
    /// </summary>
    private bool isFound = false;

    private void Awake()
    {
        turn_speed /= 4; // プレイヤーを知覚していない場合は回転をゆっくりに
    }

    protected virtual void Update()
    {
        // 知覚判定の切り替え
        if (isFound)
        {
            if (tracking_time > 0)
            {
                tracking_time -= Time.deltaTime;
                SetFocusAngle();
            }
            else
            {
                if (Focus.magnitude <= lose_distance)
                {
                    tracking_time = max_tracking_time;
                    SetFocusAngle();
                }
                else
                {
                    LoseSightEnemy();
                }
            }
        }

        // アクション内容の切り替え
        if (Action_time > 0)
        {
            Action_time -= Time.deltaTime;
        }
        else
        {
            if (isFound)
            {
                ChangeFoundRooteen();
            }
            else
            {
                ChangeRooteen();
            }
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Character_rot, turn_speed * Time.deltaTime);
    }

    // *** Update内で呼び出すメソッド ***
    /// <summary>
    /// プレイヤーの方向に向くための回転"Character_rot"を設定
    /// </summary>
    private void SetFocusAngle()
    {
        FocusTarget();
        Vector3 v3 = new Vector3(Focus.x, 0, Focus.z).normalized;
        Character_rot = Quaternion.LookRotation(v3, _transform.up);
    }

    /// <summary>
    /// プレイヤーを知覚しているときの行動
    /// </summary>
    protected virtual void ChangeFoundRooteen()
    {
        if (isCharge)
        {
            Action?.Invoke();
        }
        else if (Focus.magnitude <= atk_distance)
        {
            Velocity = Vector3.zero;
            UseSkill(active_skill, SkillType.battle);
        }
        else
        {
            Velocity = _transform.forward * status.Speed; // 追跡
        }
    }

    /// <summary>
    /// プレイヤーを発見していないときの行動
    /// </summary>
    protected virtual void ChangeRooteen()
    {
        if (isCharge)
        {
            Action?.Invoke();
        }
        else
        {
            if (Random.Range(1, 101) <= move_judge) // 非戦闘時move_skillの使用
            {
                UseSkill(move_skill, SkillType.ordinary);
            }
            else // 非戦闘時active_skillの使用
            {
                UseSkill(active_skill, SkillType.ordinary);
            }
        }
    }

    /// <summary>
    /// プレイヤーを見失う処理("Character_rot"もリセットされる)
    /// </summary>
    protected override void LoseSightEnemy()
    {
        Action_cancel?.Invoke();
        target = null;
        Target_transform = null;
        isFound = false;
        turn_speed /= 4;
        Character_rot = _transform.rotation;
        Search();
        Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
    }

    /// <summary>
    /// プレイヤーを見失ったとき専用のメソッドで、一定時間プレイヤーを探す動きを実装
    /// </summary>
    protected virtual void Search()
    {
        Velocity = Vector3.zero;
        Action_time = 2.0f;
    }

    // *** スキルに関わるメソッド ***
    /// <summary>
    /// 引数に"move_skill"か"active_skill"を指定し、発動判定を行使します。
    /// </summary>
    private void UseSkill(List<EnemyActiveSkill> list, SkillType type)
    {
        SkillType exclusion; // 判定から除外するスキル
        if (type == SkillType.battle)
        {
            exclusion = SkillType.ordinary;
        }
        else
        {
            exclusion = SkillType.battle;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Type != exclusion)
            {
                if (list[i].Judge(type))
                {
                    if (list[i].Skill.TrySkill())
                    {
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
    }

    protected override void SetActiveSkill()
    {
        for (int i = 0; i < active_skill.Count; i++)
        {
            active_skill[i].Skill.SetSkill(gameObject);
            if (active_skill[i].Skill.Range * 0.8f > atk_distance)
            {
                atk_distance = active_skill[i].Skill.Range * 0.8f;
            }
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


    /// <summary>
    /// 敵キャラクターに追加でスキルを習得させる(パッシブスキル専用)
    /// </summary>
    public override void GetNewSkill(GameObject skill)
    {
        int n = passive_skill.Count;
        passive_skill.Add(skill.GetComponent<PassiveSkill>());
        passive_skill[n].SkillContent();
    }


    /// <summary>
    /// 敵キャラクターに追加でスキルを習得させる(ムーブ・アクティブスキルで使用。確率の引数を2つ指定)
    /// </summary>
    public void GetNewSkill(GameObject skill, SkillType type, int po, int pb)
    {
        List<EnemyActiveSkill> list;
        switch (skill.tag)
        {
            case "ActiveSkill":
                list = active_skill;
                break;
            case "MoveSkill":
                list = move_skill;
                break;
            default:
                Debug.LogError("オブジェクトのタグが不正です。");
                list = null;
                break;
        }

        int n = list.Count;
        list.Add(new EnemyActiveSkill()
        { Skill = skill.GetComponent<ActiveSkill>(), Type = type, P_ordinary = po, P_battle = pb });
        list[n].Skill.SetSkill(gameObject);
    }

    // *** 戦闘関係メソッド ***
    public override void Beat(int exp)
    {
        LevelUp(1); // 敵が敵を倒すとレベルが1上がります。
    }

    protected override void DeathCharacter()
    {
        // 死亡アニメーション
    }


    // *** Update以外で知覚判定を切り替えるメソッド ***
    /// <summary>
    /// 敵の身体がプレイヤーとぶつかったときに知覚
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player_Body") && !isFound)
        {
            FoundEnemy(other.gameObject);
        }
    }


    /// <summary>
    /// 視覚・聴覚・魔法感知が発生した場合に呼び出されます。
    /// </summary>
    /// <param name="target"></param>
    public override void FoundEnemy(GameObject player)
    {
        target = player.GetComponent<Character>();
        Target_transform = player.GetComponent<Transform>();
        turn_speed *= 4;
        isFound = true;
        Action_time = 0; // 即座に見つけたときの行動ルーチンに移る
        tracking_time = max_tracking_time;
        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }
}