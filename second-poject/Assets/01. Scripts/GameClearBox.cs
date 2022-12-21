using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.ClearDungeon();
        }
    }
}
