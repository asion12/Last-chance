using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void OnPlayerCastSkillSet()
    {
        BattleManager.instance.CastSkill(BattleManager.instance.player, BattleManager.instance.targetEnemy, transform.gameObject.GetComponent<SO_Skill>());
    }
}
