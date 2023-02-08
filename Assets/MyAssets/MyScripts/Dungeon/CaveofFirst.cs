using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CaveofFirst : D_Manager
{
    protected override void Update() { }

    // チュートリアルマップのため初期配置はフロア側で行います。
    protected override void FirstSpawn() { }


    protected override void CallDungeonFloor()
    {
        // チュートリアルマップのためマップにランダム要素はありません(floorTypeは常に'0')
        S_Manager.SetNewMainScene(structure[floorType][floor - 1]);
    }
}