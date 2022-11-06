using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stats characterStats = new Stats();
    public Elements resistElements = new Elements();
    public Elements weakElements = new Elements();
    public int carelessCounter = 0;
    protected BattleManager battleManager;
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }
    // public void GetStat(int statID)
    // {
    //     switch (statID)
    //     {
    //     }
    // }
}
