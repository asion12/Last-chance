using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Player player;
    private EventManager eventManager;

    void Start()
    {
        player = FindObjectOfType<Player>();
        eventManager = FindObjectOfType<EventManager>();
        UIUpdate_PlayerSkillList();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void UIUpdate_PlayerStatsInfo()
    {
        battle_PlayerStatsInfo.text = "";
        battle_PlayerStatsInfo.text += player.nowHP + " ";
        battle_PlayerStatsInfo.text += player.characterStats.MAX_HP;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.nowMP + " ";
        battle_PlayerStatsInfo.text += player.characterStats.MAX_MP;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.STR + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.STR;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.FIR + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.FIR;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.INT + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.INT;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.WIS + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.WIS;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.FOC + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.FOC;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.DEX + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.DEX;
        battle_PlayerStatsInfo.text += "\n";

        battle_PlayerStatsInfo.text += player.characterStats.CHA + " ";
        battle_PlayerStatsInfo.text += player.buff_debuffStats.CHA;
        battle_PlayerStatsInfo.text += "\n";
    }

    private void UIUpdate_PlayerElementsInfo()
    {
        CheckElementAndAddInfo(player.resistElements, player.weakElements, "Resist", "Weak");
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
        for (int i = 0; i < player.skillList.Count; i++)
        {
            //Debug.Log("Count is " + player.skillList.Count);
            GameObject skillButton;
            skillButton = Instantiate(PlayerSkillButtonPrefab);
            skillButton.transform.SetParent(PlayerSkillListContent.transform);
            List<SO_Skill> skills = player.skillList;
            int idx = i;
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnPlayerCastSkillSet(skills[idx]));
            skillButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = skills[idx].skillName;
            skillButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "NEED FOC : " + SetIntHundred((int)skills[idx].needFOC);
            //Debug.Log(i);
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
}
