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
    }

    public void UIUpdate_PlayerSkillList()
    {
        for (int i = 0; i < player.skillList.Count; i++)
        {
            Debug.Log("Count is " + player.skillList.Count);
            GameObject skillButton;
            skillButton = Instantiate(PlayerSkillButtonPrefab);
            skillButton.transform.SetParent(PlayerSkillListContent.transform);
            List<SO_Skill> skills = player.skillList;
            int idx = i;
            skillButton.GetComponent<Button>().onClick.AddListener(() => eventManager.OnPlayerCastSkillSet(skills[idx]));
            Debug.Log(i);
        }
    }
}
