using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    private bool isOverClockCasting = false;
    private Player player = null;
    private OutDungeonUIManager outDungeonUIManager = null;
    private StoreManager_New storeManager_New = null;
    private EffectManager effectManager = null;
    private bool isSellMode = false;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        outDungeonUIManager = FindObjectOfType<OutDungeonUIManager>();
        storeManager_New = FindObjectOfType<StoreManager_New>();
        effectManager = FindObjectOfType<EffectManager>();
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
            if (BattleManager.instance.nowTurnID == 1)
            {
                if (setSkill.needMp <= player.nowMP)
                    effectManager.MakeSkillEffect(setSkill, true);
                PlayerCastSkillSet(setSkill);
            }
        }
    }

    public void OnInvenSkillClick(SO_Skill targetSkill)
    {
        if (isSellMode)
        {
            OnSkillSetSellMode(targetSkill);
        }
        else
        {
            OnSkillSet(targetSkill);
        }
    }

    public void OnSkillSetSellMode(SO_Skill sellSkill)
    {
        if (sellSkill.playerSkillSetted)
        {
            Debug.Log("Player Skill Setted! you cant sell this skill");
        }
        else
        {
            sellSkill.isSell = true;
        }
    }

    public void OnSkillSet(SO_Skill setSkill)
    {
        //player.SetTotalElements();
        Debug.Log("Skill Set Start");
        GameObject tempObj = EventSystem.current.currentSelectedGameObject;
        if (setSkill.playerSkillSetted)
        {
            Debug.Log("Remove Skill Start !");
            outDungeonUIManager.Non_SetSkillInventoryButton(tempObj);
            player.RemoveSkillFromList(setSkill);
            //outDungeonUIManager.DungeonEnterCheck();
        }
        else if (setSkill.playerHavingCount < 1)
        {
            Debug.Log("Not Enough Skill Count!");
            //outDungeonUIManager.DungeonEnterCheck();
        }
        else
        {
            Debug.Log("Set Skill Start !");
            outDungeonUIManager.SetSkillInventoryButton(tempObj);
            player.AddSkillToList(setSkill);
            //outDungeonUIManager.DungeonEnterCheck();
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
        Debug.Log("Enter Dungeon!");
        GameManager.instance.EnterDungeon();
    }

    public void OnIncreaseStat(string statID)
    {
        if (statID == "STR")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.STR >= 32)
            {
                Debug.Log("STR OVER");
            }
            else
            {
                player.characterStats.STR++;
                player.statPoint--;
            }
        }
        else if (statID == "FIR")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.FIR >= 32)
            {
                Debug.Log("FIR OVER");
            }
            else
            {
                player.characterStats.FIR++;
                player.statPoint--;
            }
        }
        else if (statID == "INT")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.INT >= 32)
            {
                Debug.Log("INT OVER");
            }
            else
            {
                player.characterStats.INT++;
                player.statPoint--;
            }
        }
        else if (statID == "WIS")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.WIS >= 32)
            {
                Debug.Log("WIS OVER");
            }
            else
            {
                player.characterStats.WIS++;
                player.statPoint--;
            }
        }
        else if (statID == "FOC")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.FOC >= 32)
            {
                Debug.Log("FOC OVER");
            }
            else
            {
                player.characterStats.FOC++;
                player.statPoint--;
            }
        }
        else if (statID == "DEX")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.DEX >= 32)
            {
                Debug.Log("DEX OVER");
            }
            else
            {
                player.characterStats.DEX++;
                player.statPoint--;
            }
        }
        else if (statID == "CHA")
        {
            if (player.statPoint <= 0)
            {
                Debug.Log("Not Enough Point!");
            }
            else if (player.characterStats.CHA >= 32)
            {
                Debug.Log("CHA OVER");
            }
            else
            {
                player.characterStats.CHA++;
                player.statPoint--;
            }
        }
    }

    public void OnDecreaseStat(string statID)
    {
        if (statID == "STR")
        {
            if (player.characterStats.STR <= 2)
            {
                Debug.Log("STR ENDLESS");
            }
            else
            {
                player.characterStats.STR--;
                player.statPoint++;
            }
        }
        else if (statID == "FIR")
        {
            if (player.characterStats.FIR <= 2)
            {
                Debug.Log("FIR ENDLESS");
            }
            else
            {
                player.characterStats.FIR--;
                player.statPoint++;
            }
        }
        else if (statID == "INT")
        {
            if (player.characterStats.INT <= 2)
            {
                Debug.Log("INT ENDLESS");
            }
            else
            {
                player.characterStats.INT--;
                player.statPoint++;
            }
        }
        else if (statID == "WIS")
        {
            if (player.characterStats.STR <= 2)
            {
                Debug.Log("WIS ENDLESS");
            }
            else
            {
                player.characterStats.WIS--;
                player.statPoint++;
            }
        }
        else if (statID == "FOC")
        {
            if (player.characterStats.FOC <= 2)
            {
                Debug.Log("FOC ENDLESS");
            }
            else
            {
                player.characterStats.FOC--;
                player.statPoint++;
            }
        }
        else if (statID == "DEX")
        {
            if (player.characterStats.DEX <= 2)
            {
                Debug.Log("DEX ENDLESS");
            }
            else
            {
                player.characterStats.DEX--;
                player.statPoint++;
            }
        }
        else if (statID == "CHA")
        {
            if (player.characterStats.CHA <= 2)
            {
                Debug.Log("CHA ENDLESS");
            }
            else
            {
                player.characterStats.CHA--;
                player.statPoint++;
            }
        }
    }

    public void OnBuyPotion(string potionID)
    {
        storeManager_New.BuyPotion(potionID);
    }

    public void OnSellPotion(string potionID)
    {
        storeManager_New.SellPotion(potionID);
    }
}