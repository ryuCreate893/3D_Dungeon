using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Tooltip("ダンジョンの情報を作成します。")]
[CreateAssetMenu(menuName = "Create Dungeon")]
class DungeonData : ScriptableObject
{
    [Header("ダンジョン情報")]
    [SerializeField, Tooltip("ダンジョン名")]
    private string dungeon_name;
    [SerializeField, Tooltip("ダンジョンの深さと階層ごとの情報")]
    private DepthData[] depth;
    [SerializeField, Tooltip("ダンジョンの最下層シーン名")]
    private string last_floor;
    [SerializeField, Tooltip("ダンジョン専用のStairオブジェクトを登録")]
    private GameObject stair;

    public string Dungeon_name { get { return dungeon_name; } }
    public DepthData[] Depth { get { return depth; } }
    public string Last_floor { get { return last_floor; } }
    public GameObject Stair { get { return stair; } }
}

[System.Serializable, Tooltip("各階層の出現データを設定")]
public class DepthData
{
    [SerializeField, Tooltip("ランダムで遷移させるシーン名を設定")]
    private string[] pattern;
    [SerializeField, Tooltip("出現するアイテムの種類を設定")]
    private SpawnData[] item;
    [SerializeField, Tooltip("出現するトラップの種類を設定")]
    private SpawnData[] trap;
    [SerializeField, Tooltip("出現する敵の種類を設定")]
    private SpawnData[] enemy;

    [SerializeField, Tooltip("落ちているアイテムの数を設定")]
    private int onItems;
    [SerializeField, Tooltip("設置されているトラップの数を設定")]
    private int onTraps;
    [SerializeField, Tooltip("棲息している(初期の)敵の数を設定")]
    private int onEnemies;
    [SerializeField, Tooltip("敵の最大棲息数を設定")]
    private int max_enemy;
    [SerializeField, Tooltip("敵出現時のレベル最低値を設定('-5' から '0')")]
    [Range(-5, 0)]
    private int min_spawn_level;
    [SerializeField, Tooltip("敵出現時のレベル最高値を設定('0' から '5')")]
    [Range(0, 5)]
    private int max_spawn_level;
    [SerializeField, Tooltip("敵の出現間隔を設定(出現しない場合は'-1'を記入")]
    private int spawn_time;
    [SerializeField, Tooltip("制限時間を設定(制限がない場合は'-1'を記入")]
    private int limit_time;

    public string[] Pattern { get { return pattern; } }
    public SpawnData[] Item { get { return item; } }
    public SpawnData[] Trap { get { return trap; } }
    public SpawnData[] Enemy { get { return enemy; } }
    public int OnItems { get { return onItems; } }
    public int OnTraps { get { return onTraps; } }
    public int OnEnemies { get { return onEnemies; } }
    public int Max_enemy { get { return max_enemy; } }
    public int Min_spawn_level { get { return min_spawn_level; } }
    public int Max_spawn_level { get { return max_spawn_level; } }
    public float Spawn_time { get { return spawn_time; } }
    public float Limit_time { get { return limit_time; } }
}

[System.Serializable, Tooltip("各出現情報を設定")]
public class SpawnData
{
    [SerializeField, Tooltip("出現するオブジェクトの種類")]
    private GameObject kind;
    [SerializeField, Tooltip("オブジェクトが出現する確率")]
    [Range(0, 100)]
    private int probability;

    public GameObject Kind { get { return kind; } }
    public int Probability { get { return probability; } }
}