using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{

    [Header("외부 캔버스 UI")]
    [SerializeField] private TMP_Text LeftOverTimeLimitText;
    [SerializeField] private TMP_Text battle_TurnText;

    [Header("전투 관련 UI")]
    [SerializeField] private GameObject battle_SkillScrollView;
    [SerializeField] private GameObject PlayerSkillListContent;
    [SerializeField] private GameObject PlayerSkillButtonPrefab;
    [SerializeField] private Image PlayerSkillListBG;
    [SerializeField] private GameObject PlayerRunButton;
    [SerializeField] private Image PlayerRunButtonBG;
    private bool isRunButtonOn = false;

    [Header("플레이어 베이스 UI")]
    [SerializeField] private TextMeshProUGUI PlayerCarelessCount;
    [SerializeField] private Image PlayerNowHpBar;
    [SerializeField] private Image PlayerNowMpBar;
    [SerializeField] private Image PlayerNowCpBar;
    [SerializeField] private Text PlayerCpText;
    [SerializeField] private TMP_Text PlayerHPText;
    [SerializeField] private TMP_Text PlayerMPText;
    [SerializeField] private TextMeshProUGUI PlayerLevelText;
    [SerializeField] private Image PlayerNowExpBar;
    [SerializeField] private TextMeshProUGUI PlayerExpText;
    [SerializeField] private Text PlayerElementsInfo;
    [SerializeField] private Text PlayerStatsInfo;

    [Header("목표 적 베이스 UI")]
    [SerializeField] private GameObject TargetEnemyBaseGroup;
    [SerializeField] private TextMeshProUGUI TargetEnemyCarelessCount;
    [SerializeField] private Image TargetEnemyNowHpBar;
    [SerializeField] private Image TargetEnemyNowMpBar;
    [SerializeField] private Image TargetEnemyNowCpBar;
    [SerializeField] private TextMeshProUGUI TargetEnemyCpText;
    [SerializeField] private TextMeshProUGUI TargetEnemyLevelText;
    [SerializeField] private List<Image> TargetEnemyBaseList_Image;
    [SerializeField] private List<TextMeshProUGUI> TargetEnemyBaseList_TMP;

    [Header("플레이어 스킬 관련 UI")]
    [SerializeField] private GameObject PlayerSkillList;
    [SerializeField] private GameObject SkillButtonsParent;

    [Header("연출용 UI")]
    [SerializeField] private TextMeshProUGUI GameLog;
    [SerializeField] private GameObject battleStartText_0;
    [SerializeField] private GameObject battleStartText_1;

    [Header("탈출 UI")]
    [SerializeField] private GameObject canExitText;

    [Header("인벤토리 UI")]
    [SerializeField] private GameObject ItemList;
    //[SerializeField] private Canvas InventoryUI;

    private Character player = null;
    private EventManager eventManager;
    private bool isCarelessUISetted = false;
    private bool isCarelessUINonSetted = false;
    private List<SO_Skill> skills = new List<SO_Skill>();

    public bool isInvenOn = false;

    private float tempPlayerHp = -1;
    private float tempPlayerMp = -1;
    private float tempPlayerCp = -1;
    private float tempPlayerExp = -1;
    private float tempTargetHp = -1;
    private float tempTargetMp = -1;
    private float tempTargetCp = -1;


    private void Awake()
    {
        player = FindObjectOfType<Player>();
        eventManager = FindObjectOfType<EventManager>();
        if (player.skillList == null)
        {
            Debug.Log("Player Skill List is Null!");
        }
        else
        {
            skills = player.skillList;
            SetPlayerSkillList();
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        // if (BattleManager.instance.nowTurnID == 0)
        // {
        //     //Debug.Log("Battle UI OFF");
        //     playerBattleUI.transform.gameObject.SetActive(false);
        // }
        // else if (BattleManager.instance.nowTurnID == 1)
        // {
        //     battle_SkillScrollView.SetActive(true);
        // }
        // else if (BattleManager.instance.nowTurnID == 2)
        // {
        //     battle_SkillScrollView.SetActive(false);
        // }

        UIUpdate_PlayerElementsInfo();
        UIUpdate_PlayerStatsInfo();
        UIUpdate_NowTurn();
        UIUpdate_PlayerBase();
        UIUpdate_CheckCarelessUIOn();
        UIUpdate_CheckSkillUse();
        UIUpdate_SetPlayerCanExit();
        UIUpdate_SetLeftOverTimeLimit();
        //OnIventory();
    }

    private void UIUpdate_SetPlayerCanExit()
    {
        if (player.GetComponent<Player>().isCanExit)
        {
            canExitText.SetActive(true);
        }
        else
        {
            canExitText.SetActive(false);
        }
    }

    // private void OnIventory()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {

    //         if (BattleManager.instance.nowTurnID == 0)
    //         {
    //             if (InventoryUI.gameObject.activeSelf)
    //             {
    //                 isInvenOn = false;
    //                 InventoryUI.gameObject.SetActive(false);
    //             }
    //             else
    //             {
    //                 isInvenOn = true;
    //                 InventoryUI.gameObject.SetActive(true);
    //             }
    //         }
    //     }
    // }

    private void UIUpdate_NowTurn()
    {
        if (BattleManager.instance.nowTurnID == 0)
        {
            battle_TurnText.text = "NON-BATTLE STATE";
        }
        else if (BattleManager.instance.nowTurnID == 1)
        {
            battle_TurnText.text = "PLAYER\nTURN";
        }
        else if (BattleManager.instance.nowTurnID == 2)
        {
            battle_TurnText.text = "ENEMY\nTURN";
        }
    }

    private void UIUpdate_PlayerBase()
    {
        SetBarSize(PlayerNowHpBar.gameObject, player.nowHP, tempPlayerHp, player.totalStats.MAX_HP);
        SetBarSize(PlayerNowMpBar.gameObject, player.nowMP, tempPlayerMp, player.totalStats.MAX_MP);
        SetBarSize(PlayerNowCpBar.gameObject, player.nowCP, tempPlayerCp, player.maxCP);
        SetBarSize(PlayerNowExpBar.gameObject, player.GetComponent<Player>().EXP, tempPlayerExp, player.GetComponent<Player>().maxEXP);

        string carelessText = "";
        carelessText += player.carelessCounter.ToString() + " / " + player.max_carelessCounter.ToString();

        string expText = "";
        expText += player.GetComponent<Player>().EXP.ToString() + " / " + player.GetComponent<Player>().maxEXP.ToString();

        PlayerExpText.text = expText;
        player.nowCP.ToString();
        PlayerCarelessCount.text = carelessText;
        PlayerCpText.text = player.nowCP.ToString();
        PlayerLevelText.text = "Level " + player.Level;
    }

    public void UIUpdate_TargetEnemyBase(GameObject targetEnemy, bool isIn)
    {
        if (isIn && targetEnemy != null)
        {
            TargetEnemyBaseGroup.SetActive(true);
            // TargetEnemyNowHpBar.SetActive(true);
            // TargetEnemyNowMpBar.SetActive(true);
            // TargetEnemyNowCpBar.SetActive(true);
            // TargetEnemyCpText.gameObject.SetActive(true);
            // TargetEnemyCarelessCount.gameObject.SetActive(true);
            // TargetEnemyLevelText.gameObject.SetActive(true);
            // Debug.Log("TargetIn!");

            BattleManager.instance.SetCharacter(targetEnemy.GetComponent<Character>());

            SetBarSize(TargetEnemyNowHpBar.gameObject, BattleManager.instance.targetCharacter.nowHP, tempTargetHp, BattleManager.instance.targetCharacter.totalStats.MAX_HP);
            SetBarSize(TargetEnemyNowMpBar.gameObject, BattleManager.instance.targetCharacter.nowMP, tempTargetMp, BattleManager.instance.targetCharacter.totalStats.MAX_MP);
            SetBarSize(TargetEnemyNowCpBar.gameObject, BattleManager.instance.targetCharacter.nowCP, tempTargetCp, BattleManager.instance.targetCharacter.maxCP);

            string carelessText = "";

            carelessText += BattleManager.instance.targetCharacter.carelessCounter.ToString() + " / " + BattleManager.instance.targetCharacter.max_carelessCounter.ToString();
            TargetEnemyCpText.text = BattleManager.instance.targetCharacter.nowCP.ToString();
            TargetEnemyCarelessCount.text = carelessText;
            TargetEnemyLevelText.text = "Lv. " + BattleManager.instance.targetCharacter.Level.ToString();
        }
    }

    public void UIUpdate_OffTargetEnemyBase()
    {
        //Debug.Log("OffTarget!");

        tempTargetHp = 0;
        tempTargetMp = 0;
        tempTargetCp = 0;

        TargetEnemyNowHpBar.transform.localScale = new Vector3(0, 1, 1);
        TargetEnemyNowMpBar.transform.localScale = new Vector3(0, 1, 1);
        TargetEnemyNowCpBar.transform.localScale = new Vector3(0, 1, 1);

        //Debug.Log("Target is NUll");
        TargetEnemyBaseGroup.SetActive(false);
        // TargetEnemyNowHpBar.SetActive(false);
        // TargetEnemyNowMpBar.SetActive(false);
        // TargetEnemyNowCpBar.SetActive(false);
        // TargetEnemyCpText.gameObject.SetActive(false);
        // TargetEnemyCarelessCount.gameObject.SetActive(false);
    }

    private void UIUpdate_PlayerStatsInfo()
    {
        PlayerStatsInfo.text = "";
        //SetText(PlayerStatsInfo, player.characterStats.MAX_HP, player.buff_debuffStats.MAX_HP);
        //SetText(PlayerStatsInfo, player.characterStats.MAX_MP, player.buff_debuffStats.MAX_MP);
        SetText(PlayerStatsInfo, player.characterStats.STR);
        SetText(PlayerStatsInfo, player.characterStats.FIR);
        SetText(PlayerStatsInfo, player.characterStats.INT);
        SetText(PlayerStatsInfo, player.characterStats.WIS);
        SetText(PlayerStatsInfo, player.characterStats.FOC);
        SetText(PlayerStatsInfo, player.characterStats.DEX);
        SetText(PlayerStatsInfo, player.characterStats.CHA);
    }

    private void UIUpdate_PlayerElementsInfo()
    {
        SetElementAddInfo(player.totalResistElements, player.totalWeakElements, "Resist", "Weak");
    }

    private void UIUpdate_CheckSkillUse()
    {
        for (int i = 0; i < player.skillList.Count; i++)
        {
            if (skills[i].isCanUse)
            {
                SkillButtonsParent.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                SkillButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void UIUpdate_CheckCarelessUIOn()
    {
        if (BattleManager.instance.player.isBattleMode && BattleManager.instance.targetEnemy.isCareless && !isCarelessUISetted)
        {
            isCarelessUISetted = true;
            isCarelessUINonSetted = false;
            SetCarelessUIActive();
        }
        else if (!isCarelessUINonSetted)
        {
            isCarelessUISetted = false;
            isCarelessUINonSetted = true;
            SetCarelessUIInactive();
        }
    }

    private void UIUpdate_SetLeftOverTimeLimit()
    {
        if (!GameManager.instance.isTimeLimitOver)
        {
            // string tempBeforeString = LeftOverTimeLimitText.text;
            // string tempAfterString = (GameManager.instance.nowTimeLimit - GameManager.instance.maxTimeLimit).ToString("F2");
            // if (tempBeforeString != tempAfterString)
            // {
            //     //FX_ChangingCharacterOfString_forLeftOverTimeLimitText(tempBeforeString, tempAfterString);
            // }
            LeftOverTimeLimitText.text = (GameManager.instance.maxTimeLimit - GameManager.instance.nowTimeLimit).ToString("F2");
        }
        else
        {
            LeftOverTimeLimitText.text = "RUN!";
        }
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

    public void SetBattleUIInactive()
    {
        //playerBattleUI.gameObject.SetActive(false);
        FX_PlayerSkillListInactive();
    }

    public void SetBattleUIActive()
    {
        //playerBattleUI.gameObject.SetActive(true);
        FX_PlayerSkillListActive();
    }

    private void ResetPlayerSkillList()
    {
        SetPlayerSkillListEnable();
        SetPlayerSkillList();
    }

    private void SetPlayerSkillList()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SO_Skill tempSkill = skills[i];
            Debug.Log("Count is " + player.skillList.Count);
            GameObject skillButton;
            skillButton = Instantiate(PlayerSkillButtonPrefab);
            skillButton.transform.SetParent(PlayerSkillListContent.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnSkillClick(tempSkill));

            Color textColor = GetTextColorToElement(tempSkill.skillElements);


            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().color = textColor;
            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = tempSkill.skillName;

            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = Color.white;
            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "차감 MP " + SetIntHundred((int)tempSkill.needMp, Color.white) + "";

            skillButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "필요 CP " + SetIntHundred((int)tempSkill.needCP, Color.white) + "";
            skillButton.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "" + SetIntHundred((int)tempSkill.skillDamage, Color.white) + " 대미지";
            //skillButtons.Add(skillButton);
            //Debug.Log(i);
        }
    }

    private void SetPlayerSkillListEnable()
    {
        for (int i = 0; i < PlayerSkillListContent.transform.childCount; i++)
        {
            Destroy(PlayerSkillList.transform.GetChild(i));
        }

    }

    private void SetBarSize(GameObject tempBarObject, float originValue, float tempValue, float originMaxValue)
    {
        float tempBarSize = originValue / originMaxValue;
        if (tempValue != originValue)
        {
            tempValue = originValue;
            FX_BarSizeChange(tempBarObject, tempBarSize);
        }
    }

    private void SetText<T>(Text text, T state)
    {
        text.text += $"{state}\n";
    }

    private void SetElementAddInfo(Elements resistElements, Elements weakElements, string resistText, string weakText)
    {
        PlayerElementsInfo.text = "";

        for (int i = 0; i < 7; i++)
        {
            switch (i)
            {
                case 0:
                    if (resistElements.SOLAR)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.SOLAR)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 1:
                    if (resistElements.LUMINOUS)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.LUMINOUS)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 2:
                    if (resistElements.IGNITION)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.IGNITION)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 3:
                    if (resistElements.HYDRO)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.HYDRO)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 4:
                    if (resistElements.BIOLOGY)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.BIOLOGY)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 5:
                    if (resistElements.METAL)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.METAL)
                        PlayerElementsInfo.text += weakText;
                    break;
                case 6:
                    if (resistElements.SOIL)
                        PlayerElementsInfo.text += resistText;
                    else if (weakElements.SOIL)
                        PlayerElementsInfo.text += weakText;
                    break;
                default:
                    break;
            }
            PlayerElementsInfo.text += "\n";
        }
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

    private void SetCarelessUIActive()
    {
        FX_PlayerRunButtonActive();
    }

    private void SetCarelessUIInactive()
    {
        FX_PlayerRunButtonInactive();
    }

    private void SetColorAlphaZero(Color tempColor)
    {
        tempColor = new Color(tempColor.r, tempColor.g, tempColor.b, 0);
    }

    private void FX_PlayerSkillListActive()
    {
        PlayerSkillListBG.DOFade(0, 0.125f).OnComplete(() => { PlayerSkillListBG.gameObject.SetActive(false); });
        PlayerSkillList.transform.DOScale(1, 0.125f);
    }

    private void FX_PlayerSkillListInactive()
    {
        PlayerSkillListBG.gameObject.SetActive(true);
        PlayerSkillListBG.DOFade(1f, 0.125f);
        PlayerSkillList.transform.DOScale(0.975f, 0.125f);
    }

    private void FX_PlayerRunButtonActive()
    {
        if (!isRunButtonOn)
        {
            Debug.Log("Actived!");
            isRunButtonOn = true;
            Sequence sequence = DOTween.Sequence();

            sequence
            .SetAutoKill(true)
            .Append(
                PlayerRunButtonBG.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo)
            )
            .Join(
            PlayerRunButtonBG.DOFade(0, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                PlayerRunButtonBG.gameObject.SetActive(false);
            }))
            .Join(
                PlayerRunButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo)
            );
        }
    }

    private void FX_PlayerRunButtonInactive()
    {
        if (isRunButtonOn)
        {
            Debug.Log("InActived!");
            Sequence sequence = DOTween.Sequence();
            PlayerRunButtonBG.gameObject.SetActive(true);

            sequence
            .SetAutoKill(true)
            .Append(
                PlayerRunButtonBG.DOFade(0.5f, 0.5f).SetEase(Ease.OutExpo))
            .Join(
                PlayerRunButtonBG.transform.DOScale(0.95f, 0.5f).SetEase(Ease.OutExpo)
            ).Join(
                PlayerRunButton.transform.DOScale(0.95f, 0.5f).SetEase(Ease.OutExpo)
            );
        }
    }

    private void FX_BarSizeChange(GameObject tempBar, float tempScaleX)
    {
        tempBar.transform.DOScaleX(tempScaleX, 0.25f).SetEase(Ease.OutExpo);
    }

    public void FX_BattleStart()
    {
        Sequence battleStartSequence = DOTween.Sequence().
        Append(
            battleStartText_0.transform.DOLocalMoveX(-10, 0.25f).SetEase(Ease.OutExpo)
        )
        .Join(
            battleStartText_1.transform.DOLocalMoveX(0, 0.25f).SetEase(Ease.OutExpo)
        )
        .AppendInterval(0.25f)
        .Append(
            battleStartText_0.transform.DOLocalMoveX(-960, 0.25f).SetEase(Ease.InExpo)
        )
        .Join(
            battleStartText_1.transform.DOLocalMoveX(960, 0.25f).SetEase(Ease.InExpo)
        );
    }

    // public void FX_ChangingCharacterOfString_forLeftOverTimeLimitText(string beforeString, string afterString)
    // {
    //     //for (LeftOverTimeLimitText.textInfo.characterCount)
    // }

    // private void SetImageListAlphaZero(List<Image> tempImageList)
    // {
    //     for (int i = 0; i < tempImageList.Count; i++)
    //     {
    //         tempImageList[i].color = new Color(tempImageList[i].color.r, tempImageList[i].color.g, tempImageList[i].color.b, 0);
    //     }
    // }
    // private void FX_ImageListActvie(List<Image> tempImageList)
    // {
    //     for (int i = 0; i < tempImageList.Count; i++)
    //     {
    //         tempImageList[i].DOFade(1, 0.5f).SetEase(Ease.OutExpo);
    //     }
    //     // DOTween.To(() => tempImage.color, x => tempImage.color = x, new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 1), 0.5f).SetEase;
    // }
    // private void FX_ImageListInactive(List<Image> tempImageList)
    // {
    //     for (int i = 0; i < tempImageList.Count; i++)
    //     {
    //         tempImageList[i].DOFade(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() => tempImageList[i].gameObject.SetActive(false));
    //     }
    // }

    // private void SetTMPListAlphaZero(List<TextMeshProUGUI> tempTMPList)
    // {
    //     for (int i = 0; i < tempTMPList.Count; i++)
    //     {
    //         tempTMPList[i].color = new Color(tempTMPList[i].color.r, tempTMPList[i].color.g, tempTMPList[i].color.b, 0);
    //     }
    // }
    // private void FX_TMPListActvie(List<TextMeshProUGUI> tempTMPList)
    // {
    //     for (int i = 0; i < tempTMPList.Count; i++)
    //     {
    //         tempTMPList[i].DOFade(1, 0.5f).SetEase(Ease.OutExpo);
    //     }
    // }
    // private void FX_TMPListInactvie(List<TextMeshProUGUI> tempTMPList)
    // {
    //     for (int i = 0; i < tempTMPList.Count; i++)
    //     {
    //         tempTMPList[i].DOFade(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() => tempTMPList[i].gameObject.SetActive(false));
    //     }
    // }
}