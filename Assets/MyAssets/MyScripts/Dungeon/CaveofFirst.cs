using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CaveofFirst : D_Manager
{
    protected override void Update() { }

    // �`���[�g���A���}�b�v�̂��ߏ����z�u�̓t���A���ōs���܂��B
    protected override void FirstSpawn() { }


    protected override void CallDungeonFloor()
    {
        // �`���[�g���A���}�b�v�̂��߃}�b�v�Ƀ����_���v�f�͂���܂���(floorType�͏��'0')
        S_Manager.SetNewMainScene(structure[floorType][floor - 1]);
    }
}