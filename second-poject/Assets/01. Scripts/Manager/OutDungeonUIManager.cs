using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class OutDungeonUIManager : MonoBehaviour
{
    [Header("기본 오브젝트")]
    [SerializeField] private Canvas OutDungeonUI;
    [SerializeField] private GameObject OutDungeonUIGroup;

    [Header("스킬 인벤토리 버튼 관련")]
    [SerializeField] private List<SO_Skill> PlayerInventorySkillListData;
    [SerializeField] private GameObject PlayerInventorySkillButtonPrefab;
    [SerializeField] private GameObject PlayerInventorySkillListContent;

    [Header("던전 진입 관련")]
    [SerializeField] private TextMeshProUGUI EnterInfo_0;
    [SerializeField] private TextMeshProUGUI EnterInfo_1;
    [SerializeField] private TextMeshProUGUI EnterInfo_2;
    [SerializeField] private Image DungeonEnterButton;
    [SerializeField] private Image DungeonEnterButtonBG;

    private bool enterCheck_0 = false;
    private bool enterCheck_1 = false;
    private bool enterCheck_2 = false;

    private EventManager eventManager;
    private Player player;
    private void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        ResetPlayerSkillInventory();
        DungeonEnterCheck();
    }

    private void Update()
    {
    }

    public void SetActiveDungeonEnterButton()
    {
        if (enterCheck_0 && enterCheck_1 && enterCheck_2)
        {
            FX_ActiveDungeonEnterButton();
        }
        else
        {
            FX_InactiveDungeonEnterButton();
        }
    }

    private void FX_ActiveDungeonEnterButton()
    {
        DungeonEnterButtonBG.DOFade(0, 0.125f).OnComplete(() => { DungeonEnterButtonBG.gameObject.SetActive(false); });
    }

    private void FX_InactiveDungeonEnterButton()
    {
        DungeonEnterButtonBG.gameObject.SetActive(true);
        DungeonEnterButtonBG.DOFade(0.75f, 0.125f);
    }

    public void DungeonEnterCheck()
    {
        if (player.isOverResist || player.isOverWeak)
        {
            enterCheck_0 = false;
            FX_InactiveEnterInfo(EnterInfo_0);
        }
        else
        {
            enterCheck_0 = true;
            FX_ActiveEnterInfo(EnterInfo_0);
        }

        if (!CheckPlayerHasNoMp0Skill())
        {
            enterCheck_1 = false;
            FX_InactiveEnterInfo(EnterInfo_1);
        }
        else
        {
            enterCheck_1 = true;
            FX_ActiveEnterInfo(EnterInfo_1);
        }

        if (player.skillList.Count < 1 || 5 < player.skillList.Count)
        {
            enterCheck_2 = false;
            FX_InactiveEnterInfo(EnterInfo_2);
        }
        else
        {
            enterCheck_2 = true;
            FX_ActiveEnterInfo(EnterInfo_2);
        }
        SetActiveDungeonEnterButton();
    }

    private bool CheckPlayerHasNoMp0Skill()
    {
        for (int i = 0; i < player.skillList.Count; i++)
        {
            if (player.skillList[i].needMp <= 0)
            {
                return true;
            }
        }
        return false;
    }

    private void FX_ActiveEnterInfo(TextMeshProUGUI tempTMP)
    {
        tempTMP.DOFade(1, 0.125f);
    }

    private void FX_InactiveEnterInfo(TextMeshProUGUI tempTMP)
    {
        tempTMP.DOFade(0.25f, 0.125f);
    }

    public void ResetPlayerSkillInventory()
    {
        EnablePlayerSkillInventory();
        SetPlayerSkillInvnetory();
    }

    private void EnablePlayerSkillInventory()
    {
        for (int i = 0; i < PlayerInventorySkillListContent.transform.childCount; i++)
        {
            Destroy(PlayerInventorySkillListContent.transform.GetChild(i).gameObject);
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
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnSkillSet(tempSkill));

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

    public void Non_SetSkillInventoryButton(GameObject SkillInventoryButton)
    {
        FX_SkillInventoryBGInactive(SkillInventoryButton.transform.GetChild(3).gameObject.GetComponent<Image>());
        FX_SkillInventorySetTextInactive(SkillInventoryButton.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>());
    }

    public void SetSkillInventoryButton(GameObject SkillInventoryButton)
    {
        FX_SkillInventoryBGActive(SkillInventoryButton.transform.GetChild(3).gameObject.GetComponent<Image>());
        FX_SkillInventorySetTextActive(SkillInventoryButton.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>());
    }

    private void FX_SkillInventoryBGActive(Image effectImage)
    {
        effectImage.DOFade(0.75f, 0.125f);
    }

    private void FX_SkillInventoryBGInactive(Image effectImage)
    {
        effectImage.DOFade(0f, 0.125f);
    }

    private void FX_SkillInventorySetTextActive(TextMeshProUGUI effectText)
    {
        effectText.DOFade(1, 0.125f);
    }

    private void FX_SkillInventorySetTextInactive(TextMeshProUGUI effectText)
    {
        effectText.DOFade(0, 0.125f);
    }
}