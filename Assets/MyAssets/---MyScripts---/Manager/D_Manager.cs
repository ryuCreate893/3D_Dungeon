using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ダンジョン内の進行管理を行います。
class D_Manager : MonoBehaviour
{
    [SerializeField, Tooltip("先へ行くための階段などのオブジェクトを登録します。")]
    private GameObject stair;
    [SerializeField, Tooltip("S_Managerを登録します。")]
    private S_Manager scene_manager;
    [SerializeField]
    private DungeonData dungeon;
    public static D_Manager gameInstance;

    /// <summary>
    /// ダンジョンをセットします。
    /// </summary>
    public DungeonData Dungeon { get { return dungeon; } set { dungeon = value; } }

    // *** ダンジョンの階層管理 ***
    /// <summary>
    /// 現在の階層
    /// </summary>
    public int Floor { get; private set; }
    /// <summary>
    /// 最下層
    /// </summary>
    private int maxFloor = 0;
    /// <summary>
    /// 現在の階層に留まることができる時間
    /// </summary>
    private float limitTime;
    /// <summary>
    /// 同じフロアが連続して登場しないようにするためにフロア名を記憶する変数
    /// </summary>
    private string floorName;


    // *** 敵のスポーン管理 ***
    /// <summary>
    /// 敵がスポーンする時間間隔
    /// </summary>
    private float maxSpawnTime;
    /// <summary>
    /// スポーンタイマー
    /// </summary>
    private float spawnTime;
    /// <summary>
    /// スポーンストッパー(trueの場合はspawnTimeを経過させない)
    /// </summary>
    private bool stop = false;



    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            maxFloor = Dungeon.Depth.Length;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        maxSpawnTime = Dungeon.Depth[Floor].SpawnTime;
        spawnTime = maxSpawnTime;
        limitTime = Dungeon.Depth[Floor].LimitTime;
        int rnd = Random.Range(0, Dungeon.Depth[Floor].FloorPattern.Length);
        floorName = Dungeon.Depth[Floor].FloorPattern[rnd];
    }

    private void Update()
    {
        if (spawnTime != -1)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
                SelectSpawn(1, Dungeon.Depth[Floor].Enemy);
                spawnTime = maxSpawnTime;
            }
        }
    }

    /// <summary>
    /// フロアを移動します。("Stair" オブジェクトから呼び出し)
    /// </summary>
    public void Lift()
    {
        Floor++;
        if (Floor == maxFloor)
        {
            S_Manager.SetLastMainScene(Dungeon.LastScene);
        }
        else
        {
            int n = Dungeon.Depth[Floor].FloorPattern.Length;
            int rnd = Random.Range(0, n);
            if (Dungeon.Depth[Floor].FloorPattern[rnd] == floorName)
            {
                rnd++;
                if (rnd == n) rnd = 0;
            }
            floorName = Dungeon.Depth[Floor].FloorPattern[rnd];
            scene_manager.SetNewMainScene(floorName);
        }
    }

    /// <summary>
    /// フロアの初期オブジェクト配置を行います。
    /// </summary>
    public void SetFloor()
    {
        SelectSpawn(Dungeon.Depth[Floor].MapItem, Dungeon.Depth[Floor].Item);
        SelectSpawn(Dungeon.Depth[Floor].MapTrap, Dungeon.Depth[Floor].Trap);
        SelectSpawn(Dungeon.Depth[Floor].MapEnemy, Dungeon.Depth[Floor].Enemy);
    }

    private void SelectSpawn(int n, SpawnData[] data)
    {
        bool check = false;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < data.Length; j++)
            {
                if (Random.Range(0, 101) <= data[j].Probability)
                {
                    Spawn(data[j].Kind);
                    check = true;
                    break;
                }
                if (!check)
                {
                    int rnd = Random.Range(0, data.Length);
                    Spawn(data[rnd].Kind);
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
    protected virtual void CallDungeonFloor() { }

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