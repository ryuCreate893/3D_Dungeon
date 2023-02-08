using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 各ダンジョンキャラクター、オブジェクトなどの管理を行います。
abstract class D_Manager : MonoBehaviour
{
    public static D_Manager gameInstance;

    /* メモ用
    アイテム、トラップ、敵などは以下のコードで追加することができる。
    dungeon.Items.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    dungeon.Traps.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    dungeon.Enemies.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    Kindは追加するオブジェクトの種類、probabilityは出現確率を意味している。
    */

    // *** ダンジョン情報, フロア構造の設定 ***
    [Header("ダンジョン情報")]
    [SerializeField, Tooltip("ダンジョン名")]
    private string dungeonName;
    [SerializeField, Tooltip("ダンジョンの長さ")]
    private int maxFloor;
    [SerializeField, Tooltip("ダンジョン最深部踏破後のイベントシーン名")]
    private string goalScene;
    [SerializeField, Tooltip("ダンジョンの構造一覧(シーン名を登録)")]
    protected string[][] structure;

    [Space(10)]

    // *** ダンジョン内の敵, アイテム, トラップの設定 ***
    [Header("アイテム情報")]
    [SerializeField, Tooltip("初期アイテム数")]
    protected int floorItem;
    [SerializeField, Tooltip("落ちているアイテムの種類と確率")]
    protected List<SpawnProbability> items = new List<SpawnProbability>();

    [Header("トラップ情報")]
    [SerializeField, Tooltip("初期トラップ数")]
    protected int floorTrap;
    [SerializeField, Tooltip("設置されているトラップの種類と確率")]
    protected List<SpawnProbability> traps = new List<SpawnProbability>();

    [Header("敵情報")]
    [SerializeField, Tooltip("初期の敵数")]
    protected int floorEnemy;
    [SerializeField, Tooltip("敵の最大数")]
    protected int maxEnemy;
    [SerializeField, Tooltip("生息している敵の種類と確率")]
    protected List<SpawnProbability> enemies = new List<SpawnProbability>();
    [SerializeField, Tooltip("敵が出現する時間間隔")]
    private float spawnTime;

    // *** ダンジョン中における進行管理用変数 ***
    /// <summary>
    ///プレイヤーがいる現在の階層です。
    /// </summary>
    protected int floor = 1;

    /// <summary>
    /// 敵がスポーンする時間を管理します。
    /// </summary>
    private float spawnTimer = 0;

    /// <summary>
    /// ダンジョンフロア群を切り替えます。
    /// </summary>
    protected int floorType = 0;

    /// <summary>
    /// 同じフロアが連続して登場しないようにするための変数です。
    /// </summary>
    protected int floorNumber = -1;


    protected virtual void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].SpawnCheck)
                {
                    Spawn(enemies[i].Kind);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// フロアを移動します。("Stair" オブジェクトから呼び出し)
    /// </summary>
    public void Lift()
    {
        if (floor != maxFloor)
        {
            floor++;
            CallDungeonFloor(); // 子クラス"Dungeon_(name)"で個々に設定する
        }
        else
        {
            S_Manager.SetNewMainScene(goalScene);
        }
    }

    /// <summary>
    /// フロアのオブジェクト初期配置を行います。
    /// </summary>
    protected virtual void FirstSpawn()
    {
        // アイテムの設置
        for (int i = 0; i < floorItem; i++)
        {
            for (int j = 0; j < items.Count; j++)
            {
                if (items[j].SpawnCheck)
                {
                    Spawn(items[j].Kind);
                    break;
                }
            }
        }

        // トラップの設置
        for (int i = 0; i < floorTrap; i++)
        {
            for (int j = 0; j < traps.Count; j++)
            {
                if (traps[j].SpawnCheck)
                {
                    Spawn(traps[j].Kind);
                    break;
                }
            }
        }

        // 敵の設置
        for (int i = 0; i < floorEnemy; i++)
        {
            for (int j = 0; j < enemies.Count; j++)
            {
                if (enemies[j].SpawnCheck)
                {
                    Spawn(enemies[j].Kind);
                    break;
                }
            }
        }
    }

    private void Spawn(GameObject spawn)
    {
        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        Vector3 v3 = new Vector3(x, 0.5f, z);
        GameObject obj = Instantiate(spawn);
        obj.transform.position = v3;
    }

    /// <summary>
    /// フロアを移動します。("Stair" オブジェクトから呼び出し)
    /// </summary>
    abstract protected void CallDungeonFloor();

    [System.Serializable]
    [Tooltip("アイテム, トラップ, 敵の種類と発生確率を設定します。\n'SpawnCheck'で確率 の判定を行うこともできます。")]
    public class SpawnProbability
    {
        [Tooltip("オブジェクトの種類")]
        private GameObject kind;
        [Tooltip("オブジェクトが出現する確率")]
        [Range(0, 100)]
        private int probability;

        public GameObject Kind { get { return kind; } set { kind = value; } }

        public int Probability { get { return probability; } set { probability = value; } }

        /// <summary>
        /// スポーン確率を判定して、trueの場合該当の敵をスポーンさせる
        /// </summary>
        public bool SpawnCheck { get { return Random.Range(1, 101) <= probability; } }
    }
}