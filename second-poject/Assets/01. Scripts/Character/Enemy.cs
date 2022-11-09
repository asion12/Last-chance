using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private bool isChanging = false;
    protected override void Update()
    {
        base.Update();
        if (BattleManager.instance.nowTurnID == 2 && !isChanging)
        {
            StartCoroutine(CastSkill());
        }
    }
    public IEnumerator CastSkill()
    {
        isChanging = true;
        yield return new WaitForSeconds(1);
        BattleManager.instance.TurnChange();
        Debug.Log("Enemy Casted!");
        Debug.Log("Turn Changed");
        isChanging = false;
    }
}
