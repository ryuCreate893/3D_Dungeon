using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("ダンジョンの情報を作成します。")]
[CreateAssetMenu(menuName = "Create Dungeon")]
class DungeonData : ScriptableObject
{
    [Header("ダンジョン情報")]
    [SerializeField, Tooltip("ダンジョン名")]
    private string dungeonName;
    [SerializeField, Tooltip("ダンジョンの深さと階層ごとの情報")]
    private DepthData[] depth;
    [SerializeField, Tooltip("ダンジョンの最下層シーン名")]
    private string lastScene;

    public string DungeonName { get { return dungeonName; } }
    public DepthData[] Depth { get { return depth; } }
    public string LastScene { get { return lastScene; } }
}

[System.Serializable, Tooltip("各階層の出現データを設定")]
public class DepthData
{
    [SerializeField, Tooltip("各階層におけるフロアの種類を設定")]
    private string[] floorPattern;
    [SerializeField, Tooltip("各階層におけるアイテムの種類と数を設定")]
    private SpawnData[] item;
    [SerializeField, Tooltip("各階層におけるトラップの種類と数を設定")]
    private SpawnData[] trap;
    [SerializeField, Tooltip("各階層における敵の種類と数を設定")]
    private SpawnData[] enemy;

    [SerializeField, Tooltip("落ちているアイテムの数を設定")]
    private int mapItem;
    [SerializeField, Tooltip("設置されているトラップの数を設定")]
    private int mapTrap;
    [SerializeField, Tooltip("棲息している敵の数を設定")]
    private int mapEnemy;
    [SerializeField, Tooltip("敵の最大棲息数を設定")]
    private int maxEnemy;
    [SerializeField, Tooltip("敵が出現する間隔を設定")]
    private float spawnTime;
    [SerializeField, Tooltip("フロア内に留まることができる時間を設定(制限がない場合は'-1'を記入")]
    private float limitTime;

    public string[] FloorPattern { get { return floorPattern; } }
    public SpawnData[] Item { get { return item; } }
    public SpawnData[] Trap { get { return trap; } }
    public SpawnData[] Enemy { get { return enemy; } }
    public int MapItem { get { return mapItem; } }
    public int MapTrap { get { return mapTrap; } }
    public int MapEnemy { get { return mapEnemy; } }
    public int MaxEnemy { get { return maxEnemy; } }
    public float SpawnTime { get { return spawnTime; } }
    public float LimitTime { get { return limitTime; } }
}

[System.Serializable, Tooltip("各出現情報を設定")]
public class SpawnData
{
    [SerializeField, Tooltip("出現するオブジェクトの種類")]
    private GameObject kind;
    [SerializeField, Tooltip("出現するオブジェクトの数")]
    [Range(0, 100)]
    private int quantity;
    [SerializeField, Tooltip("オブジェクトが出現する確率")]
    [Range(0, 100)]
    private int probability;

    public GameObject Kind { get { return kind; } }
    public int Quantity { get { return quantity; } }
    public int Probability { get { return probability; } }
}