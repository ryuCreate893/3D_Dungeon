using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �_���W�������̐i�s�Ǘ����s���܂��B
class D_Manager : MonoBehaviour
{
    public static D_Manager gameInstance;

    /// <summary>
    /// �_���W�������Z�b�g���܂��B
    /// </summary>
    public DungeonData Dungeon { get; set; }

    // *** �_���W�����̊K�w�Ǘ� ***
    /// <summary>
    /// ���݂̊K�w
    /// </summary>
    private int floor = 0;
    /// <summary>
    /// �ŉ��w
    /// </summary>
    private int maxFloor = 0;
    /// <summary>
    /// ���݂̊K�w�ɗ��܂邱�Ƃ��ł��鎞��
    /// </summary>
    private float limitTime;
    /// <summary>
    /// �����t���A���A�����ēo�ꂵ�Ȃ��悤�ɂ��邽�߂Ƀt���A�����L������ϐ�
    /// </summary>
    private string floorName;


    // *** �G�̃X�|�[���Ǘ� ***
    /// <summary>
    /// �G���X�|�[�����鎞�ԊԊu
    /// </summary>
    private float maxSpawnTime;
    /// <summary>
    /// �X�|�[���^�C�}�[
    /// </summary>
    private float spawnTime;
    /// <summary>
    /// �X�|�[���X�g�b�p�[(true�̏ꍇ��spawnTime���o�߂����Ȃ�)
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
        maxSpawnTime = Dungeon.Depth[floor].SpawnTime;
        spawnTime = maxSpawnTime;
        limitTime = Dungeon.Depth[floor].LimitTime;
        int rnd = Random.Range(0, Dungeon.Depth[floor].FloorPattern.Length);
        floorName = "a";
    }

    private void Update()
    {
        if (limitTime != -1)
        {
            limitTime -= Time.deltaTime;
        }

        if (spawnTime != -1)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
                SelectSpawn(1, Dungeon.Depth[floor].Enemy);
                spawnTime = maxSpawnTime;
            }
        }
    }

    /// <summary>
    /// �t���A���ړ����܂��B("Stair" �I�u�W�F�N�g����Ăяo��)
    /// </summary>
    public void Lift()
    {
        floor++;
        if (floor == maxFloor)
        {
            S_Manager.SetLastMainScene(Dungeon.LastScene);
        }
        else
        {
            int n = Dungeon.Depth[floor].FloorPattern.Length;
            int rnd = Random.Range(0, n);
            if (Dungeon.Depth[floor].FloorPattern[rnd] == floorName)
            {
                if (rnd == n - 1)
                {
                    rnd = 0;
                }
                else
                {
                    rnd++;
                }
            }
            floorName = Dungeon.Depth[floor].FloorPattern[rnd];
            S_Manager.SetNewMainScene(floorName);
        }
    }

    /// <summary>
    /// �t���A�̏����I�u�W�F�N�g�z�u���s���܂��B
    /// </summary>
    public void SetFloor()
    {
        SelectSpawn(Dungeon.Depth[floor].MapItem, Dungeon.Depth[floor].Item);
        SelectSpawn(Dungeon.Depth[floor].MapTrap, Dungeon.Depth[floor].Trap);
        SelectSpawn(Dungeon.Depth[floor].MapEnemy, Dungeon.Depth[floor].Enemy);
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
    /// �t���A���ړ����܂��B("Stair" �I�u�W�F�N�g����Ăяo��)
    /// </summary>
    protected virtual void CallDungeonFloor() { }

    [System.Serializable]
    [Tooltip("�A�C�e��, �g���b�v, �G�̎�ނƔ����m����ݒ肵�܂��B\n'SpawnCheck'�Ŋm�� �̔�����s�����Ƃ��ł��܂��B")]
    public class SpawnProbability
    {
        [Tooltip("�I�u�W�F�N�g�̎��")]
        private GameObject kind;
        [Tooltip("�I�u�W�F�N�g���o������m��")]
        [Range(0, 100)]
        private int probability;

        public GameObject Kind { get { return kind; } set { kind = value; } }

        public int Probability { get { return probability; } set { probability = value; } }

        /// <summary>
        /// �X�|�[���m���𔻒肵�āAtrue�̏ꍇ�Y���̓G���X�|�[��������
        /// </summary>
        public bool SpawnCheck { get { return Random.Range(1, 101) <= probability; } }
    }
}