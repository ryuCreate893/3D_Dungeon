using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("�_���W�����̏����쐬���܂��B")]
[CreateAssetMenu(menuName = "Create Dungeon")]
class DungeonData : ScriptableObject
{
    [Header("�_���W�������")]
    [SerializeField, Tooltip("�_���W������")]
    private string dungeonName;
    [SerializeField, Tooltip("�_���W�����̐[���ƊK�w���Ƃ̏��")]
    private DepthData[] depth;
    [SerializeField, Tooltip("�_���W�����̍ŉ��w�V�[����")]
    private string lastScene;

    public string DungeonName { get { return dungeonName; } }
    public DepthData[] Depth { get { return depth; } }
    public string LastScene { get { return lastScene; } }
}

[System.Serializable, Tooltip("�e�K�w�̏o���f�[�^��ݒ�")]
public class DepthData
{
    [SerializeField, Tooltip("�e�K�w�ɂ�����t���A�̎�ނ�ݒ�")]
    private string[] floorPattern;
    [SerializeField, Tooltip("�e�K�w�ɂ�����A�C�e���̎�ނƐ���ݒ�")]
    private SpawnData[] item;
    [SerializeField, Tooltip("�e�K�w�ɂ�����g���b�v�̎�ނƐ���ݒ�")]
    private SpawnData[] trap;
    [SerializeField, Tooltip("�e�K�w�ɂ�����G�̎�ނƐ���ݒ�")]
    private SpawnData[] enemy;

    [SerializeField, Tooltip("�����Ă���A�C�e���̐���ݒ�")]
    private int mapItem;
    [SerializeField, Tooltip("�ݒu����Ă���g���b�v�̐���ݒ�")]
    private int mapTrap;
    [SerializeField, Tooltip("�������Ă���G�̐���ݒ�")]
    private int mapEnemy;
    [SerializeField, Tooltip("�G�̍ő吱������ݒ�")]
    private int maxEnemy;
    [SerializeField, Tooltip("�G���o������Ԋu��ݒ�")]
    private float spawnTime;
    [SerializeField, Tooltip("�t���A���ɗ��܂邱�Ƃ��ł��鎞�Ԃ�ݒ�(�������Ȃ��ꍇ��'-1'���L��")]
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

[System.Serializable, Tooltip("�e�o������ݒ�")]
public class SpawnData
{
    [SerializeField, Tooltip("�o������I�u�W�F�N�g�̎��")]
    private GameObject kind;
    [SerializeField, Tooltip("�o������I�u�W�F�N�g�̐�")]
    [Range(0, 100)]
    private int quantity;
    [SerializeField, Tooltip("�I�u�W�F�N�g���o������m��")]
    [Range(0, 100)]
    private int probability;

    public GameObject Kind { get { return kind; } }
    public int Quantity { get { return quantity; } }
    public int Probability { get { return probability; } }
}