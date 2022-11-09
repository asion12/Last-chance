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
        if (BattleManager.instance.nowTurnID == 1)
        {
            playerBattleUI.transform.gameObject.SetActive(true);
        }
        else
        {
            playerBattleUI.transform.gameObject.SetActive(false);
        }

        UIUpdate_PlayerElementsInfo();
    }

    public void UIUpdate_PlayerElementsInfo()
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

    public void UIUpdate_PlayerSkillList()
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
