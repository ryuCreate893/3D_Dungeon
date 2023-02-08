using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{
    // *** 外部設定 ***
    [Header("ステータス情報")]
    [SerializeField, Tooltip("キャラクターの基礎ステータス・知覚情報を設定")]
    private Status _status;
    [SerializeField, Tooltip("キャラクターの成長度を設定")]
    private GrowthValue _grow;
    [SerializeField, Tooltip("キャラクターの内部ステータス(float)を設定")]
    private FloatStatus _float;
    [SerializeField, Tooltip("キャラクターの実際のステータス(int)を設定")]
    public CurrentStatus _current;

    // *** キャラクターのコンポーネントを保有する変数 ***
    /// <summary>
    /// キャラクターのTransformコンポーネントを保有
    /// </summary>
    protected Transform _transform;
    /// <summary>
    /// キャラクターのRigidbodyコンポーネントを保有
    /// </summary>
    protected Rigidbody _rigidbody;
    /// <summary>
    /// キャラクターのAnimatorコンポーネントを保有
    /// </summary>
    protected Animator _animator;

    // *** キャラクターのコンポーネント以外の情報を保有する変数 ***
    /// <summary>
    /// キャラクターの回転方向を保有
    /// </summary>
    protected Quaternion _characterRotation;
    /// <summary>
    /// キャラクターの移動方向のベクトル
    /// </summary>
    protected Vector3 _velocity;
    /// <summary>
    /// アクション一つひとつに掛かる時間(外部からメソッドで変更可能)
    /// </summary>
    protected float _actionTime;
    /// <summary>
    /// 保有するスキルの最長射程(外部からメソッドで変更可能)
    /// </summary>
    protected float _maxSkillRange = 0;
    /// <summary>
    /// 現在チャージしているスキルの番号(外部からメソッドで変更可能)
    /// </summary>
    protected int _chargeSkill = -1;


    // *** 標的のコンポーネントを保有する変数 ***
    /// <summary>
    /// 標的のメソッド
    /// </summary>
    protected Character _target;
    /// <summary>
    /// 標的の座標
    /// </summary>
    protected Transform _targetTransform;
    /// <summary>
    /// 本体→標的の方向ベクトル
    /// </summary>
    protected Vector3 _focus;

    protected virtual void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterRotation = _transform.rotation;
        SpawnCharacter(2); // テスト用(本来は外部から呼び出す)
    }

    private void FixedUpdate()
    {
        _velocity += new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.velocity = _velocity;
    }

    // *** ステータス関連 メソッド ******************
    /// <summary>
    /// キャラクターをスポーンさせます。 "n"の分だけレベルを上下させることができます。
    /// </summary>
    /// <param name="n"></param>
    public void SpawnCharacter(int n)
    {
        // スポーン時の初期ステータスを設定します。
        _float.MaxHp = _status.Basic.MaxHp;
        _float.MaxSp = _status.Basic.MaxSp;
        _float.Atk = _status.Basic.Atk;
        _float.Def = _status.Basic.Def;
        _float.Speed = _status.Basic.Speed;
        _current.Level = _status.Basic.Level;

        if (n > 0)
        {
            LevelUp(n);
        }
        else if (n < 0)
        {
            LevelDown(n);
        }
        else
        {
            SetCurrentStatus();
        }

        SetActiveSkill();
        SetPassiveSkill();
        Debug.Log("キャラクターセット完了！ キャラクター名 : " + gameObject.name);
    }

    /// <summary>
    /// レベルが上昇したときの処理を行います。
    /// </summary>
    protected void LevelUp(int n)
    {
        for (int i = 0; i < n; i++)
        {
            _current.Level++;
            _float.MaxHp *= _grow.MaxHp;
            _float.MaxSp *= _grow.MaxSp;
            _float.Atk *= _grow.Atk;
            _float.Def *= _grow.Def_percent;
            _float.Speed *= _grow.Speed;
            Debug.Log(gameObject.name + "のレベルが上がった！");
        }
        SetCurrentStatus();
    }

    /// <summary>
    /// レベルが下落したときの処理を行います。
    /// </summary>
    protected void LevelDown(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (_current.Level == 1)
            {
                Debug.Log("しかし" + gameObject.name + "のレベルはもう下がらなかった！");
                break;
            }
            else
            {
                _current.Level--;
                _float.MaxHp /= _grow.MaxHp;
                _float.MaxSp /= _grow.MaxSp;
                _float.Atk /= _grow.Atk;
                _float.Def /= _grow.Def_percent;
                _float.Speed /= _grow.Speed;
                Debug.Log(gameObject.name + "のレベルが下がった！");
            }
        }
        SetCurrentStatus();
    }

    /// <summary>
    /// 実際に表示されるステータスをセットします。
    /// </summary>
    private void SetCurrentStatus()
    {
        _current.MaxHp = (int)_float.MaxHp;
        _current.Hp = _current.MaxHp;
        _current.MaxSp = (int)_float.MaxSp;
        _current.Sp = _current.MaxSp;
        _current.Atk = (int)_float.Atk;
        _current.Def = (int)_float.Def;
        _current.Speed = (int)_float.Speed;
        _current.Exp = _status.Basic.Exp;
    }

    // *** バトル関連 メソッド ******************
    /// <summary>
    /// ダメージ計算
    /// </summary>
    public void DamageCalculation(int enemy_attack)
    {
        int damage;
        if (_current.Def == 0)
        {
            damage = enemy_attack;
        }
        else if (_current.Def >= 100)
        {
            damage = 0;
        }
        else
        {
            damage = enemy_attack * (100 - _current.Def) / 100;
        }
        if (damage > 0) Damaged(damage);
    }

    /// <summary>
    /// ダメージ計算(弱体あり)
    /// </summary>
    public void DamageCalculation(int enemy_attack, int weak)
    {
        _float.Def /= weak;
        _current.Def = (int)_float.Def;
        DamageCalculation(enemy_attack);
        _float.Def *= weak;
        _current.Def = (int)_float.Def;
    }

    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    private void Damaged(int damage)
    {
        _current.Hp -= damage;
        if (_current.Hp <= 0)
        {
            _current.Hp = 0;
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

    // *** 変数操作 メソッド ******************
    /// <summary>
    /// rigidbody.velocityを取得します。
    /// </summary>
    public Vector3 GetRigidbody()
    {
        return _rigidbody.velocity;
    }
    /// <summary>
    /// rigidbody.velocityを調整します。
    /// </summary>
    public void SetRigidbody(Vector3 v3)
    {
        _rigidbody.velocity = v3;
    }

    /// <summary>
    /// rigidbodyにAddForceを加えます。
    /// </summary>
    public void AddForceRigidbody(Vector3 v3)
    {
        _rigidbody.AddForce(v3, ForceMode.Impulse);
    }

    /// <summary>
    /// アクション時間をセットし、アクション時間が残っている間は行動を制限させます。
    /// </summary>
    public void SetActionTime(float time)
    {
        _actionTime = time;
    }

    /// <summary>
    /// 現在チャージしているスキルをセットします。
    /// </summary>
    public void SetChargeSkill(int n)
    {
        _chargeSkill = n;
    }

    /// <summary>
    /// 移動方向と速度をセットします。
    /// </summary>
    public void SetVelocity(Vector3 v3)
    {
        _velocity = v3;
    }

    /// <summary>
    /// 移動方向と速度をセットします。
    /// </summary>
    public void SetRotation(Quaternion quaternion)
    {
        _characterRotation = quaternion;
    }

    /// <summary>
    /// スキルの最長射程を登録します。
    /// </summary>
    public void SetMaxRange(float range)
    {
        _maxSkillRange = Mathf.Max(_maxSkillRange, range);
    }

    // *** 位置関係取得 メソッド ******************
    /// <summary>
    /// 自身のTransformを返します。
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMyPosition()
    {
        return _transform.position;
    }

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
    /// 自身→標的の方向ベクトルを設定します。標的がいない場合は自分の正面をセットします。
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
        return (_targetTransform.position - transform.position).magnitude <= range;
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
    abstract public void Beat();

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();

    abstract protected void SetCharacterAngle();
}