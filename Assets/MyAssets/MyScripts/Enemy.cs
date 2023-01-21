using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : Character
{
    private Transform player_transform; // プレイヤー位置情報

    private void Update()
    {
        if (!TimeCheck()) _actionTime -= Time.deltaTime;


        if (found)
        {
            trackingTime -= Time.deltaTime;
            FoundCheck();
        }
        else if (TimeCheck())
        {
            Rooteen();
        }
    }

    /// <summary>
    /// プレイヤーを見失うかどうか判定し、見失った場合は行動を変更します。
    /// </summary>
    private void FoundCheck()
    {
        if (trackingTime <= 0)
        {
            distance = DistanceCheck();
            if (distance > loseSightDistance)
            {
                found = false;
            }
            else
            {
                trackingTime = maxTrackingTime;
            }
        }

        if (found)
        {
            // 十分に距離を詰めていない場合はプレイヤーに向かって移動し続け、
            // 距離が十分な場合はスキルによる攻撃を開始します。
        }
        else
        {
            Search();
            // "?"を出す？
            Debug.Log(gameObject.name + "がプレイヤーを見失いました。");
        }
    }

    /// <summary>
    /// 未発見時の行動をします。
    /// </summary>
    protected virtual void Rooteen()
    {
        int rnd = Random.Range(1, 5);
        switch (rnd)
        {
            case 1:
                Move();
                break;
            case 2:
                Turn();
                break;
            case 3:
                Search();
                break;
            case 4:
                UseSkill();
                break;
        }
    }

    /// <summary>
    /// 発見時の行動をします。
    /// </summary>
    protected virtual void FoundRooteen()
    {
        for (int i = 0; i < activeSkill.Count; i++)
        {
            activeSkill[i].TrySkill();
            if (!TimeCheck()) break;
        }
    }

    /// <summary>
    /// プレイヤーとの距離を返します。
    /// </summary>
    protected float DistanceCheck()
    {
        return (transform.position - player_transform.position).magnitude;
    }

    /// <summary>
    /// 視覚・聴覚・魔法感知・本体接触で敵がプレイヤーを知覚したときに呼び出されます。
    /// </summary>
    /// <param name="player"></param>
    public void Founded(Transform player)
    {
        player_transform = player;
        found = true;
        _actionTime = 0; // 即座に見つけたときの行動ルーチンに移る
        trackingTime = maxTrackingTime;
        Debug.Log(gameObject.name + "がプレイヤーを発見しました！！");
    }

    protected virtual void Move() 
    {
        transform.position += Vector3.left; // 後で作り変え
    }

    protected virtual void Turn()
    {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(0.1f, Vector3.up);
    }

    protected virtual void Search()
    {
        _actionTime = stopTime;
    }
    protected virtual void UseSkill()
    {
        
    }


    public override void FoundEnemy() { }
    public override void LoseSightEnemy() { }
    public override void Beat() { }
    protected override void DeathCharacter() { }

    private void OnColliderEnter(Collision other)
    {
        // プレイヤーの身体と敵の身体が接触したときは敵がプレイヤーを知覚します。
        if (other.gameObject.CompareTag("Player_Body") && !found)
        {
            Transform player = other.gameObject.GetComponent<Transform>();
            Founded(player);
        }
    }

    [Tooltip("プレイヤーを見失うまでの時間を設定します。")]
    [SerializeField]
    private float maxTrackingTime;

    [Tooltip("プレイヤーを見失う距離を設定します。")]
    [SerializeField]
    private float loseSightDistance;

    [Tooltip("プレイヤー攻撃する距離を'自動で'設定します。")]
    [SerializeField]
    private float AttackDistance;

    [Tooltip("プレイヤーを知覚している状態かどうかを判定します。(true=気づいている)")]
    [SerializeField]
    private bool found = false;

    [Tooltip("敵はtrackingTimeが残っている間、プレイヤーを狙い続けます。")]
    [SerializeField]
    private float trackingTime = 0;

    [Tooltip("一時的に停止してから次の行動に移るまでの時間です。")]
    [SerializeField]
    private float stopTime = 2.0f;

    [Tooltip("敵とプレイヤーの距離です。")]
    [SerializeField]
    private float distance;
}