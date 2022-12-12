using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    private bool isOverClockCasting = false;
    private Player player = null;
    private OutDungeonUIManager outDungeonUIManager = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        outDungeonUIManager = FindObjectOfType<OutDungeonUIManager>();
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
        GameObject tempObj = EventSystem.current.currentSelectedGameObject;
        if (setSkill.playerSkillSetted)
        {
            Debug.Log("this Skill is now Setting! !");
            outDungeonUIManager.Non_SetSkillInventoryButton(tempObj);
            player.RemoveSkillFromList(setSkill);
        }
        else if (setSkill.playerHavingCount < 1)
        {
            Debug.Log("Not Enough Skill Count!");
        }
        else
        {
            Debug.Log("Skill Setted!");
            outDungeonUIManager.SetSkillInventoryButton(tempObj);
            player.AddSkillToList(setSkill);
        }
        outDungeonUIManager.DungeonEnterCheck();
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
        Debug.Log("Enter Dungeon!");
        GameManager.instance.EnterDungeon();
    }
}