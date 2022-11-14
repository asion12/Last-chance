using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void OnPlayerCastSkillSet(SO_Skill castSkill)
    {
        BattleManager.instance.CastSkill(BattleManager.instance.player, BattleManager.instance.targetEnemy, castSkill);
    }
}