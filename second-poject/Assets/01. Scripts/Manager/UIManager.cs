using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    // about UI
    public Canvas PlayerBattleUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BattleManager.instance.nowTurnID == 1)
        {
            PlayerBattleUI.transform.gameObject.SetActive(true);
        }
        else
        {
            PlayerBattleUI.transform.gameObject.SetActive(false);
        }
    }
}
