using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private bool isOverClockCasting = false;


    public void OnPlayerOverClockCast()
    {
        isOverClockCasting = true;
    }

    public void OnSkillClick(SO_Skill setSkill)
    {

        if (isOverClockCasting)
        {
            PlayerOverClockSkillSet(setSkill);
        }
        else
        {
            PlayerCastSkillSet(setSkill);
        }
    }

    public void PlayerCastSkillSet(SO_Skill castSkill)
    {
        BattleManager.instance.CastSkill(BattleManager.instance.player, BattleManager.instance.targetEnemy, castSkill);
    }

    public void PlayerOverClockSkillSet(SO_Skill overClockSkill)
    {

    }

    public void OnPlayerBattleRun()
    {
        StartCoroutine(BattleManager.instance.BattleRun(true));
    }

    public void OnDungeonTravelStart()
    {
        GameManager.instance.isGameStarted = true;
    }
}