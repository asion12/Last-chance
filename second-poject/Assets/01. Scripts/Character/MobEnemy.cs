using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEnemy : Character
{
    public void CastS()
    {
        BattleManager.instance.TurnChange();
    }
}