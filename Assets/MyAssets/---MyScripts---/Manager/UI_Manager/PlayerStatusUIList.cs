using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUIList : MonoBehaviour
{
    [SerializeField]
    private PlayerStatusUI hp;
    [SerializeField]
    private PlayerStatusUI sp;
    [SerializeField]
    private PlayerStatusUI exp;

    public PlayerStatusUI Hp { get { return hp; } }
    public PlayerStatusUI Sp { get { return sp; } }
    public PlayerStatusUI Exp { get { return exp; } }
}