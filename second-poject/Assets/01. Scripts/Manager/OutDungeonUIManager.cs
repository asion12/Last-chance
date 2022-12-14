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

    [Header("상점 관련")]
    [SerializeField] private TextMeshProUGUI PlayerGoldText;
    [SerializeField] private GameObject SkillTableParent;
    [SerializeField] private GameObject SkillTableButtonPrefab;

    [Header("스탯 업그레이드 관련")]
    public TextMeshProUGUI addStatInfo_STR;
    public TextMeshProUGUI addStatInfo_FIR;
    public TextMeshProUGUI addStatInfo_INT;
    public TextMeshProUGUI addStatInfo_WIS;
    public TextMeshProUGUI addStatInfo_FOC;
    public TextMeshProUGUI addStatInfo_DEX;
    public TextMeshProUGUI addStatInfo_CHA;
    [SerializeField] private TextMeshProUGUI PlayerStatPointText;
    [SerializeField] private bool enterCheck_0 = false;
    [SerializeField] private bool enterCheck_1 = false;
    [SerializeField] private bool enterCheck_2 = false;

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
        UIUpdate_SetPlayerGoldText();
        UIUpdate_SetPlayerStatPointText();
        UIUpdate_SetPlayerStatAddInfo();
    }

    private void EnableSkillTable()
    {
        for (int i = 0; i < SkillTableParent.transform.childCount; i++)
        {
            Destroy(SkillTableParent.transform.GetChild(0));
        }
    }

    public void SetSkillTable(List<SO_Skill> setSkills)
    {
        for (int i = 0; i < setSkills.Count; i++)
        {
            GameObject tempObj;
            tempObj = Instantiate(SkillTableButtonPrefab);
            tempObj.transform.SetParent(SkillTableParent.transform);
        }
    }

    private void UIUpdate_SetPlayerStatAddInfo()
    {
        addStatInfo_STR.text = "+" + (player.characterStats.STR - 2).ToString();
        addStatInfo_FIR.text = "+" + (player.characterStats.FIR - 2).ToString();
        addStatInfo_INT.text = "+" + (player.characterStats.INT - 2).ToString();
        addStatInfo_WIS.text = "+" + (player.characterStats.WIS - 2).ToString();
        addStatInfo_FOC.text = "+" + (player.characterStats.FOC - 2).ToString();
        addStatInfo_DEX.text = "+" + (player.characterStats.DEX - 2).ToString();
        addStatInfo_CHA.text = "+" + (player.characterStats.CHA - 2).ToString();
    }

    public void UIUpdate_SetPlayerGoldText()
    {
        PlayerGoldText.text = GameManager.instance.Gold.ToString() + " G";
    }

    public void UIUpdate_SetPlayerStatPointText()
    {
        PlayerStatPointText.text = player.statPoint.ToString() + " SP";
    }

    public void CheckActiveDungeonEnterButton()
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
            //Debug.Log("0 inactive!");
            enterCheck_0 = false;
            FX_InactiveEnterInfo(EnterInfo_0);
        }
        else
        {
            //Debug.Log("0 active!");
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

        if (10 < player.skillList.Count + GameManager.instance.potionCount)
        {
            enterCheck_2 = false;
            FX_InactiveEnterInfo(EnterInfo_2);
        }
        else
        {
            enterCheck_2 = true;
            FX_ActiveEnterInfo(EnterInfo_2);
        }

        Debug.Log("Enter Checked!");
        CheckActiveDungeonEnterButton();
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
            tempSkill.playerSkillSetted = false;
            GameObject skillButton;
            skillButton = Instantiate(PlayerInventorySkillButtonPrefab);
            skillButton.transform.SetParent(PlayerInventorySkillListContent.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnInvenSkillClick(tempSkill));

            Color textColor = GetTextColorToElement(tempSkill.skillElements);

            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().color = textColor;
            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tempSkill.skillName;

            //skillButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

            skillButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "차감 MP " + SetIntHundred((int)tempSkill.needMp, Color.white) + "";

            string tempStr = "";
            if (tempSkill.categoryPhysics)
            {
                tempStr = "물리";
            }
            else
            {
                tempStr = "화학";
            }

            skillButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = tempStr + " 대미지 " + SetIntHundred((int)tempSkill.skillDamage, Color.white) + "";
            //skillButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;

            Color color;
            ColorUtility.TryParseHtmlString("#FFB300", out color);
            skillButton.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().color = color;
            skillButton.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "필요 CP " + SetIntHundred((int)tempSkill.needCP, Color.white) + "";

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
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "SOLAR";
                    }
                    else if (tempSkill.setWeakElements.SOLAR)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "SOLAR";
                    }
                    break;
                case 1:
                    if (tempSkill.setResistElements.LUMINOUS)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "LUMINOUS";
                    }
                    else if (tempSkill.setWeakElements.LUMINOUS)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "LUMINOUS";
                    }
                    break;
                case 2:
                    if (tempSkill.setResistElements.IGNITION)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "IGNITION";
                    }
                    else if (tempSkill.setWeakElements.IGNITION)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "IGNITION";
                    }
                    break;
                case 3:
                    if (tempSkill.setResistElements.HYDRO)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "HYDRO";
                    }
                    else if (tempSkill.setWeakElements.HYDRO)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "HYDRO";
                    }
                    break;
                case 4:
                    if (tempSkill.setResistElements.BIOLOGY)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "BIOLOGY";
                    }
                    else if (tempSkill.setWeakElements.BIOLOGY)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "BIOLOGY";
                    }
                    break;
                case 5:
                    if (tempSkill.setResistElements.METAL)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "METAL";
                    }
                    else if (tempSkill.setWeakElements.METAL)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "METAL";
                    }
                    break;
                case 6:
                    if (tempSkill.setResistElements.SOIL)
                    {
                        elementResOrWckInfoString += "RESIST";
                        elementInfoString += "SOIL";
                    }
                    else if (tempSkill.setWeakElements.SOIL)
                    {
                        elementResOrWckInfoString += "WEAK";
                        elementInfoString += "SOIL";
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
        Color tempColor = Color.white;
        if (el.SOLAR)
            ColorUtility.TryParseHtmlString("#ebc802", out tempColor);
        else if (el.LUMINOUS)
            ColorUtility.TryParseHtmlString("#2a0599", out tempColor);
        else if (el.IGNITION)
            ColorUtility.TryParseHtmlString("#FF0505", out tempColor);
        else if (el.HYDRO)
            ColorUtility.TryParseHtmlString("#02d0eb", out tempColor);
        else if (el.BIOLOGY)
            ColorUtility.TryParseHtmlString("#03961e", out tempColor);
        else if (el.METAL)
            ColorUtility.TryParseHtmlString("#4a4d59", out tempColor);
        else if (el.SOIL)
            ColorUtility.TryParseHtmlString("#75350d", out tempColor);

        return tempColor;
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