using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{
    // *** アクション中の行動内容を管理するデリゲート ***
    public delegate void ActionMethod();
    public ActionMethod Action;

    public delegate void CancelMethod();
    public CancelMethod Action_cancel;

    public delegate void DamagedMethod();
    public DamagedMethod Damaged;

    // *** 自分自身のコンポーネント ***
    [SerializeField]
    protected Transform my_transform;
    [SerializeField]
    protected Rigidbody my_rigidbody;
    [SerializeField]
    protected Animator my_animator;

    // *** コンポーネント以外の情報 ***
    /// <summary>
    /// キャラクターの移動方向
    /// </summary>
    public Vector3 My_vel { protected get; set; }
    /// <summary>
    /// キャラクターの回転方向
    /// </summary>
    public Quaternion My_rot { protected get; set; }
    /// <summary>
    /// アクション一つひとつに掛かる時間
    /// </summary>
    public float Action_time { get; set; }
    /// <summary>
    /// スキルのチャージ状態を保有
    /// </summary>
    public bool isCharge { get; set; } = false;
    /// <summary>
    /// 回転スピード
    /// </summary>
    protected float turn_speed = 360;

    /// <summary>
    /// 標的のメソッド
    /// </summary>
    protected Character target_method;
    /// <summary>
    /// 標的の座標
    /// </summary>
    public Transform target_transform { get; set; }
    /// <summary>
    /// 標的への方向ベクトル
    /// </summary>
    public Vector3 Focus { get; set; }

    [SerializeField, Tooltip("ステータス情報")]
    protected CharacterStatus status;
    public CharacterStatus Status { get { return status; } }


    protected virtual void Start()
    {
        // 初期ステータスのセット
        status.Level = status.Base_Level;
        status.MaxHp = status.Base_MaxHp;
        status.Hp = status.MaxHp;
        status.MaxSp = status.Base_MaxSp;
        status.Sp = status.MaxSp;
        status.Atk = status.Base_Atk;
        status.Def = status.Base_Def;
        status.Speed = status.Base_Speed; // Speedのみfloat値
        status.Exp = status.Base_Exp;

        SetActiveSkill(); // アクティブスキルのセット(Player, Enemyで異なる処理)
        SetPassiveSkill(); // パッシブスキルのセット(Player, Enemyで異なる処理)

        My_rot = my_transform.rotation; // 回転の設定
        Debug.Log("キャラクターセット完了！ キャラクター名 : " + gameObject.name);
    }


    // *** バトル関連 メソッド ******************
    /// <summary>
    /// ダメージ計算
    /// </summary>
    public void DamageCalculation(int enemy_attack)
    {
        int damage;
        if (status.Def == 0)
        {
            damage = enemy_attack;
        }
        else if (status.Def >= 100)
        {
            damage = 0;
        }
        else
        {
            damage = enemy_attack * (100 - status.Def) / 100;
        }

        if (damage > 0) CharacterDamaged(damage);
    }

    /// <summary>
    /// ダメージ計算(引数に防御倍率"(0.0 - 1.0)倍"を指定)
    /// </summary>
    public void DamageCalculation(int enemy_attack, float weak)
    {
        int def = status.Def;
        if (weak > 1) weak = 1;
        status.Def = (int)(status.Def * weak);
        DamageCalculation(enemy_attack);
        status.Def = def;
    }

    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    protected virtual void CharacterDamaged(int damage)
    {
        status.Hp -= damage;

        if (status.Hp <= 0)
        {
            status.Hp = 0;
            DeathCharacter();
        }
        else
        {
            Damaged?.Invoke();
        }
    }

    public void LevelUp()
    {
        status.Level++;

        status.MaxHp -= status.AddHp;
        status.MaxHp *= (status.GrowMaxHp + 100) / 100;
        status.MaxHp += status.AddHp;
        status.Hp = status.MaxHp;

        status.MaxSp -= status.AddSp;
        status.MaxSp *= (status.GrowMaxSp + 100) / 100;
        status.MaxSp += status.AddSp;
        status.Sp = status.MaxSp;

        status.Atk -= status.AddAtk;
        status.Atk *= (status.GrowAtk + 100) / 100;
        status.Atk += status.AddAtk;

        status.Exp *= (status.GrowExp + 100) / 100;
    }

    public void LevelDown()
    {
        if (status.Level > 1)
        {
            status.Level--;

            status.MaxHp -= status.AddHp;
            status.MaxHp /= (status.GrowMaxHp + 100) / 100;
            status.MaxHp += status.AddHp;
            if (status.Hp > status.MaxHp) status.Hp = status.MaxHp;

            status.MaxSp -= status.AddSp;
            status.MaxSp /= (status.GrowMaxSp + 100) / 100;
            status.MaxSp += status.AddSp;
            if (status.Sp > status.MaxSp) status.Sp = status.MaxSp;

            status.Atk -= status.AddAtk;
            status.Atk /= (status.GrowAtk + 100) / 100;
            status.Atk += status.AddAtk;

            status.Exp /= (status.GrowExp + 100) / 100;
        }
    }

    // *** abstract メソッド ******************
    abstract protected void SetActiveSkill();
    abstract protected void SetPassiveSkill();
    abstract public void GetNewSkill(GameObject skill);

    /// <summary>
    /// 敵を倒したときの処理
    /// </summary>
    abstract public void Beat(int exp);

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();
}