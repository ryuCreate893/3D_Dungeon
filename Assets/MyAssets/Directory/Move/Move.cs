using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Move : Mover
{
    /// <summary>
    /// �L�����N�^�[���擾����ړ��x�N�g���p�̕ϐ��ł��B
    /// </summary>
    

    public override Vector3 SetVelocity()
    {
        return user.transform.forward;
    }
}
