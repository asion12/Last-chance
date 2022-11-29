using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
  
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            if (instance != this) 
                Destroy(this.gameObject); 
        }
    }


    public int Gold = 0;
    public int Hp_0 = 0;
    public int HP_1 = 0;
    public int HP_2 = 0;
    public int MPPortion = 0;
    public int MP_0 = 0;
    public int MP_1 = 0;
    public int MP_2 = 0;

    public void testclickGOldup()
    {
        GameManager.instance.Gold += 1000;
        Debug.Log(Gold);
    }
    

}
