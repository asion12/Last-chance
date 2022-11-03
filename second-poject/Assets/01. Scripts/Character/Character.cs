using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    public Stats characterStats = new Stats();
    [SerializeField]
    public Elements resistElements = new Elements();
    [SerializeField]
    public Elements weakElements = new Elements();
    private BattleManager battleManager;
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }
    public void GetStat(int statID)
    {
        switch (statID)
        {
            case 0:
                break;
            default:
                break;
        }
    }
}
