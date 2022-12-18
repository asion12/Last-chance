using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager_New : MonoBehaviour
{
    public List<SO_Skill> StoreSkillPool;
    public List<SO_Skill> nowStoreSkillTable;
    public List<SO_Skill> nowOrderSkillTable;
    OutDungeonUIManager outDungeonUIManager;
    // Start is called before the first frame update
    void Start()
    {
        outDungeonUIManager = FindObjectOfType<OutDungeonUIManager>();
        ResetSkillStore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ResetSkillStore()
    {
        nowStoreSkillTable = new List<SO_Skill>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, StoreSkillPool.Count);
            nowStoreSkillTable.Add(StoreSkillPool[randomIndex]);
        }
    }

    public void BuySkill(SO_Skill tempSkill)
    {
        if (CheckPrice(tempSkill.buyCost))
        {
            DecreaseGold(tempSkill.buyCost);
            tempSkill.playerHavingCount++;
        }
    }

    public void SellSkillSet(SO_Skill tempSkill)
    {
        if (tempSkill.playerHavingCount > 0 && tempSkill.playerSkillSetted == false)
        {
            // IncreaseGold(tempSkill.buyCost);
            // tempSkill.playerHavingCount--;
            tempSkill.isSell = true;
        }
    }

    public void SellAllSkillSet(SO_Skill tempSkill)
    {
        // for(int i = 0; i < outDungeonUIManager.PlayerInventorySkillListData.Count; i++)
        // {
        //     if()
        // }
    }

    public void SellPotion(string potionID)
    {
        if (potionID == "HP_LOW")
        {
            if (GameManager.instance.HP_0 >= 1)
            {
                IncreaseGold(100);
                GameManager.instance.HP_0--;
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
                IncreaseGold(450);
                GameManager.instance.HP_1--;
            }
        }
        else if (potionID == "MP_MID")
        {
            if (GameManager.instance.MP_1 >= 1)
            {
                IncreaseGold(450);
                GameManager.instance.MP_1--;
            }
        }
        else if (potionID == "HP_HIGH")
        {
            if (GameManager.instance.HP_2 >= 1)
            {
                IncreaseGold(2000);
                GameManager.instance.HP_2--;
            }
        }
        else if (potionID == "MP_HIGH")
        {
            if (GameManager.instance.MP_2 >= 1)
            {
                IncreaseGold(2000);
                GameManager.instance.MP_2--;
            }
        }
        else
        {
            Debug.Log("Loss!!!!");
        }
        GameManager.instance.CheckPotionCount();
        outDungeonUIManager.DungeonEnterCheck();
    }

    public void BuyPotion(string potionID)
    {
        if (potionID == "HP_LOW")
        {
            if (CheckPrice(100))
            {
                GameManager.instance.HP_0++;
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
            if (CheckPrice(450))
            {
                GameManager.instance.HP_1++;
                DecreaseGold(450);
            }
        }
        else if (potionID == "MP_MID")
        {
            if (CheckPrice(450))
            {
                GameManager.instance.MP_1++;
                DecreaseGold(450);
            }
        }
        else if (potionID == "HP_HIGH")
        {
            if (CheckPrice(2000))
            {
                GameManager.instance.HP_2++;
                DecreaseGold(2000);
            }
        }
        else if (potionID == "MP_HIGH")
        {
            if (CheckPrice(2000))
            {
                GameManager.instance.MP_2++;
                DecreaseGold(2000);
            }
        }
        else
        {
            Debug.Log("Loss!!!!");
        }
        GameManager.instance.CheckPotionCount();
        outDungeonUIManager.DungeonEnterCheck();
    }

    private void IncreaseGold(int increaseValue)
    {
        GameManager.instance.Gold += increaseValue;
    }

    private void DecreaseGold(int decraeseValue)
    {
        GameManager.instance.Gold -= decraeseValue;
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
}
