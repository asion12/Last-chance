using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    // about Base UI
    public Canvas playerBattleUI;

    // about Battle
    [SerializeField] private Text battle_TurnText;
    [SerializeField] private GameObject battle_SkillScrollView;
    [SerializeField] private Text battle_PlayerElementsInfo;
    [SerializeField] private Text battle_PlayerStatsInfo;
    [SerializeField] private GameObject PlayerSkillListContent;
    [SerializeField] private GameObject PlayerSkillButtonPrefab;
    [SerializeField] private Text Gold;

    [SerializeField] private GameObject PlayerNowHpBar;
    [SerializeField] private GameObject PlayerNowMpBar;

    [SerializeField] private Text PlayerCarelessCount;
    [SerializeField] private Text TargetEnemyCarelessCount;

    [SerializeField] private GameObject TargetEnemyNowHpBar;
    [SerializeField] private GameObject TargetEnemyNowMpBar;

    [SerializeField] private GameObject SkillButtonsParent;

    [SerializeField] private Text GameLog;
    [SerializeField] private Canvas CarelessSkillUI;
    private Character player = null;
    private EventManager eventManager;
    private List<SO_Skill> skills = new List<SO_Skill>();

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        if (player.skillList == null)
        {
            Debug.Log("Player Skill List is Null!");
        }
        else
        {
            skills = player.skillList;
            UIUpdate_PlayerSkillList();
        }
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        eventManager = FindObjectOfType<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gold.text = GameManager.instance.Gold.ToString();
        //�ù� ����� ���� ���Ķ�  �Ф��ؼ� �̿� �Ф�.
        if (BattleManager.instance.nowTurnID == 0)
        {
            playerBattleUI.transform.gameObject.SetActive(false);
        }
        else if (BattleManager.instance.nowTurnID == 1)
        {
            playerBattleUI.transform.gameObject.SetActive(true);
            battle_SkillScrollView.SetActive(true);
        }
        else if (BattleManager.instance.nowTurnID == 2)
        {
            playerBattleUI.transform.gameObject.SetActive(true);
            battle_SkillScrollView.SetActive(false);
        }

        UIUpdate_PlayerElementsInfo();
        UIUpdate_PlayerStatsInfo();
        UIUpdate_NowTurn();
        UIUpdate_PlayerBase();
        UIUpdate_CheckCarelessUIOn();
        UIUpdate_CheckSkillUse();
    }

    private void UIUpdate_NowTurn()
    {
        if (BattleManager.instance.nowTurnID == 0)
        {
            battle_TurnText.text = "PEACE";
        }
        else if (BattleManager.instance.nowTurnID == 1)
        {
            battle_TurnText.text = "PLAYER TURN";
        }
        else if (BattleManager.instance.nowTurnID == 2)
        {
            battle_TurnText.text = "ENEMY TURN";
        }
    }

    private void UIUpdate_PlayerBase()
    {
        string carelessText = "";

        float hpBarSize = (float)player.nowHP / (float)player.totalStats.MAX_HP;
        float mpBarSize = (float)player.nowMP / (float)player.totalStats.MAX_MP;
        PlayerNowHpBar.transform.localScale = new Vector3(hpBarSize, 1, 1);
        PlayerNowMpBar.transform.localScale = new Vector3(mpBarSize, 1, 1);

        carelessText += player.carelessCounter.ToString() + " / " + player.max_carelessCounter.ToString();

        PlayerCarelessCount.text = carelessText;
    }

    public void UIUpdate_TargetEnemyBase(GameObject targetEnemy, bool isIn)
    {
        if (isIn && targetEnemy != null)
        {
            TargetEnemyNowHpBar.SetActive(true);
            TargetEnemyNowMpBar.SetActive(true);
            TargetEnemyCarelessCount.gameObject.SetActive(true);

            Debug.Log("TargetIn!");

            BattleManager.instance.SetCharacter(targetEnemy.GetComponent<Character>());

            float hpBarSize = (float)BattleManager.instance.targetCharacter.nowHP / (float)BattleManager.instance.targetCharacter.totalStats.MAX_HP;
            float mpBarSize = (float)BattleManager.instance.targetCharacter.nowMP / (float)BattleManager.instance.targetCharacter.totalStats.MAX_MP;
            string carelessText = "";
            TargetEnemyNowHpBar.transform.localScale = new Vector3(hpBarSize, 1, 1);
            TargetEnemyNowMpBar.transform.localScale = new Vector3(mpBarSize, 1, 1);
            carelessText += BattleManager.instance.targetCharacter.carelessCounter.ToString() + " / " + BattleManager.instance.targetCharacter.max_carelessCounter.ToString();
            TargetEnemyCarelessCount.text = carelessText;
        }
    }

    public void UIUpdate_OffTargetEnemyBase()
    {
        Debug.Log("Target is NUll");
        TargetEnemyNowHpBar.SetActive(false);
        TargetEnemyNowMpBar.SetActive(false);
        TargetEnemyCarelessCount.gameObject.SetActive(false);
    }

    private void SetText<T>(Text text, T state, T state2)
    {
        text.text += $"{state} {state2}\n";
    }

    private void UIUpdate_PlayerStatsInfo()
    {
        battle_PlayerStatsInfo.text = "";
        SetText(battle_PlayerStatsInfo, player.characterStats.MAX_HP, player.buff_debuffStats.MAX_HP);
        SetText(battle_PlayerStatsInfo, player.characterStats.MAX_MP, player.buff_debuffStats.MAX_MP);
        SetText(battle_PlayerStatsInfo, player.characterStats.STR, player.buff_debuffStats.STR);
        SetText(battle_PlayerStatsInfo, player.characterStats.FIR, player.buff_debuffStats.FIR);
        SetText(battle_PlayerStatsInfo, player.characterStats.INT, player.buff_debuffStats.INT);
        SetText(battle_PlayerStatsInfo, player.characterStats.WIS, player.buff_debuffStats.WIS);
        SetText(battle_PlayerStatsInfo, player.characterStats.FOC, player.buff_debuffStats.FOC);
        SetText(battle_PlayerStatsInfo, player.characterStats.DEX, player.buff_debuffStats.DEX);
        SetText(battle_PlayerStatsInfo, player.characterStats.CHA, player.buff_debuffStats.CHA);
    }

    private void UIUpdate_PlayerElementsInfo()
    {
        CheckElementAndAddInfo(player.totalResistElements, player.totalWeakElements, "Resist", "Weak");
    }

    private void CheckElementAndAddInfo(Elements resistElements, Elements weakElements, string resistText, string weakText)
    {
        battle_PlayerElementsInfo.text = "";

        for (int i = 0; i < 7; i++)
        {
            switch (i)
            {
                case 0:
                    if (resistElements.SOLAR)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.SOLAR)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 1:
                    if (resistElements.LUMINOUS)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.LUMINOUS)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 2:
                    if (resistElements.IGNITION)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.IGNITION)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 3:
                    if (resistElements.HYDRO)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.HYDRO)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 4:
                    if (resistElements.BIOLOGY)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.BIOLOGY)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 5:
                    if (resistElements.METAL)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.METAL)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                case 6:
                    if (resistElements.CLAY)
                        battle_PlayerElementsInfo.text += resistText;
                    else if (weakElements.CLAY)
                        battle_PlayerElementsInfo.text += weakText;
                    break;
                default:
                    break;
            }
            battle_PlayerElementsInfo.text += "\n";
        }
    }

    private void UIUpdate_PlayerSkillList()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            Debug.Log("Count is " + player.skillList.Count);
            GameObject skillButton;
            skillButton = Instantiate(PlayerSkillButtonPrefab);
            skillButton.transform.SetParent(PlayerSkillListContent.transform);
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnSkillClick(skills[i]));
            Debug.Log("now IDX = " + i);
            Debug.Log(skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text);
            Debug.Log(skills[i].skillName);
            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = skills[i].skillName;

            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "NEED " + SetIntHundred((int)skills[i].needMp) + " MP";
            skillButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "NEED " + SetIntHundred((int)skills[i].needFOC) + " FOC";
            skillButton.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "GIVE " + SetIntHundred((int)skills[i].skillDamage) + " DMG";
            //skillButtons.Add(skillButton);
            //Debug.Log(i);
        }
    }

    private void UIUpdate_CheckSkillUse()
    {
        for (int i = 0; i < player.skillList.Count; i++)
        {
            if (skills[i].isCanUse)
            {
                //SkillButtonsParent.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                //SkillButtonsParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private string SetIntHundred(int num)
    {
        string numString = num.ToString();
        string changeToString = "";
        for (int i = 0; i < 3 - numString.Length; i++)
        {
            changeToString += '0';
        }
        changeToString += numString;

        return changeToString;
    }

    private void UIUpdate_CheckCarelessUIOn()
    {
        if (BattleManager.instance.player.battleMode && BattleManager.instance.targetEnemy.isCareless)
        {
            CarelessSkillUI.gameObject.SetActive(true);
        }
        else
        {
            CarelessSkillUI.gameObject.SetActive(false);
        }
    }
}
