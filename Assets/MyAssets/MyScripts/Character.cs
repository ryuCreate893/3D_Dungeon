using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Character : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Transform _transform;
    protected Animator _animator;
    protected Quaternion _characterRotation;
    protected Vector3 _velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _characterRotation = transform.rotation;
        SpawnCharacter(2); // テスト用(本来は外部から呼び出す)
    }


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
    public bool TimeCheck()
    {
        return _actionTime <= 0;
    }


    /// <summary>
    /// キャラクターをスポーンさせます。 "n"の分だけレベルを上下させることができます。
    /// </summary>
    /// <param name="n"></param>
    public void SpawnCharacter(int n)
    {
        SetStartStatus(n);
        ActiveSkillSet();
        PassiveSkillSet();
    }

    protected void ActiveSkillSet()
    {
        for (int i = 0; i < activeSkill.Count; i++)
        {
            passiveSkill[i].UserSet(gameObject);
            if (_maxSkillRange < activeSkill[i].GetRange())
            {
                _maxSkillRange = activeSkill[i].GetRange(); // 最長射程のスキルを登録
            }
        }
    }

    protected void PassiveSkillSet()
    {
        for (int i = 0; i < passiveSkill.Count; i++)
        {
            passiveSkill[i].UserSet(gameObject);
            passiveSkill[i].TrySkill(); // パッシブスキルは常時発動
        }
    }

    /// <summary>
    /// 初期ステータスをセットします。スポーン時の1回だけ起動します。
    /// </summary>
    private void SetStartStatus(int n)
    {
        _float.MaxHp = _status.Basic.MaxHp;
        _float.MaxSp = _status.Basic.MaxSp;
        _float.Atk = _status.Basic.Atk;
        _float.Def = _status.Basic.Def;
        _float.Speed = _status.Basic.Speed;
        _current.Level = _status.Basic.Level + n;
        if (n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                LevelUp();
            }
        }
        else if (n < 0)
        {
            for (int i = 0; i < -n; i++)
            {
                LevelDown();
            }
        }
        SetCurrentStatus();
        Debug.Log("ステータスセット完了！ キャラクター名 : " + gameObject.name);
    }


    /// <summary>
    /// レベルが上昇したときの処理を行います。
    /// </summary>
    private void LevelUp()
    {
        _float.MaxHp *= _grow.MaxHp;
        _float.MaxSp *= _grow.MaxSp;
        _float.Atk *= _grow.Atk;
        _float.Def *= _grow.Def_percent;
        _float.Speed *= _grow.Speed;
    }


    /// <summary>
    /// レベルが下落したときの処理を行います。
    /// </summary>
    private void LevelDown()
    {
        _float.MaxHp /= _grow.MaxHp;
        _float.MaxSp /= _grow.MaxSp;
        _float.Atk /= _grow.Atk;
        _float.Def /= _grow.Def_percent;
        _float.Speed /= _grow.Speed;
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


    /// <summary>
    /// ダメージを受けた場合の処理(弱体なし)
    /// </summary>
    public void Damaged(int enemy_attack)
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
            int per = 100 - _current.Def;
            damage = (int)(enemy_attack * per / 100);

            _current.Hp -= damage;
            if (_current.Hp <= 0)
            {
                _current.Hp = 0;
                DeathCharacter();
            }
        }
    }


    /// <summary>
    /// ダメージを受けた場合の処理(弱体あり)
    /// </summary>
    public void Damaged(int enemy_attack, int weak)
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
            int per = 100 - weaked;
            damage = (int)(enemy_attack * per / 100);

            _current.Hp -= damage;
            if (_current.Hp <= 0)
            {
                _current.Hp = 0;
                DeathCharacter();
            }
        }
    }

    /// <summary>
    /// 敵を発見した場合の処理
    /// </summary>
    abstract public void FoundEnemy();

    /// <summary>
    /// 敵を見失った場合の処理
    /// </summary>
    abstract public void LoseSightEnemy();

    /// <summary>
    /// 敵を倒したときの処理
    /// </summary>
    abstract public void Beat();

    /// <summary>
    /// HPが0以下になった場合の処理
    /// </summary>
    abstract protected void DeathCharacter();


    [Tooltip("キャラクターの基礎ステータス・知覚情報を設定します。")]
    [SerializeField]
    private Status _status;

    [Tooltip("キャラクターの成長度を設定します。")]
    [SerializeField]
    private GrowthValue _grow;

    [Tooltip("キャラクターの内部ステータスを設定します。(小数点以下まで保有)")]
    [SerializeField]
    private FloatStatus _float;

    [Tooltip("キャラクターの実際のステータスを設定します。(整数で表示)")]
    [SerializeField]
    public CurrentStatus _current;

    [Tooltip("フィールド上で使うスキルです。\n使用優先度が高い順に並べていきます。")]
    [SerializeField]
    protected List<ActiveSkill> activeSkill;

    [Tooltip("常時発動している固有能力です。\n優先度などで並べる必要はありません。")]
    [SerializeField]
    protected List<PassiveSkill> passiveSkill;

    /// <summary>
    /// 標的とするオブジェクトを設定します。
    /// キャラクターは標的としたオブジェクトの方を向くようにします。
    /// </summary>
    protected GameObject _target;

    /// <summary>
    /// スキルチャージ、発動、ダッシュなど、アクションに掛かる時間です。
    /// </summary>
    protected float _actionTime;

    /// <summary>
    /// 全スキルの中での最長射程距離です。
    /// </summary>
    protected float _maxSkillRange = 0;
}