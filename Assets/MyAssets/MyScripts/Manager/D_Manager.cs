using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �e�_���W�����L�����N�^�[�A�I�u�W�F�N�g�Ȃǂ̊Ǘ����s���܂��B
abstract class D_Manager : MonoBehaviour
{
    public static D_Manager gameInstance;

    /* �����p
    �A�C�e���A�g���b�v�A�G�Ȃǂ͈ȉ��̃R�[�h�Œǉ����邱�Ƃ��ł���B
    dungeon.Items.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    dungeon.Traps.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    dungeon.Enemies.Add(new SpawnProbability { Kind = GameObject, Probability = int })
    Kind�͒ǉ�����I�u�W�F�N�g�̎�ށAprobability�͏o���m�����Ӗ����Ă���B
    */

    // *** �_���W�������, �t���A�\���̐ݒ� ***
    [Header("�_���W�������")]
    [SerializeField, Tooltip("�_���W������")]
    private string dungeonName;
    [SerializeField, Tooltip("�_���W�����̒���")]
    private int maxFloor;
    [SerializeField, Tooltip("�_���W�����Ő[�����j��̃C�x���g�V�[����")]
    private string goalScene;
    [SerializeField, Tooltip("�_���W�����̍\���ꗗ(�V�[������o�^)")]
    protected string[][] structure;

    [Space(10)]

    // *** �_���W�������̓G, �A�C�e��, �g���b�v�̐ݒ� ***
    [Header("�A�C�e�����")]
    [SerializeField, Tooltip("�����A�C�e����")]
    protected int floorItem;
    [SerializeField, Tooltip("�����Ă���A�C�e���̎�ނƊm��")]
    protected List<SpawnProbability> items = new List<SpawnProbability>();

    [Header("�g���b�v���")]
    [SerializeField, Tooltip("�����g���b�v��")]
    protected int floorTrap;
    [SerializeField, Tooltip("�ݒu����Ă���g���b�v�̎�ނƊm��")]
    protected List<SpawnProbability> traps = new List<SpawnProbability>();

    [Header("�G���")]
    [SerializeField, Tooltip("�����̓G��")]
    protected int floorEnemy;
    [SerializeField, Tooltip("�G�̍ő吔")]
    protected int maxEnemy;
    [SerializeField, Tooltip("�������Ă���G�̎�ނƊm��")]
    protected List<SpawnProbability> enemies = new List<SpawnProbability>();
    [SerializeField, Tooltip("�G���o�����鎞�ԊԊu")]
    private float spawnTime;

    // *** �_���W�������ɂ�����i�s�Ǘ��p�ϐ� ***
    /// <summary>
    ///�v���C���[�����錻�݂̊K�w�ł��B
    /// </summary>
    protected int floor = 1;

    /// <summary>
    /// �G���X�|�[�����鎞�Ԃ��Ǘ����܂��B
    /// </summary>
    private float spawnTimer = 0;

    /// <summary>
    /// �_���W�����t���A�Q��؂�ւ��܂��B
    /// </summary>
    protected int floorType = 0;

    /// <summary>
    /// �����t���A���A�����ēo�ꂵ�Ȃ��悤�ɂ��邽�߂̕ϐ��ł��B
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
    /// �t���A���ړ����܂��B("Stair" �I�u�W�F�N�g����Ăяo��)
    /// </summary>
    public void Lift()
    {
        if (floor != maxFloor)
        {
            floor++;
            CallDungeonFloor(); // �q�N���X"Dungeon_(name)"�ŌX�ɐݒ肷��
        }
        else
        {
            S_Manager.SetNewMainScene(goalScene);
        }
    }

    /// <summary>
    /// �t���A�̃I�u�W�F�N�g�����z�u���s���܂��B
    /// </summary>
    protected virtual void FirstSpawn()
    {
        // �A�C�e���̐ݒu
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

        // �g���b�v�̐ݒu
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

        // �G�̐ݒu
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
    /// �t���A���ړ����܂��B("Stair" �I�u�W�F�N�g����Ăяo��)
    /// </summary>
    abstract protected void CallDungeonFloor();

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