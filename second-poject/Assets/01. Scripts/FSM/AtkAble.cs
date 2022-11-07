using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public interface IAtkAble
{
    AtkBehaviour nowAtkBehaviour
    {
        get;
    }

    void OnExecuteAttack(int atkIdx);
}

