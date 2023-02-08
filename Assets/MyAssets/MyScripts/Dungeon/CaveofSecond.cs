using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CaveofSecond : D_Manager
{
    protected override void FirstSpawn()
    {
        base.FirstSpawn();
    }

    protected override void CallDungeonFloor()
    {
        int rnd;
        if (floor == 3)
        {
            // 3�K�w�ڂ�˔j������_���W�����̃^�C�v��ς���
            floorType++;
            rnd = Random.Range(0, structure[floorType].Length);
        }
        else if (floorNumber == -1)
        {
            rnd = Random.Range(0, structure[floorType].Length);
        }
        else
        {
            rnd = Random.Range(0, structure[floorType].Length - 1);
            if (rnd == floorNumber) rnd++;
        }
        floorNumber = rnd;
        S_Manager.SetNewMainScene(structure[floorType][rnd]);
    }
}