using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class D_Manager : MonoBehaviour
{
    public static D_Manager gameInstance;

    [SerializeField, Tooltip("�_���W�����f�[�^��o�^")]
    private DungeonData dungeon;
    public DungeonData Dungeon { get { return dungeon; } set { dungeon = value; } }


    // *** �_���W�����̊K�w�Ǘ� ***
    private int floor = 0;
    private int max_floor = 0;
    private float limit_time;
    /// <summary>
    /// �����V�[�����A�����ēo�ꂵ�Ȃ��悤�ɂ��邽�߂̕ϐ�
    /// </summary>
    private string scene_name;


    // *** �G�̃X�|�[���Ǘ� ***
    private int max_enemy;
    private float spawn_time;
    public int Enemies { get; set; }
    /// <summary>
    /// �X�|�[���X�g�b�p�[(true�̏ꍇ��spawn_time���o�߂����Ȃ�)
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
        // �G�̏o���Ǘ�
        if (spawn_time != -1)
        {
            spawn_time -= Time.deltaTime;
            if (spawn_time <= 0)
            {
                SetMapEnemy(Dungeon.Depth[floor].Enemy, 1);
                spawn_time = (float)Dungeon.Depth[floor].Spawn_time;
            }
        }

        // �t���A�̎��Ԑ����Ǘ�
    }

    /// <summary>
    /// �t���A���ړ����܂��B("Stair"���Ăяo��)
    /// </summary>
    public void Lift()
    {
        Debug.Log("�t���A���ړ����܂��B");
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
        Debug.Log("�t���A���ړ����܂����B��[" + floor + "F]");
    }

    /// <summary>
    /// �t���A�̏����I�u�W�F�N�g�z�u���s���܂��B
    /// </summary>
    public void SetFloor()
    {
        // �Ő[���ȊO�ōs���鏉������
        if (floor != max_floor)
        {
            // �e�ϐ��̃Z�b�g
            max_enemy = Dungeon.Depth[floor].Max_enemy;
            spawn_time = (float)Dungeon.Depth[floor].Spawn_time;
            limit_time = (float)Dungeon.Depth[floor].Limit_time;

            // �I�u�W�F�N�g�z�u
            SetMapGimmick(Dungeon.Depth[floor].Item, Dungeon.Depth[floor].OnItems);
            SetMapGimmick(Dungeon.Depth[floor].Trap, Dungeon.Depth[floor].OnTraps);
            SetMapEnemy(Dungeon.Depth[floor].Enemy, Dungeon.Depth[floor].OnEnemies);

            // Stair�̐ݒu
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

            // �m�����Q�Ƃ��������ׂăX���[�����ꍇ
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
                // �m�����Q�Ƃ��������ׂăX���[�����ꍇ
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

            if (Physics.OverlapSphere(v3, r).Length == 1) // �n�ʈȊO�ɐG��Ă��Ȃ��ꍇ
            {
                isOverlap = false;
            }
        }

        obj.transform.position = v3;
        float rot = Random.Range(-360, 360);
        obj.transform.rotation = Quaternion.Euler(0, rot, 0);
    }
}