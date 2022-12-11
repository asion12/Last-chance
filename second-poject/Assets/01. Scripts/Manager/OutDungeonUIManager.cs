using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutDungeonUIManager : MonoBehaviour
{
    [SerializeField] private Canvas OutDungeonUI;
    [SerializeField] private List<SO_Skill> PlayerInventorySkillListData;
    [SerializeField] private GameObject PlayerInventorySkillButtonPrefab;
    [SerializeField] private GameObject PlayerInventorySkillListContent;
    private EventManager eventManager;
    private void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        ResetPlayerSkillInventory();
    }

    private void ResetPlayerSkillInventory()
    {
        EnablePlayerSkillInventory();
        SetPlayerSkillInvnetory();
    }

    private void EnablePlayerSkillInventory()
    {
        for (int i = 0; i < PlayerInventorySkillListContent.transform.childCount; i++)
        {
            Destroy(PlayerInventorySkillListContent.transform.GetChild(i));
        }
    }

    private void SetPlayerSkillInvnetory()
    {
        for (int i = 0; i < PlayerInventorySkillListData.Count; i++)
        {
            SO_Skill tempSkill = PlayerInventorySkillListData[i];
            GameObject skillButton;
            skillButton = Instantiate(PlayerInventorySkillButtonPrefab);
            skillButton.transform.SetParent(PlayerInventorySkillListContent.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnSkillClick(tempSkill));

            Color textColor = GetTextColorToElement(tempSkill.skillElements);

            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().color = textColor;
            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tempSkill.skillName;

            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "차감 MP " + SetIntHundred((int)tempSkill.needMp, Color.white) + "";

            skillButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "필요 CP " + SetIntHundred((int)tempSkill.needCP, Color.white) + "";
            skillButton.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "" + SetIntHundred((int)tempSkill.skillDamage, Color.white) + " 대미지";

            // 스킬 추가 정보 표기
            skillButton.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = SetElementInfoString(tempSkill);
            skillButton.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "Count : " + tempSkill.playerHavingCount.ToString();
            //skillButtons.Add(skillButton);
            //Debug.Log(i);
        }
    }

    private string SetElementInfoString(SO_Skill tempSkill)
    {
        string tempString = "";
        string elementResOrWckInfoString = "";
        string elementInfoString = "";
        for (int i = 0; i < 7; i++)
        {
            switch (i)
            {
                case 0:
                    if (tempSkill.setResistElements.SOLAR)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Solar";
                    }
                    else if (tempSkill.setWeakElements.SOLAR)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Solar";
                    }
                    break;
                case 1:
                    if (tempSkill.setResistElements.LUMINOUS)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Luminous";
                    }
                    else if (tempSkill.setWeakElements.LUMINOUS)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Luminous";
                    }
                    break;
                case 2:
                    if (tempSkill.setResistElements.IGNITION)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Ignition";
                    }
                    else if (tempSkill.setWeakElements.IGNITION)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Ignition";
                    }
                    break;
                case 3:
                    if (tempSkill.setResistElements.HYDRO)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Hydro";
                    }
                    else if (tempSkill.setWeakElements.HYDRO)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Hydro";
                    }
                    break;
                case 4:
                    if (tempSkill.setResistElements.BIOLOGY)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Biology";
                    }
                    else if (tempSkill.setWeakElements.BIOLOGY)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Biology";
                    }
                    break;
                case 5:
                    if (tempSkill.setResistElements.METAL)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Metal";
                    }
                    else if (tempSkill.setWeakElements.METAL)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Metal";
                    }
                    break;
                case 6:
                    if (tempSkill.setResistElements.SOIL)
                    {
                        elementResOrWckInfoString += "Resist";
                        elementInfoString += "Soil";
                    }
                    else if (tempSkill.setWeakElements.SOIL)
                    {
                        elementResOrWckInfoString += "Weak";
                        elementInfoString += "Soil";
                    }
                    break;
                default:
                    break;
            }
        }
        if (elementInfoString == "" && elementInfoString == "")
        {
            elementResOrWckInfoString = "NULL";
            elementInfoString = "NULL";
        }

        tempString = elementResOrWckInfoString + " : " + elementInfoString;
        return tempString;
    }

    private Color GetTextColorToElement(Elements el)
    {
        if (el.SOLAR)
            return Color.yellow;
        else if (el.LUMINOUS)
            return Color.magenta;
        else if (el.IGNITION)
            return Color.red;
        else if (el.HYDRO)
            return Color.blue;
        else if (el.BIOLOGY)
            return Color.green;
        else if (el.METAL)
            return Color.black;
        else if (el.SOIL)
            return Color.gray;

        return Color.cyan;
    }

    private string SetIntHundred(int num, Color textColor)
    {
        string numString = num.ToString();
        string changeToString = "";

        string colorString = "<color=";
        colorString += "#" + ColorUtility.ToHtmlStringRGB(textColor) + "88";
        colorString += ">";

        for (int i = 0; i < 3 - numString.Length; i++)
        {
            changeToString += colorString + "0</color>";
        }
        changeToString += numString;

        return changeToString;
    }

    public void ActiveOutDungeonUI()
    {
        OutDungeonUI.transform.gameObject.SetActive(true);
    }

    public void InactiveOutDungeonUI()
    {
        OutDungeonUI.transform.gameObject.SetActive(false);
    }
}