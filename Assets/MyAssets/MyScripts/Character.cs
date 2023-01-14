using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Character : MonoBehaviour
{
    private void Awake() // テスト用
    {
        SetCharacter(2); 
    }

    public virtual void SetCharacter(int n) // "n"は初期スポーン時の基礎レベルとの差です。(基礎レベル50, n = 2のときレベル52でスポーン)
    {
        // *** ステータスのセット ***
        status.Current.Level = status.Basic.Level + n;
        SetBaseStatus();
        if (n < 0)
        {
            n *= -1;
            for (int i = 0; i < n; i++)
            {
                LevelDown();
            }
        }
        else if(n > 0)
        {
            for(int i = 0;i < n; i++)
            {
                LevelUp();   
            }
        }
        SetCurrentStatus();
        Debug.Log(status.Current.MaxHp);

        // *** アクティブスキルのセット ***
        for (int i = 0; i < activeSkill.Count; i++)
        {
            activeSkill[i].UserSpSet(gameObject);
        }

        // *** パッシブスキルの発動 ***
        for (int i = 0; i < passiveSkill.Count; i++)
        {
            
        }
    }

    /// <summary>
    /// ベースとなるステータスをセットします。スポーン時の1回のみ起動します。
    /// </summary>
    private void SetBaseStatus()
    {
        status.Float.MaxHp       = status.Basic.MaxHp;
        status.Float.MaxSp       = status.Basic.MaxSp;
        status.Float.Atk         = status.Basic.Atk;
        status.Float.Def_percent = status.Basic.Def_percent;
        status.Float.Speed       = status.Basic.Speed;
        status.Float.Exp         = status.Basic.Exp;
    }

    /// <summary>
    /// レベルが上昇したときの処理を行います。
    /// </summary>
    private void LevelUp()
    {
        Debug.Log(status.Growth.MaxHp);
        status.Float.MaxHp       *= status.Growth.MaxHp;
        status.Float.MaxSp       *= status.Growth.MaxSp;
        status.Float.Atk         *= status.Growth.Atk;
        status.Float.Def_percent *= status.Growth.Def_percent;
        status.Float.Speed       *= status.Growth.Speed;
        status.Float.Exp         *= status.Growth.Exp;
    }

    /// <summary>
    /// レベルが下落したときの処理を行います。
    /// </summary>
    private void LevelDown()
    {
        status.Float.MaxHp       /= status.Growth.MaxHp;
        status.Float.MaxSp       /= status.Growth.MaxSp;
        status.Float.Atk         /= status.Growth.Atk;
        status.Float.Def_percent /= status.Growth.Def_percent;
        status.Float.Speed       /= status.Growth.Speed;
        status.Float.Exp         /= status.Growth.Exp;
    }

    private void SetCurrentStatus()
    {

        status.Current.MaxHp       = (int)status.Float.MaxHp;
        status.Current.Hp          = status.Current.MaxHp;
        status.Current.MaxSp       = (int)status.Float.MaxSp;
        status.Current.Sp          = status.Current.MaxSp;
        status.Current.Atk         = (int)status.Float.Atk;
        status.Current.Def_percent = (int)status.Float.Def_percent;
        status.Current.Speed       = (int)status.Float.Speed;
        status.Current.Exp         = (int)status.Float.Exp;
    }

    /// ダメージを受けた場合の処理
    protected virtual void Damaged() { }

    /// 敵を発見した場合の処理
    protected virtual void FoundEnemy() { }

    /// 敵を見失った場合の処理
    protected virtual void LoseSightEnemy() { }



    /// HPが0以下になった場合の処理
    protected virtual void DeathCharacter() { }

    /// <summary>
    /// キャラクターの能力を設定
    /// </summary>
    public Status status;

    [Tooltip("フィールド上で使うスキルです。\n使用優先度が高い順に並べていきます。")]
    [SerializeField]
    protected List<ActiveSkill> activeSkill;

    [Tooltip("常時発動している固有能力です。\n優先度などで並べる必要はありません。")]
    [SerializeField]
    protected List<PassiveSkill> passiveSkill;
}