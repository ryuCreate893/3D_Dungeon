using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{
    // *** 外部設定 ***
    [SerializeField, Tooltip("キャラクターの基礎ステータス・知覚情報を設定します。")]
    private Status _status;
    [SerializeField, Tooltip("キャラクターの成長度を設定します。")]
    private GrowthValue _grow;
    [SerializeField, Tooltip("キャラクターの内部ステータス(float)を設定します。")]
    private FloatStatus _float;
    [SerializeField, Tooltip("キャラクターの実際のステータス(int)を設定します。")]
    public CurrentStatus _current;
    [SerializeField, Tooltip("フィールド上で使うスキルです。\n使用優先度が高い順に並べていきます。")]
    protected List<ActiveSkill> _activeSkill;
    [SerializeField, Tooltip("常時発動している固有能力です。\n優先度などで並べる必要はありません。")]
    protected List<PassiveSkill> _passiveSkill;

    // *** 内部設定 ***
    /// <summary>
    /// キャラクターの位置・向きを保有
    /// </summary>
    protected Transform _transform;
    /// <summary>
    /// キャラクターの速度・物理演算を保有
    /// </summary>
    protected Rigidbody _rigidbody;
    /// <summary>
    /// キャラクターのアニメーションを保有
    /// </summary>
    protected Animator _animator;
    /// <summary>
    /// キャラクターの移動速度・方向(_rigidbodyに加えて移動させる)
    /// </summary>
    protected Vector3 _velocity;
    /// <summary>
    /// キャラクターの回転方向
    /// </summary>
    protected Quaternion _characterRotation;
    /// <summary>
    /// 現在チャージしているスキルの番号
    /// </summary>
    protected int _chargeSkillnumber = -1;
    /// <summary>
    /// 標的の座標
    /// </summary>
    protected Transform _targetTransform;
    /// <summary>
    /// 標的のメソッド
    /// </summary>
    protected Character _targetMethod;
    /// <summary>
    /// 標的との距離
    /// </summary>
    protected float _targetDistance;
    /// <summary>
    /// アクション一つひとつに掛かる時間
    /// </summary>
    protected float _actionTime;
    /// <summary>
    /// キャラクターが保有するスキルの最長射程距離
    /// </summary>
    protected float _maxSkillRange = 0;


    private void Awake()
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
        _current.Level = _status.Basic.Level + n;

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

        // アクティブスキルをキャラクターに覚えさせます。
        for (int i = 0; i < _activeSkill.Count; i++)
        {
            _activeSkill[i].UserSet(gameObject, i);
            if (_maxSkillRange < _activeSkill[i].GetRange())
            {
                _maxSkillRange = _activeSkill[i].GetRange(); // 最長射程のスキルを登録
            }
        }

        // パッシブスキルをキャラクターに覚えさせて、発動させます。
        for (int i = 0; i < _passiveSkill.Count; i++)
        {
            _passiveSkill[i].UserSet(gameObject, i);
            _passiveSkill[i].SkillContent();
        }

        Debug.Log("キャラクターセット完了！ キャラクター名 : " + gameObject.name);
    }

    /// <summary>
    /// レベルが上昇したときの処理を行います。
    /// </summary>
    private void LevelUp(int n)
    {
        for (int i = 0; i < n; i++)
        {
            _float.MaxHp *= _grow.MaxHp;
            _float.MaxSp *= _grow.MaxSp;
            _float.Atk *= _grow.Atk;
            _float.Def *= _grow.Def_percent;
            _float.Speed *= _grow.Speed;
        }
        SetCurrentStatus();
    }

    /// <summary>
    /// レベルが下落したときの処理を行います。
    /// </summary>
    private void LevelDown(int n)
    {
        for (int i = 0; i < n; i++)
        {
            _float.MaxHp /= _grow.MaxHp;
            _float.MaxSp /= _grow.MaxSp;
            _float.Atk /= _grow.Atk;
            _float.Def /= _grow.Def_percent;
            _float.Speed /= _grow.Speed;
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

    public void GetActiveSkill(GameObject skill)
    {
        _activeSkill.Add(skill.GetComponent<ActiveSkill>());
    }

    public void GetPassiveSkill(GameObject skill)
    {
        _passiveSkill.Add(skill.GetComponent<PassiveSkill>());
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
    /// (弱体あり)ダメージ計算
    /// </summary>
    public void DamageCalculation(int enemy_attack, int weak)
    {
        int weaked = (int)(_current.Def / weak);
        int damage;
        if (weaked == 0)
        {
            damage = enemy_attack;
        }
        else if (weaked >= 100)
        {
            damage = 0;
        }
        else
        {
            damage = enemy_attack * (100 - weaked) / 100;
        }

        if (damage > 0) Damaged(damage);
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
        else if (_chargeSkillnumber != -1)
        {
            // 攻撃を受けたらキャンセルされるスキルをチャージしていた場合
            _activeSkill[_chargeSkillnumber].DamagedCancel();
            _chargeSkillnumber = -1;
        }
    }

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();

    /// <summary>
    /// 敵を倒したときの処理
    /// </summary>
    abstract public void Beat();


    // *** アクション関連 メソッド ******************
    /// <summary>
    /// 外部からアクション時間をセットし、アクション時間が残っている間は行動を制限させます。
    /// </summary>
    public void ActionTimeSet(float time)
    {
        _actionTime = time;
    }

    /// <summary>
    /// アクションに掛かる時間を管理します。'0'の場合trueを返します。
    /// </summary>
    public bool TimeCheck(float time)
    {
        return time <= 0;
    }

    /// <summary>
    /// 敵を発見した場合の処理
    /// </summary>
    abstract public void FoundEnemy(GameObject target);

    /// <summary>
    /// 敵を見失った場合の処理
    /// </summary>
    abstract protected void LoseSightEnemy();


    // *** 位置関係取得 メソッド ******************
    /// <summary>
    /// 自身→標的の単位ベクトルを返します。標的がいない場合は零ベクトルを返します。
    /// </summary>
    public Vector3 FocusTarget()
    {
        if (_targetTransform != null)
        {
            Vector3 v3 = _targetTransform.position - transform.position;
            _targetDistance = v3.magnitude;
            return v3;
        }
        else
        {
            _targetDistance = 0;
            return Vector3.zero;
        }
    }

    /// <summary>
    /// 向かせたい方向を引数に指定し、その方向に回転させます。
    /// </summary>
    /// <returns></returns>
    protected abstract void SetCharacterAngle(Vector3 v3look);

    /// <summary>
    /// 標的との距離を計算して、引数に指定した距離の方が長い場合はtrueを返します。
    /// </summary>
    public bool CheckTargetDistence(float range)
    {
        return (_targetTransform.position - transform.position).magnitude <= range;
    }
}