using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : Character
{
    [Header("スキル情報")]
    [SerializeField, Tooltip("アクティブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> activeSkill;
    [SerializeField, Tooltip("ムーブスキルの一覧と発動確率")]
    private List<EnemyActiveSkill> moveSkill;
    [SerializeField, Tooltip("パッシブスキルの一覧")]
    private List<PassiveSkill> passiveSkill;

    /// <summary>
    /// 敵対していない状態におけるムーブスキルの発動確率(基本90%)
    /// </summary>
    public int MoveProbability { get; protected set; } = 90;
    /// <summary>
    /// プレイヤーを攻撃し始める距離
    /// </summary>
    public float AttackDistance { get; protected set; } = 0;
    /// <summary>
    /// プレイヤーを見失う距離
    /// </summary>
    public float LoseSightDistance { get; protected set; } = 5.0f;
    /// <summary>
    /// プレイヤーを追跡する時間
    /// </summary>
    public float MaxTrackingTime { get; protected set; } = 5.0f;

    /// <summary>
    /// プレイヤーを追跡する時間の経過状況
    /// </summary>
    private float trackingTime = 0;
    /// <summary>
    /// 敵がプレイヤーを知覚しているかどうかを判定(true=気づいている)
    /// </summary>
    private bool isFound = false;

    protected override void Awake()
    {
        base.Awake();
        _turnSpeed /= 4;
    }

    protected virtual void Update()
    {
        // プレイヤーを発見している場合
        if (isFound)
        {
            // 追跡時間が残っている
            if (trackingTime > 0)
            {
                trackingTime -= Time.deltaTime;
                SetFocusAngle();
            }

            // 追跡時間が経過した (見失った場合は"LoseSightEnemy()"を呼び出す)
            else
            {
                if (CheckTargetDistence(LoseSightDistance))
                {
                    trackingTime = MaxTrackingTime;
                    SetFocusAngle();
                }
                else
                {
                    LoseSightEnemy();
                }
            }
        }


        // アクション時間が残っている
        if (_actionTime > 0)
        {
            _actionTime -= Time.deltaTime;
        }

        // アクション時間が経過した
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

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, _turnSpeed * Time.deltaTime);
    }


    // *** Update内で呼び出すメソッド ***
    /// <summary>
    /// プレイヤーの方向に向くための回転"_characterRotation"を設定
    /// </summary>
    private void SetFocusAngle()
    {
        FocusTarget();
        Vector3 v3 = new Vector3(_focus.x, 0, _focus.z).normalized;
        _characterRotation = Quaternion.LookRotation(v3, _transform.up);
    }


    /// <summary>
    /// プレイヤーを知覚しているときの行動
    /// </summary>
    protected virtual void ChangeFoundRooteen()
    {
        // スキルをチャージしている
        if (_chargeSkill != -1)
        {
            activeSkill[_chargeSkill].Skill.TrySkill();
        }

        // スキルはチャージしていないが、攻撃の射程内である
        else if (CheckTargetDistence(AttackDistance))
        {
            _velocity = Vector3.zero;
            for (int i = 0; i < activeSkill.Count; i++)
            {
                if (activeSkill[i].Type != SkillType.ordinary)
                {
                    if (activeSkill[i].JudgeBattle)
                    {
                        activeSkill[i].Skill.TrySkill();
                        if (_actionTime > 0) break; // スキルが選択された場合はfor文から抜ける
                    }
                }
            }
        }

        // プレイヤーを知覚しているが、射程外にいる
        else
        {
            _velocity = _transform.forward * _status.Speed;
        }
    }


    /// <summary>
    /// プレイヤーを発見していないときの行動
    /// </summary>
    protected virtual void ChangeRooteen()
    {
        // スキルをチャージしている
        if (_chargeSkill != -1)
        {
            if (activeSkill[_chargeSkill].Skill.TrySkill())
            {
                activeSkill[_chargeSkill].Skill.SkillContent();
            }
            _chargeSkill = -1;
        }

        // スキルをチャージしていない
        else
        {
            // moveSkillの使用
            if (Random.Range(1, 101) <= MoveProbability)
            {
                UseOrdinarySkill(moveSkill);
            }

            // activeSkillの使用
            else
            {
                UseOrdinarySkill(activeSkill);
            }
        }
    }


    /// <summary>
    /// プレイヤーを見失う処理("_characterRotation"もリセットされる)
    /// </summary>
    protected override void LoseSightEnemy()
    {
        _target = null;
        _targetTransform = null;
        isFound = false;
        _chargeSkill = -1;
        _turnSpeed /= 4;
        _characterRotation = _transform.rotation;
        Search();
        Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
    }


    /// <summary>
    /// プレイヤーを見失ったとき専用のメソッドで、一定時間プレイヤーを探す動きを実装
    /// </summary>
    protected virtual void Search()
    {
        _velocity = Vector3.zero;
        _actionTime = 2.0f;
    }



    // *** スキルに関わるメソッド ***
    /// <summary>
    /// 引数に"moveSkill"か"activeSkill"を指定し、非戦闘時のスキル発動を判定します。
    /// </summary>
    private void UseOrdinarySkill(List<EnemyActiveSkill> skillList)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].Type != SkillType.battle) // 戦闘時以外に使用できるスキルの判定
            {
                if (skillList[i].JudgeOrdinary) // 確率の判定
                {
                    if (skillList[i].Skill.TrySkill()) // 使用可能かどうかの判定
                    {
                        skillList[i].Skill.SkillContent();
                        break;
                    }
                }
            }
        }
    }


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


    /// <summary>
    /// 敵キャラクターに追加でスキルを習得させる(パッシブスキル専用)
    /// </summary>
    public override void GetNewSkill(GameObject skill)
    {
        int n = passiveSkill.Count;
        passiveSkill.Add(skill.GetComponent<PassiveSkill>());
        passiveSkill[n].SkillContent();
    }


    /// <summary>
    /// 敵キャラクターに追加でスキルを習得させる(ムーブ・アクティブスキルで使用。確率の引数を2つ指定)
    /// </summary>
    public void GetNewSkill(GameObject skill, int po, int pb)
    {
        int n;
        switch (skill.tag)
        {
            case "ActiveSkill":
                n = activeSkill.Count;
                activeSkill.Add(new EnemyActiveSkill()
                { Skill = skill.GetComponent<ActiveSkill>(), P_ordinary = po, P_battle = pb });
                activeSkill[n].Skill.SetSkill(gameObject);
                break;
            case "MoveSkill":
                n = moveSkill.Count;
                moveSkill.Add(new EnemyActiveSkill()
                { Skill = skill.GetComponent<ActiveSkill>(), P_ordinary = po, P_battle = pb });
                moveSkill[n].Skill.SetSkill(gameObject);
                break;
            case "PassiveSkill":
                GetNewSkill(skill);
                break;
            default:
                Debug.LogError("スキルを登録しているオブジェクトのタグが不正です。");
                break;
        }
    }


    protected override void DamagedCancel(int n)
    {
        activeSkill[n].Skill.DamagedCancel();
    }



    // *** 戦闘関係メソッド ***
    public override void Beat()
    {
        _status.LevelUp(1); // 敵が敵を倒すとレベルが1上がります。
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
    /// 視覚・聴覚・魔法感知が発声した場合に呼び出されます。
    /// </summary>
    /// <param name="target"></param>
    public override void FoundEnemy(GameObject target)
    {
        _target = target.GetComponent<Character>();
        _targetTransform = _target.GetComponent<Transform>();
        _turnSpeed *= 4;
        isFound = true;
        _actionTime = 0; // 即座に見つけたときの行動ルーチンに移る
        trackingTime = MaxTrackingTime;
        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }
}