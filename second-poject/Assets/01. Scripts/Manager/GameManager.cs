using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int potion = 0;
    public void testclickGOldup()
    {
        GameManager.instance.Gold += 1000;
        Debug.Log(Gold);
    }
    public void SellItem()
    {
        if (GameManager.instance.potion>=1)
        {
            GameManager.instance.potion -= 1;
            GameManager.instance.Gold += 1000;
        }
      
    }
}
