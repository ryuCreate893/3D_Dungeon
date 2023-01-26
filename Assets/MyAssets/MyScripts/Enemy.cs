using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : Character
{
    [SerializeField, Tooltip("敵がプレイヤーを見失う距離")]
    private float loseSightDistance;

    [SerializeField, Tooltip("敵がプレイヤーを見失う判定を呼び出す時間")]
    private float maxTrackingTime;

    [SerializeField, Tooltip("敵がプレイヤー攻撃し始める距離")]
    private float attackDistance;

    /// <summary>
    /// 敵が次の行動に移るまでの時間
    /// </summary>
    //private float stopTime = 2.5f;

    /// <summary>
    /// 敵がプレイヤーを知覚しているかどうかを判定(true=気づいている)
    /// </summary>
    private bool found = false;

    /// <summary>
    /// 敵がプレイヤーの追跡を続ける時間
    /// </summary>
    private float trackingTime = 0;


    private void Update()
    {
        // プレイヤーを知覚している場合
        if (found)
        {
            trackingTime -= Time.deltaTime;
            if (TimeCheck(trackingTime)) LoseSightCheck();
        }

        if (!TimeCheck(_actionTime))
        {
            _actionTime -= Time.deltaTime;
        }
        else
        {
            if (found)
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
    /// trackingTimeが0になったとき、プレイヤーを見失うかどうか判定します。
    /// </summary>
    private void LoseSightCheck()
    {
        if (CheckTargetDistence(loseSightDistance))
        {
            trackingTime = maxTrackingTime;
        }
        else
        {
            LoseSightEnemy();
        }
    }

    protected override void LoseSightEnemy()
    {
        found = false;
        _targetTransform = null;
        _targetMethod = null;
        Search();
        Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
    }

    /// <summary>
    /// プレイヤー知覚時の行動をします。
    /// </summary>
    protected virtual void ChangeFoundRooteen()
    {
        Vector3 v3 = FocusTarget();
        v3 = new Vector3(v3.x, 0, v3.z).normalized;
        SetCharacterAngle(v3);

        if (CheckTargetDistence(attackDistance))
        {
            _velocity = Vector3.zero;
            if (_chargeSkillnumber != -1) // スキルをチャージしている場合
            {
                _activeSkill[_chargeSkillnumber].TrySkill();
                _chargeSkillnumber = -1;
            }
            else // スキルをチャージしていない場合
            {
                for (int i = 0; i < _activeSkill.Count; i++)
                {
                    if (_activeSkill[i].SkillTypeCheck() != SkillType.ordinary)
                    {
                        // ★確率を実装したい★
                        _chargeSkillnumber = _activeSkill[i].ChargeSkill(i);
                        // スキルが選択された場合はfor文から抜ける
                        if (!TimeCheck(_actionTime)) break;
                    }
                }
            }
        }
        else
        {
            _velocity = v3 * _current.Speed;
            _chargeSkillnumber = -1;
        }
    }

    /// <summary>
    /// プレイヤー未発見時の行動をします。
    /// </summary>
    protected virtual void ChangeRooteen()
    {
        if (_chargeSkillnumber != -1)
        {
            _activeSkill[_chargeSkillnumber].TrySkill();
            _chargeSkillnumber = -1;
        }
        else
        {
            int rnd = Random.Range(1, 5);
            switch (rnd)
            {
                case 1: // 移動
                    Move();
                    break;
                case 2: // 回転
                    Turn();
                    break;
                case 3: // 見回す
                    Search();
                    break;
                case 4: // スキル
                    UseSkill();
                    break;
            }
        }
    }

    protected override void SetCharacterAngle(Vector3 v3look)
    {
        // 現在の向きからv3lookへの回転を取得(Enemy専用)
        Quaternion _horizontalRotation = Quaternion.FromToRotation(transform.forward, v3look);

        // キャラクターの方向ベクトルを作成
        Vector3 v3 = _horizontalRotation * transform.forward;

        // y軸を軸としたキャラクターの回転を取得
        _characterRotation = Quaternion.LookRotation(v3, Vector3.up);

        // 現在の向きから移動後の向きまで回転させる
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _characterRotation, 1200 * Time.deltaTime);
    }

    /// <summary>
    /// 真っ直ぐ進みます。
    /// </summary>
    protected virtual void Move()
    {
        _velocity = transform.forward * _current.Speed;
        _actionTime = 1;
    }

    /// <summary>
    /// 向きを変えます。
    /// </summary>
    protected virtual void Turn()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        SetCharacterAngle(new Vector3(x, 0, z).normalized);
        _actionTime = 2;
    }

    /// <summary>
    /// 周りを見渡してプレイヤーを探します。
    /// </summary>
    protected virtual void Search()
    {
        _velocity = Vector3.zero;
        _actionTime = 2;
    }

    /// <summary>
    /// 持っているスキルを使用します。
    /// </summary>
    protected virtual void UseSkill()
    {
        _velocity = Vector3.zero;
        for (int i = 0; i < _activeSkill.Count; i++)
        {
            if (_activeSkill[i].SkillTypeCheck() != SkillType.battle)
            {
                _chargeSkillnumber = _activeSkill[i].ChargeSkill(i);
            }
            if (!TimeCheck(_actionTime)) break;
        }
    }

    /// <summary>
    /// 視覚・聴覚・魔法感知・本体接触で敵がプレイヤーを知覚したときに呼び出されます。
    /// </summary>
    /// <param name="player"></param>
    public override void FoundEnemy(GameObject target)
    {
        _targetTransform = target.GetComponent<Transform>();
        found = true;
        _actionTime = 0; // 即座に見つけたときの行動ルーチンに移る
        trackingTime = maxTrackingTime;
        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }

    public override void Beat() { }
    protected override void DeathCharacter() { }

    private void OnCollisionEnter(Collision other)
    {
        // プレイヤーの身体と敵の身体が接触したときは敵がプレイヤーを知覚します。
        if (other.gameObject.CompareTag("Player_Body") && !found)
        {
            FoundEnemy(other.gameObject);
        }
    }


}