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
    public Canvas ui;
    private bool canvas = false;
    public float nowTimeLimit = 0;
    public float maxTimeLimit = 180;
    public bool isTimeLimitOver = false;
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
        nowTimeLimit += Time.deltaTime;
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
        CursorLook();
        TimeLimitCheck();
    }

    private void TimeLimitCheck()
    {
        if (isGameStarted)
        {
            nowTimeLimit += Time.deltaTime;
        }
        if (nowTimeLimit >= maxTimeLimit)
        {
            isTimeLimitOver = true;
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
    public void CursorLook()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (ui.gameObject.activeSelf)
            {
                ui.gameObject.SetActive(false);
            }
            else
            {
                ui.gameObject.SetActive(true);
            }
        }
    }
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
        isGameStarted = false;
        isTimeLimitOver = false;
        nowTimeLimit = 0;
        player.transform.position = playerStartPoint.transform.position;
        player.nowHP = player.characterStats.MAX_HP;
        player.nowMP = player.characterStats.MAX_MP;
        player.nowCP = player.maxCP;
        player.GetComponent<Player>().isCanExit = false;
        SceneManager.UnloadSceneAsync("JJB-Dungeon");
        SceneManager.LoadScene("JJB-Dungeon", LoadSceneMode.Additive);
    }

    public void ExitDungeon()
    {

    }
}
