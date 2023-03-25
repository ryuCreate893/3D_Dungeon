using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class Enemy : Character
{
    [SerializeField]
    private NavMeshAgent nav_mesh_agent;

    [Header("スキル情報")]
    [SerializeField, Tooltip("アクティブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> active_skill;
    [SerializeField, Tooltip("ムーブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> move_skill;
    [SerializeField, Tooltip("パッシブスキルの一覧")]
    private List<PassiveSkill> passive_skill;

    /// <summary>
    /// 敵がプレイヤーを知覚しているかどうかを判定(true=気づいている)
    /// </summary>
    private bool isFound = false;

    // *** isFound = false時に使用する変数 ***
    /// <summary>
    /// 敵対していない状態におけるムーブスキルの発動確率(基本90%)
    /// </summary>
    public int move_judge { get; private set; } = 90;


    // *** isFound = true時に使用する変数 ***
    /// <summary>
    /// プレイヤーを攻撃し始める距離
    /// </summary>
    public float atk_distance { get; private set; } = 0;
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

    protected override void Start()
    {
        base.Start();
        turn_speed /= 4;
        nav_mesh_agent.isStopped = true;
    }


    protected virtual void Update()
    {
        // 知覚判定の切り替え
        if (isFound)
        {
            if (tracking_time > 0)
            {
                tracking_time -= Time.deltaTime;
            }
            else
            {
                if (Focus.magnitude <= lose_distance)
                {
                    tracking_time = max_tracking_time;
                }
                else
                {
                    LoseSight(); // プレイヤーを見失う
                }
            }
        }

        // アクション内容の切り替え
        if (Action_time > 0)
        {
            Action_time -= Time.deltaTime;
            nav_mesh_agent.isStopped = true;

            SetFocusAngle();
            my_transform.rotation = Quaternion.RotateTowards(my_transform.rotation, My_rot, turn_speed * Time.deltaTime);
        }
        else if (isFound) // アクション時間経過, 発見済
        {
            ChangeFoundRooteen();
        }
        else // アクション時間経過, 未発見
        {
            ChangeRooteen();
        }
    }

    private void FixedUpdate()
    {
        if (nav_mesh_agent.isStopped)
        {
            my_rigidbody.velocity = new Vector3(My_vel.x, my_rigidbody.velocity.y, My_vel.z);
        }
    }

    // *** Update内で呼び出すメソッド ***
    /// <summary>
    /// 向くべき方向を決定(targetがいない場合は無効)
    /// </summary>
    private void SetFocusAngle()
    {
        if (target_transform != null)
        {
            Focus = target_transform.position - my_transform.position;
            Vector3 v3 = new Vector3(Focus.x, 0, Focus.z).normalized;
            My_rot = Quaternion.LookRotation(v3, my_transform.up);
        }
    }

    /// <summary>
    /// 知覚中の "Action_time = 0" 時の行動
    /// </summary>
    protected virtual void ChangeFoundRooteen()
    {
        // スキルのチャージ中
        if (isCharge)
        {
            SetFocusAngle(); // 注目
            Action?.Invoke(); // スキルの発動
        }

        // スキル未発動、射程内
        else if ((target_transform.position - my_transform.position).magnitude <= atk_distance)
        {
            nav_mesh_agent.isStopped = true; // 追跡の停止
            SetFocusAngle(); // 注目
            UseSkill(active_skill, SkillType.battle); // スキルの発動
        }

        // スキル未発動、射程外
        else
        {
            nav_mesh_agent.isStopped = false; // 追跡
            nav_mesh_agent.SetDestination(target_transform.position);
        }
    }

    /// <summary>
    /// 未発見の "Action_time = 0" 時の行動
    /// </summary>
    protected virtual void ChangeRooteen()
    {
        // スキルのチャージ中
        if (isCharge)
        {
            Action?.Invoke(); // スキルの発動
        }

        // スキル未発動
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




    /// <summary>
    /// 敵の身体がプレイヤーとぶつかったときに知覚
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player_Body") && !isFound)
        {
            Found(other.gameObject);
        }
    }

    /// <param name="target"></param>
    public void Found(GameObject player)
    {
        target_method = player.GetComponent<Character>();
        target_transform = player.GetComponent<Transform>();
        turn_speed *= 4;
        isFound = true;
        tracking_time = max_tracking_time;
        Action_time = 0; // 即座に見つけたときの行動ルーチンに移る

        nav_mesh_agent.speed = status.Speed;
        nav_mesh_agent.angularSpeed = turn_speed;
        nav_mesh_agent.stoppingDistance = atk_distance;
        nav_mesh_agent.isStopped = false;

        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }

    private void LoseSight()
    {
        Action_cancel?.Invoke();
        target_method = null;
        target_transform = null;
        turn_speed /= 4;
        isFound = false;
        tracking_time = 0;
        My_rot = my_transform.rotation;
        Search();
        Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
    }

    /// <summary>
    /// プレイヤーを見失ったとき専用のメソッド
    /// </summary>
    protected virtual void Search()
    {
        My_vel = Vector3.zero;
        Action_time = 2.0f;
    }

    public override void Beat(int exp)
    {
        LevelUp(); // 敵が敵を倒すとレベルが1上がります。
    }

    protected override void DeathCharacter()
    {
        // 死亡アニメーション
        Player.playerInstance.Beat(status.Exp);
        Destroy(gameObject);
    }
}