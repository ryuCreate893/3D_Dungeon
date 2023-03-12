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

    // *** 自分自身のコンポーネントを保有する変数 ***
    [SerializeField]
    protected Transform _transform;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    protected Animator _animator;
    [SerializeField, Tooltip("キャラクター子オブジェクトの音コライダーを登録します。")]
    private Collider sound;
    [SerializeField, Tooltip("キャラクター子オブジェクトの魔法コライダーを登録します。")]
    private Collider magic;

    public Transform _Transform { get { return _transform; } }
    public Rigidbody _Rigidbody { get { return _rigidbody; } set { _rigidbody = value; } }
    public Collider Sound { get { return sound; } set { sound = value; } }
    public Collider Magic { get { return magic; } set { magic = value; } }


    // *** 標的のコンポーネントを保有する変数 ***
    protected Character target;
    protected Transform target_transform;
    /// <summary>
    /// 自分→標的の方向ベクトル
    /// </summary>
    public Vector3 Focus { get; set; }


    // *** キャラクターのコンポーネント以外の情報を保有する変数 ***
    /// <summary>
    /// キャラクターの移動方向ベクトル
    /// </summary>
    public Vector3 Velocity { protected get; set; }
    /// <summary>
    /// キャラクターの回転方向
    /// </summary>
    public Quaternion Character_rot { protected get; set; }
    /// <summary>
    /// アクション一つひとつに掛かる時間
    /// </summary>
    public float Action_time { get; set; }
    /// <summary>
    /// 回転スピード
    /// </summary>
    protected float turn_speed = 360;
    /// <summary>
    /// スキルのチャージ状態を保有
    /// </summary>
    public bool isCharge { get; set; } = false;

    [SerializeField, Tooltip("ステータス情報")]
    protected CharacterStatus status;
    public CharacterStatus Status { get { return status; } }

    protected virtual void Start()
    {
        SetCharacter();
        Character_rot = _transform.rotation;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(Velocity.x, _rigidbody.velocity.y, Velocity.z);
    }

    /// <summary>
    /// キャラクターのデータをセットします。
    /// </summary>
    public void SetCharacter()
    {
        SpawnStatus();
        SetActiveSkill();
        SetPassiveSkill();
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
    /// ダメージ計算(弱体あり)
    /// </summary>
    public void DamageCalculation(int enemy_attack, int weak)
    {
        int def = status.Def;
        status.Def /= weak;
        DamageCalculation(enemy_attack);
        status.Def = def;
    }

    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    private void CharacterDamaged(int damage)
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

    // *** 位置関係取得 メソッド ******************
    /// <summary>
    /// 標的の座標を返します。標的を取っていない場合は自身のTransform.forward * rangeを返します。
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetPosition(float range)
    {
        if (target_transform = null)
        {
            return _transform.forward * range * 0.99f;
        }
        return target_transform.position;
    }

    /// <summary>
    /// 自身→標的の方向ベクトル"focus"を設定します。標的がいない場合は自分の正面をセットします。
    /// </summary>
    /// 
    protected void FocusTarget()
    {
        if (target_transform != null)
        {
            Focus = target_transform.position - _transform.position;
        }
        else
        {
            Focus = _transform.forward;
        }
    }

    // *** abstract メソッド ******************
    /// <summary>
    /// キャラクターにアクティブスキルの登録を行います。
    /// </summary>
    abstract protected void SetActiveSkill();

    /// <summary>
    /// キャラクターにパッシブスキルの登録と発動を行います。
    /// </summary>
    abstract protected void SetPassiveSkill();

    /// <summary>
    /// キャラクターに追加でスキルを登録します。
    /// </summary>
    abstract public void GetNewSkill(GameObject skill);

    /// <summary>
    /// 敵を発見した場合の処理
    /// </summary>
    abstract public void FoundEnemy(GameObject target);

    /// <summary>
    /// 敵を見失った場合の処理
    /// </summary>
    abstract protected void LoseSightEnemy();

    /// <summary>
    /// 敵を倒したときの処理
    /// </summary>
    abstract public void Beat(int exp);

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();

    /// <summary>
    /// 出現時の能力を設定します。
    /// </summary>
    private void SpawnStatus()
    {
        status.Level = status.Basic.Level;
        status.FloatMaxHp = status.Basic.MaxHp;
        status.FloatMaxSp = status.Basic.MaxSp;
        status.FloatAtk = status.Basic.Atk;
        status.FloatExp = status.Basic.Exp;
        SetStatus();
    }

    public void SetStatus()
    {
        status.MaxHp = (int)status.FloatMaxHp + status.AddHp;
        status.Hp = status.MaxHp;
        status.MaxSp = (int)status.FloatMaxSp + status.AddSp;
        status.Sp = status.MaxSp;
        status.Atk = (int)status.FloatAtk + status.AddAtk;
        status.Def = status.Basic.Def + status.AddDef;
        status.Speed = status.Basic.Speed + status.AddSpeed;
        status.Exp = (int)status.FloatExp;
    }

    public void LevelUp(int n)
    {
        for(int i = 0; i < n; i++)
        {
            status.Level++;
            status.FloatMaxHp *= ((status.GrowMaxHp + 100) / 100);
            status.FloatMaxSp *= ((status.GrowMaxSp + 100) / 100);
            status.FloatAtk *= ((status.GrowAtk + 100) / 100);
            status.FloatExp *= ((status.GrowExp + 100) / 100);
        }
        SetStatus();
    }

    public void LevelDown(int n)
    {
        for(int i = 0; i < n; i++)
        {
            if (status.Level > 1)
            {
                status.Level--;
                status.FloatMaxHp /= ((status.GrowMaxHp + 100) / 100);
                status.FloatMaxSp /= ((status.GrowMaxSp + 100) / 100);
                status.FloatAtk /= ((status.GrowAtk + 100) / 100);
                status.FloatExp /= ((status.GrowExp + 100) / 100);
            }
        }
        SetStatus();
    }
}