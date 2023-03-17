using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Tooltip("�_���W�����̏����쐬���܂��B")]
[CreateAssetMenu(menuName = "Create Dungeon")]
class DungeonData : ScriptableObject
{
    [Header("�_���W�������")]
    [SerializeField, Tooltip("�_���W������")]
    private string dungeon_name;
    [SerializeField, Tooltip("�_���W�����̐[���ƊK�w���Ƃ̏��")]
    private DepthData[] depth;
    [SerializeField, Tooltip("�_���W�����̍ŉ��w�V�[����")]
    private string last_floor;
    [SerializeField, Tooltip("�_���W������p��Stair�I�u�W�F�N�g��o�^")]
    private GameObject stair;

    public string Dungeon_name { get { return dungeon_name; } }
    public DepthData[] Depth { get { return depth; } }
    public string Last_floor { get { return last_floor; } }
    public GameObject Stair { get { return stair; } }
}

[System.Serializable, Tooltip("�e�K�w�̏o���f�[�^��ݒ�")]
public class DepthData
{
    [SerializeField, Tooltip("�����_���őJ�ڂ�����V�[������ݒ�")]
    private string[] pattern;
    [SerializeField, Tooltip("�o������A�C�e���̎�ނ�ݒ�")]
    private SpawnData[] item;
    [SerializeField, Tooltip("�o������g���b�v�̎�ނ�ݒ�")]
    private SpawnData[] trap;
    [SerializeField, Tooltip("�o������G�̎�ނ�ݒ�")]
    private SpawnData[] enemy;

    [SerializeField, Tooltip("�����Ă���A�C�e���̐���ݒ�")]
    private int onItems;
    [SerializeField, Tooltip("�ݒu����Ă���g���b�v�̐���ݒ�")]
    private int onTraps;
    [SerializeField, Tooltip("�������Ă���(������)�G�̐���ݒ�")]
    private int onEnemies;
    [SerializeField, Tooltip("�G�̍ő吱������ݒ�")]
    private int max_enemy;
    [SerializeField, Tooltip("�G�o�����̃��x���Œ�l��ݒ�('-5' ���� '0')")]
    [Range(-5, 0)]
    private int min_spawn_level;
    [SerializeField, Tooltip("�G�o�����̃��x���ō��l��ݒ�('0' ���� '5')")]
    [Range(0, 5)]
    private int max_spawn_level;
    [SerializeField, Tooltip("�G�̏o���Ԋu��ݒ�(�o�����Ȃ��ꍇ��'-1'���L��")]
    private int spawn_time;
    [SerializeField, Tooltip("�������Ԃ�ݒ�(�������Ȃ��ꍇ��'-1'���L��")]
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

[System.Serializable, Tooltip("�e�o������ݒ�")]
public class SpawnData
{
    [SerializeField, Tooltip("�o������I�u�W�F�N�g�̎��")]
    private GameObject kind;
    [SerializeField, Tooltip("�I�u�W�F�N�g���o������m��")]
    [Range(0, 100)]
    private int probability;

    public GameObject Kind { get { return kind; } }
    public int Probability { get { return probability; } }
}