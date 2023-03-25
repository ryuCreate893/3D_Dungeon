using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slime_Press_Collider : MonoBehaviour
{
    private int atk;

    public void SetValue(int atk)
    {
        this.atk = atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player_Body"))
        {
            Player.playerInstance.DamageCalculation(atk);
        }
    }
}