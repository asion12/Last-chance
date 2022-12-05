using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.NonSerialized] public bool isGameStarted = true;
    public GameObject playerStartPoint;
    private Player player;

    public static GameManager instance = null;
    private void Awake()
    {
        ActiveDungeon();
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
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (BattleManager.instance.nowTurnID == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
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

    public void ActiveDungeon()
    {
        SceneManager.LoadScene("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void ResetDungeon()
    {
        player.transform.position = playerStartPoint.transform.position;
        SceneManager.UnloadSceneAsync("JJB-Dungeon");
        SceneManager.LoadScene("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void ExitDungeon()
    {

    }
}
