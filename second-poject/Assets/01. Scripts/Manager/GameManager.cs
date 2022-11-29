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
    public int HPportion = 0;
    public int MPPortion = 0;
 
    public void testclickGOldup()
    {
        GameManager.instance.Gold += 1000;
        Debug.Log(Gold);
    }
    

}
