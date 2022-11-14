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
    [SerializeField] private Text Gold;
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
        //Gold.text = GameManager.instance.Gold.ToString();
        //�ù� ����� ���� ���Ķ�  �Ф��ؼ� �̿� �Ф�.
        //if (BattleManager.instance.nowTurnID == 0)
        //{
        //    playerBattleUI.transform.gameObject.SetActive(false);
        //}
        //else if (BattleManager.instance.nowTurnID == 1)
        //{
        //    playerBattleUI.transform.gameObject.SetActive(true);
        //    battle_SkillScrollView.SetActive(true);
        //}
        //else if (BattleManager.instance.nowTurnID == 2)
        //{
        //    playerBattleUI.transform.gameObject.SetActive(true);
        //    battle_SkillScrollView.SetActive(false);
        //}

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
