using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private bool isOverClockCasting = false;
    private Player player = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

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

    public void OnSkillSet(SO_Skill setSkill)
    {
        if (setSkill.playerSkillSetted)
        {
            Debug.Log("this Skill is now Setting! !");
            player.RemoveSkillFromList(setSkill);
            setSkill.playerHavingCount++;
        }
        else if (setSkill.playerHavingCount < 1)
        {
            Debug.Log("Not Enough Skill Count!");
        }
        else
        {
            player.AddSkillToList(setSkill);
            setSkill.playerHavingCount--;
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

    public void OnEnterDungeon()
    {
        GameManager.instance.EnterDungeon();
    }
}