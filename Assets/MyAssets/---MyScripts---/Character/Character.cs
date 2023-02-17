using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{
    // *** 自分自身のコンポーネントを保有する変数 ***
    [SerializeField, Tooltip("キャラクター本体を登録します。")]
    protected Transform _transform;
    [SerializeField, Tooltip("キャラクター本体を登録します。")]
    private Rigidbody _rigidbody;
    [SerializeField, Tooltip("キャラクター本体を登録します。")]
    protected Animator _animator;
    [SerializeField, Tooltip("キャラクター子オブジェクトの音コライダーを登録します。")]
    private Collider _sound;
    [SerializeField, Tooltip("キャラクター子オブジェクトの魔法コライダーを登録します。")]
    private Collider _magic;

    public Transform _Transform { get { return _transform; } }
    public Rigidbody _Rigidbody { get { return _rigidbody; } set { _rigidbody = value; } }
    public Collider _Sound { get { return _sound; } set { _sound = value; } }
    public Collider _Magic { get { return _magic; } set { _magic = value; } }


    // *** 標的のコンポーネントを保有する変数 ***
    protected Character _target;
    protected Transform _targetTransform;
    /// <summary>
    /// 自分→標的の方向ベクトル
    /// </summary>
    public Vector3 _focus { get; set; }


    // *** キャラクターのコンポーネント以外の情報を保有する変数 ***
    /// <summary>
    /// キャラクターの移動方向ベクトル
    /// </summary>
    public Vector3 _velocity { protected get; set; }
    /// <summary>
    /// キャラクターの回転方向
    /// </summary>
    public Quaternion _characterRotation { protected get; set; }
    /// <summary>
    /// アクション一つひとつに掛かる時間
    /// </summary>
    public float _actionTime { get; set; }
    /// <summary>
    /// 現在チャージしているスキルの番号
    /// </summary>
    public int _chargeSkill { protected get; set; } = -1;
    /// <summary>
    /// 回転スピード
    /// </summary>
    protected float _turnSpeed = 360;

    [SerializeField, Tooltip("ステータス情報")]
    protected CharacterStatus _status;
    public CharacterStatus _Status { get { return _status; } }


    protected virtual void Awake()
    {
        _characterRotation = _transform.rotation;
        SpawnCharacter(2); // テスト用(本来は外部から呼び出す)
    }


    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_velocity.x, _rigidbody.velocity.y, _velocity.z);
    }

    /// <summary>
    /// キャラクターをスポーンさせます。 "n"の分だけレベルを上下させます。
    /// </summary>
    /// <param name="n"></param>
    public void SpawnCharacter(int n)
    {
        _status.SpawnStatus(n);
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
        if (_status.Def == 0)
        {
            damage = enemy_attack;
        }
        else if (_status.Def >= 100)
        {
            damage = 0;
        }
        else
        {
            damage = enemy_attack * (100 - _status.Def) / 100;
        }
        if (damage > 0) Damaged(damage);
    }

    /// <summary>
    /// ダメージ計算(弱体あり)
    /// </summary>
    public void DamageCalculation(int enemy_attack, int weak)
    {
        int def = _status.Def;
        _status.Def /= weak;
        DamageCalculation(enemy_attack);
        _status.Def = def;
    }

    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    private void Damaged(int damage)
    {
        _status.Hp -= damage;
        if (_status.Hp <= 0)
        {
            _status.Hp = 0;
            DeathCharacter();
        }
        else if (_chargeSkill != -1)
        {
            DamagedCancel(_chargeSkill);
        }
    }

    /// <summary>
    /// チャージスキルのキャンセル処理を行います。
    /// </summary>
    abstract protected void DamagedCancel(int n);


    // *** 位置関係取得 メソッド ******************
    /// <summary>
    /// 標的の座標を返します。標的を取っていない場合は自身のTransform.forward * rangeを返します。
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetPosition(float range)
    {
        if (_targetTransform = null)
        {
            return _transform.forward * range * 0.99f;
        }
        return _targetTransform.position;
    }

    /// <summary>
    /// 自身→標的の方向ベクトル"_focus"を設定します。標的がいない場合は自分の正面をセットします。
    /// </summary>
    /// 
    protected void FocusTarget()
    {
        if (_targetTransform != null)
        {
            _focus = _targetTransform.position - _transform.position;
        }
        else
        {
            _focus = _transform.forward;
        }
    }

    /// <summary>
    /// 標的との距離を計算して、引数に指定した距離の方が長い場合はtrueを返します。
    /// </summary>
    public bool CheckTargetDistence(float range)
    {
        return _focus.magnitude <= range;
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
    abstract public void Beat(GameObject target);

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();
}