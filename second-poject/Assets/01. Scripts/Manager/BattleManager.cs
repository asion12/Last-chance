using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    int nowTurnID = 0;
    // 0: Battle Mode off
    // 1: PlayerTurn
    // 2: EnemyTurn
    // Start is called before the first frame update
    void Start()
    {
    }

    public void TurnChange()
    {
        if (nowTurnID == 1)
        {
            nowTurnID = 2;
        }
        if (nowTurnID == 2)
        {
            nowTurnID = 1;
        }
        else
        {
            Debug.Log("NoOnesTurn!");
        }
    }

    public void BattleStart(bool isPlayerStart)
    {
        if (isPlayerStart)
        {
            nowTurnID = 1;
        }
        else
        {
            nowTurnID = 2;
        }
    }

    public void BattleEnd(bool isPlayerWin)
    {
        nowTurnID = 0;
        if (isPlayerWin)
        {
            PlayerWin();
        }
        else
        {
            EnemyWin();
        }
    }
    public void PlayerWin()
    {
        Debug.Log("Player Win!!");
    }
    public void EnemyWin()
    {
        Debug.Log("Enemy Win!!");
    }

    public void UseSkill(GameObject skillCaster, GameObject skillVictim, SO_Skill castSkill)
    {
        // SkillCaster FOC Check
        if (skillCaster.GetComponent<Character>().characterStats.FOC > castSkill.needFOC)
        {
            castSkill.casterCriticalPer = skillCaster.GetComponent<Character>().characterStats.FOC - castSkill.needFOC;
        }
        else if (skillCaster.GetComponent<Character>().characterStats.FOC < castSkill.needFOC)
        {
            castSkill.accuarityPer -= castSkill.needFOC - skillCaster.GetComponent<Character>().characterStats.FOC;
        }
        else
        {

        }

        // SkillVicTim DEX Check
        castSkill.accuarityPer -= skillCaster.GetComponent<Character>().characterStats.DEX;
        if (castSkill.accuarityPer < 0)
        {
            castSkill.victimDeceptionPer += -1 * castSkill.accuarityPer;
            castSkill.accuarityPer = 0;
        }
    }
}