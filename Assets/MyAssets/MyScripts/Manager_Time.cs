using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Time : MonoBehaviour
{
    [Tooltip("敵がスポーンする間隔を設定します(秒)。")]
    [SerializeField]
    [Range(1f, 120f)]
    private float SpawnTime;
    private float LestTime; // スポーンまでの残り時間

    [Tooltip("同時にスポーンする敵数を設定します(体)。")]
    [SerializeField]
    [Range(1, 30)]
    private int SpawnEnemies;

    [Tooltip("マップに滞在できる制限時間を設定します(秒)。")]
    [SerializeField]
    [Range(30f, 600f)]
    private float TimeLimit;


    private void Awake()
    {
        LestTime = SpawnTime;
    }

    private void Update()
    {
        LestTime -= Time.deltaTime;
        if(LestTime <= 0)
        {
            // 敵がスポーンする処理

            LestTime = SpawnTime;
        }
    }
}