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

    public void OnBuyPotion(string potionID)
    {
        if (potionID == "HP_LOW")
        {
            if (CheckPrice(100))
            {
                GameManager.instance.Hp_0++;
                DecreaseGold(100);
            }
        }
        else if (potionID == "MP_LOW")
        {
            if (CheckPrice(100))
            {
                GameManager.instance.MP_0++;
                DecreaseGold(100);
            }
        }
        else if (potionID == "HP_MID")
        {
            if (CheckPrice(250))
            {
                GameManager.instance.HP_1++;
                DecreaseGold(250);
            }
        }
        else if (potionID == "MP_MID")
        {
            if (CheckPrice(250))
            {
                GameManager.instance.MP_1++;
                DecreaseGold(250);
            }
        }
        else if (potionID == "HP_HIGH")
        {
            if (CheckPrice(750))
            {
                GameManager.instance.HP_2++;
                DecreaseGold(750);
            }
        }
        else if (potionID == "MP_HIGH")
        {
            if (CheckPrice(750))
            {
                GameManager.instance.MP_2++;
                DecreaseGold(750);
            }
        }
        else
        {
            Debug.Log("Loss!!!!");
        }
    }

    public void OnSellPotion(string potionID)
    {
        if (potionID == "HP_LOW")
        {
            if (GameManager.instance.Hp_0 >= 1)
            {
                IncreaseGold(100);
                GameManager.instance.Hp_0--;
            }
        }
        else if (potionID == "MP_LOW")
        {
            if (GameManager.instance.MP_0 >= 1)
            {
                IncreaseGold(100);
                GameManager.instance.MP_0--;
            }
        }
        else if (potionID == "HP_MID")
        {
            if (GameManager.instance.HP_1 >= 1)
            {
                IncreaseGold(250);
                GameManager.instance.HP_1--;
            }
        }
        else if (potionID == "MP_MID")
        {
            if (GameManager.instance.MP_1 >= 1)
            {
                IncreaseGold(250);
                GameManager.instance.MP_1--;
            }
        }
        else if (potionID == "HP_HIGH")
        {
            if (GameManager.instance.HP_2 >= 1)
            {
                IncreaseGold(750);
                GameManager.instance.HP_2--;
            }
        }
        else if (potionID == "MP_HIGH")
        {
            if (GameManager.instance.MP_2 >= 1)
            {
                IncreaseGold(750);
                GameManager.instance.MP_2--;
            }
        }
        else
        {
            Debug.Log("Loss!!!!");
        }
    }

    private bool CheckPrice(int itemPrice)
    {
        if (GameManager.instance.Gold >= itemPrice)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DecreaseGold(int decraeseValue)
    {
        GameManager.instance.Gold -= decraeseValue;
    }

    private void IncreaseGold(int increaseValue)
    {
        GameManager.instance.Gold += increaseValue;
    }
}