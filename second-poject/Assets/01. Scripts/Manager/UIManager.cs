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
    void Start()
    {

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
}
