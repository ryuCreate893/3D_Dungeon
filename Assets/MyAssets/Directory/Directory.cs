using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directory : MonoBehaviour
{
    /// <summary>
    /// 敵の名鑑です。
    /// </summary>
    public Directory_Enemy[] _enemy;

    /// <summary>
    /// アイテムの名鑑です。
    /// </summary>
    public Directory_Item[] _item;

    /// <summary>
    /// スキルの名鑑です。
    /// </summary>
    public Directory_Skill[] _skill;
}

[Tooltip("敵の名鑑の内容を設定します。")]
[System.Serializable]
public class Directory_Enemy
{
    [SerializeField]
    [Tooltip("敵の名鑑番号です。")]
    private int _number;
    [SerializeField]
    [Tooltip("敵の名前です。")]
    private string _name;
    [SerializeField]
    [Tooltip("敵の外見です。")]
    private Sprite _sprite;
    [SerializeField]
    [Tooltip("敵の基礎能力です。")]
    private Status _status;
    [SerializeField]
    [Tooltip("敵の説明です。")]
    private string _information;
    [SerializeField]
    [Tooltip("遭遇済みかどうかをチェックします。")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public Sprite _Sprite { get { return _sprite; } }
    public Status _Status { get { return _status; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}

[Tooltip("アイテムの名鑑の内容を設定します。")]
[System.Serializable]
public class Directory_Item
{
    [SerializeField]
    [Tooltip("アイテムの名鑑番号です。")]
    private int _number;
    [SerializeField]
    [Tooltip("アイテムの名前です。")]
    private string _name;
    [SerializeField]
    [Tooltip("アイテムの外見です。")]
    private Sprite _sprite;
    [SerializeField]
    [Tooltip("アイテムの説明です。")]
    private string _information;
    [SerializeField]
    [Tooltip("発見済みかどうかをチェックします。")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public Sprite _Sprite { get { return _sprite; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}

[Tooltip("スキルの名鑑の内容を設定します。")]
[System.Serializable]
public class Directory_Skill
{
    private enum type
    {
        active,
        passive
    }

    [SerializeField]
    [Tooltip("スキルの名鑑番号です。")]
    private int _number;
    [SerializeField]
    [Tooltip("スキルの名前です。")]
    private string _name;
    [SerializeField]
    [Tooltip("スキルの種類です。(0 = アクティブスキル, 1 = パッシブスキル)")]
    private type _type;
    [SerializeField]
    [Tooltip("スキルの説明です。")]
    private string _information;
    [SerializeField]
    [Tooltip("発見済みかどうかをチェックします。")]
    private bool _discover;

    public int _Number { get { return _number; } }
    public string _Name { get { return _name; } }
    public int _Type { get { return (int)_type; } }
    public string _Information { get { return _information; } }
    public bool _Discover { get { return _discover; } set { _discover = value; } }
}
