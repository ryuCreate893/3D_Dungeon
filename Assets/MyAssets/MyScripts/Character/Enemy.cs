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

    [Header("その他の変数データ")]
    [SerializeField, Tooltip("プレイヤーを知覚していない場合のムーブスキルの発動確率\n(確率から外れた場合はアクティブスキルを使用します")]
    private int moveProbability = 90;
    [SerializeField, Tooltip("敵がプレイヤー攻撃し始める距離")]
    private float attackDistance;
    [SerializeField, Tooltip("敵がプレイヤーを見失う距離")]
    private float loseSightDistance;
    [SerializeField, Tooltip("敵がプレイヤーを見つけてから見失うまでの時間")]
    private float maxTrackingTime;

    /// <summary>
    /// 敵がプレイヤーの追跡を続ける時間
    /// </summary>
    private float trackingTime = 0;

    /// <summary>
    /// 敵がプレイヤーを知覚しているかどうかを判定(true=気づいている)
    /// </summary>
    private bool isFound = false;

    protected virtual void Update()
    {
        if (isFound)
        {
            trackingTime -= Time.deltaTime;
            // 追跡時間が経過した場合の処理(発見、未発見の切り替え)
            if (trackingTime <= 0)
            {
                if (CheckTargetDistence(loseSightDistance)) // プレイヤーを続けて追跡する
                {
                    trackingTime = maxTrackingTime;
                    SetCharacterAngle();
                }
                else // プレイヤーを見失う
                {
                    LoseSightEnemy(); // _targetTransform = nullに設定
                    SetCharacterAngle();
                }
            }
            // 追跡時間が終わるまではプレイヤーの方を向き続ける
            else
            {
                SetCharacterAngle();
            }
        }

        // アクション時間が経過した場合の処理(Rooteenの切り替え)
        if (_actionTime <= 0)
        {
            _actionTime -= Time.deltaTime;
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
    }

    /// <summary>
    /// 向くべき方向を決定します。
    /// </summary>
    protected override void SetCharacterAngle()
    {
        FocusTarget();

        if (_focus == _transform.forward)
        {
            SetRotation(Quaternion.identity);
        }
        else
        {
            SetRotation(Quaternion.FromToRotation(_transform.forward, _focus));
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, _current.Speed * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤー知覚時の行動をします。
    /// </summary>
    protected virtual void ChangeFoundRooteen()
    {
        if (_chargeSkill != -1) // スキルをチャージしている場合
        {
            activeSkill[_chargeSkill].Skill.TrySkill();
        }
        else if (CheckTargetDistence(attackDistance)) // 攻撃の射程内である場合
        {
            _velocity = Vector3.zero;
            for (int i = 0; i < activeSkill.Count; i++)
            {
                if (activeSkill[i].Skill.SkillTypeCheck() != SkillType.ordinary)
                {
                    if (activeSkill[i].JudgeBattle)
                    {
                        activeSkill[i].Skill.TrySkill();
                        if (_actionTime <= 0) break; // スキルが選択された場合はfor文から抜ける
                    }
                }
            }
        }
        else // 射程外にプレイヤーがいる場合は追跡行動をとる
        {
            _velocity = _transform.forward * _current.Speed;
        }
    }

    /// <summary>
    /// プレイヤー未発見時の行動をします。
    /// </summary>
    protected virtual void ChangeRooteen()
    {
        if (_chargeSkill != -1) // スキルをチャージしている場合
        {
            activeSkill[_chargeSkill].Skill.TrySkill();
            _chargeSkill = -1;
        }
        else // スキルをチャージしていない場合
        {
            if (Random.Range(1, 101) <= moveProbability)
            {
                UseOrdinarySkill(moveSkill);
            }
            else
            {
                UseOrdinarySkill(activeSkill);
            }
        }
    }


    // *** スキル管理関係メソッド ***
    /// <summary>
    /// 引数に"moveSkill"か"activeSkill"を指定し、スキルの発動判定を行います。
    /// </summary>
    private void UseOrdinarySkill(List<EnemyActiveSkill> skillList)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].Skill.SkillTypeCheck() != SkillType.battle)
            {
                skillList[i].Skill.TrySkill();
                if (_actionTime > 0) break;
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
    /// ゲームオブジェクトのみを引数とするパターン(パッシブスキルのみ使用)
    /// </summary>
    public override void GetNewSkill(GameObject skill)
    {
        int n = passiveSkill.Count;
        passiveSkill.Add(skill.GetComponent<PassiveSkill>());
        passiveSkill[n].SkillContent();
    }

    /// <summary>
    /// ゲームオブジェクトに加えて2つの確率を引数とするパターン(ムーブ・アクティブスキルで使用)
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
        LevelUp(1); // 敵が敵を倒すとレベルが1上がります。
    }

    protected override void DeathCharacter()
    {
        // 死亡アニメーション
    }


    // *** 知覚判定を切り替えるメソッド ***
    /// <summary>
    /// 敵の身体がプレイヤーとぶつかったとき、敵がプレイヤーを知覚します。
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player_Body") && !isFound) FoundEnemy(other.gameObject);
    }

    // 各知覚判定から呼び出し
    public override void FoundEnemy(GameObject target)
    {
        _target = target.GetComponent<Character>();
        _targetTransform = _target.GetComponent<Transform>();
        isFound = true;
        _actionTime = 0; // 即座に見つけたときの行動ルーチンに移る
        trackingTime = maxTrackingTime;
        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }

    // プレイヤーを見失う処理
    protected override void LoseSightEnemy()
    {
        _target = null;
        _targetTransform = null;
        isFound = false;
        _chargeSkill = -1;
        _characterRotation = _transform.rotation;
        Search();
        Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
    }

    /// <summary>
    /// プレイヤーを見失ったとき専用のメソッドで、一定時間プレイヤーを探します。
    /// </summary>
    protected virtual void Search()
    {
        _velocity = Vector3.zero;
        _actionTime = 2.0f;
    }
}