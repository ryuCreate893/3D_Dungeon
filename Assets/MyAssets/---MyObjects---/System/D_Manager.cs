using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class D_Manager : MonoBehaviour
{
    public static D_Manager gameInstance;

    [SerializeField, Tooltip("ダンジョンデータを登録")]
    private DungeonData dungeon;
    public DungeonData Dungeon { get { return dungeon; } set { dungeon = value; } }


    // *** ダンジョンの階層管理 ***
    private int floor = 0;
    private int max_floor = 0;
    private float limit_time;
    /// <summary>
    /// 同じシーンが連続して登場しないようにするための変数
    /// </summary>
    private string scene_name;


    // *** 敵のスポーン管理 ***
    private int max_enemy;
    private float spawn_time;
    public int Enemies { get; set; }
    /// <summary>
    /// スポーンストッパー(trueの場合はspawn_timeを経過させない)
    /// </summary>
    private bool spawn_stop = false;

    private GameObject stair;

    private void Awake()
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

    private void Start()
    {
        stair = Dungeon.Stair;
    }

    private void Update()
    {
        // 敵の出現管理
        if (spawn_time != -1)
        {
            spawn_time -= Time.deltaTime;
            if (spawn_time <= 0)
            {
                SetMapEnemy(Dungeon.Depth[floor].Enemy, 1);
                spawn_time = (float)Dungeon.Depth[floor].Spawn_time;
            }
        }

        // フロアの時間制限管理
    }

    /// <summary>
    /// フロアを移動します。("Stair"より呼び出し)
    /// </summary>
    public void Lift()
    {
        Debug.Log("フロアを移動します。");
        floor++;
        int _floor = floor + 1;
        string text = dungeon.Dungeon_name + "\n" + _floor + "F";
        if (floor == max_floor)
        {
            scene_name = dungeon.Last_floor;
        }
        else
        {
            int n = dungeon.Depth[floor].Pattern.Length;
            int rnd = Random.Range(0, n);
            if (dungeon.Depth[floor].Pattern[rnd] == scene_name)
            {
                rnd++;
                if (rnd == n) rnd = 0;
            }
            scene_name = dungeon.Depth[floor].Pattern[rnd];
        }
        StartCoroutine(S_Manager.sceneInstance.SetNewMainScene(scene_name, text));
        Player.playerInstance.ResetPlayerStatusUI();
        Debug.Log("フロアを移動しました。→[" + floor + "F]");
    }

    /// <summary>
    /// フロアの初期オブジェクト配置を行います。
    /// </summary>
    public void SetFloor()
    {
        // 最深部以外で行われる初期処理
        if (floor != max_floor)
        {
            // 各変数のセット
            max_enemy = Dungeon.Depth[floor].Max_enemy;
            spawn_time = (float)Dungeon.Depth[floor].Spawn_time;
            limit_time = (float)Dungeon.Depth[floor].Limit_time;

            // オブジェクト配置
            SetMapGimmick(Dungeon.Depth[floor].Item, Dungeon.Depth[floor].OnItems);
            SetMapGimmick(Dungeon.Depth[floor].Trap, Dungeon.Depth[floor].OnTraps);
            SetMapEnemy(Dungeon.Depth[floor].Enemy, Dungeon.Depth[floor].OnEnemies);

            // Stairの設置
            if (Stair.stairInstance != null)
            {
                GameObject _stair = Instantiate(stair);
                SetTransform(_stair);
            }
        }
        else
        {
            max_enemy = Dungeon.Depth[floor].Max_enemy;
            spawn_time = -1;
            limit_time = -1;
        }
    }

    private void SetMapGimmick(SpawnData[] data, int n)
    {
        for (int i = 0; i < n; i++)
        {
            bool isSpawn = false;
            GameObject obj = null;
            for (int j = 0; j < data.Length; j++)
            {
                if (Random.Range(0, 101) <= data[j].Probability)
                {
                    obj = Instantiate(data[j].Kind);
                    isSpawn = true;
                    break;
                }
            }

            // 確率を参照したがすべてスルーした場合
            if (!isSpawn)
            {
                obj = Instantiate(data[0].Kind);
            }
            SetTransform(obj);
        }
    }

    private void SetMapEnemy(SpawnData[] data, int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (Enemies == max_enemy)
            {
                break;
            }
            else
            {
                bool isSpawn = false;
                GameObject obj = null;

                for (int j = 0; j < data.Length; j++)
                {
                    if (Random.Range(0, 101) <= data[j].Probability)
                    {
                        obj = Instantiate(data[j].Kind);
                        isSpawn = true;
                        break;
                    }
                }
                // 確率を参照したがすべてスルーした場合
                if (!isSpawn)
                {
                    obj = Instantiate(data[0].Kind);
                }
                SetTransform(obj);
                int level = Random.Range(Dungeon.Depth[floor].Min_spawn_level, Dungeon.Depth[floor].Max_spawn_level + 1);
                Character method = obj.GetComponent<Character>();
                if (level > 0)
                {
                    for(int k = 0; k < level; k++)
                    {
                        method.LevelUp();
                    }
                }
                else if(level < 0)
                {
                    for (int k = 0; k < level; k++)
                    {
                        method.LevelDown();
                    }
                }
                Enemies++;
            }
        }
    }

    private void SetTransform(GameObject obj)
    {
        bool isOverlap = true;
        float r = 3.0f;
        Vector3 v3 = Vector3.zero;

        while (isOverlap)
        {
            float x = Random.Range(-5, 5);
            float z = Random.Range(-5, 5);
            v3 = new Vector3(x, 0.5f, z);

            if (Physics.OverlapSphere(v3, r).Length == 1) // 地面以外に触れていない場合
            {
                isOverlap = false;
            }
        }

        obj.transform.position = v3;
        float rot = Random.Range(-360, 360);
        obj.transform.rotation = Quaternion.Euler(0, rot, 0);
    }
}