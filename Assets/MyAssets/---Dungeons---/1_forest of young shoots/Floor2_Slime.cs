using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Floor2_Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        lose_distance = 20.0f;
        FoundEnemy(Player.playerInstance.gameObject);
    }
}
