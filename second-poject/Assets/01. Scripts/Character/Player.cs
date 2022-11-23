using UnityEngine;
using cakeslice;

public class Player : Character
{
    // about Battle Start Check
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private float battleStartRange = 0f;
    [SerializeField] private GameObject battleStartRangePoint = null;

    private GameObject hitObject = null;
    private GameObject lastHitData = null;

    // about move
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    private bool freeze = false;

    private float xRotate, yRotate, xRotateMove, yRotateMove;

    private UIManager uIManager = null;
    public bool[] isSkillOverClockList = { false, };

    float cpIncreaseTimer = 0;
    float cpIncreaseTimer_MAX = 2f;

    float cpDecreaseTimer = 0;
    float cpDecreaseTimer_MAX = 1f;
    protected virtual void Start()
    {
        base.Start();
        uIManager = FindObjectOfType<UIManager>();
    }

    protected override void Update()
    {
        base.Update();
        //Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.forward, Color.red);
        if (!isBattleMode || isStunned)
        {
            CheckIncreaseCP();
            CameraRotateToMousePointer();
            CharacterMove();
            CanBattleStartByRayCast();
        }
        else if (isBattleMode)
        {
            CheckDecreaseCP();
        }
        TargetCharacterRayCheck();
    }

    private void CheckIncreaseCP()
    {
        cpIncreaseTimer += Time.deltaTime;
        if (cpIncreaseTimer >= cpIncreaseTimer_MAX / ((100 + totalStats.CHA) / 100))
        {
            cpIncreaseTimer = 0;
            nowCP++;
            if (nowCP > maxCP)
            {
                nowCP = maxCP;
            }
        }
    }

    private void CheckDecreaseCP()
    {
        cpDecreaseTimer += Time.deltaTime;
        if (cpDecreaseTimer >= cpDecreaseTimer_MAX * ((100 + totalStats.FOC) / 100))
        {
            cpDecreaseTimer = 0;
            nowCP--;
            if (nowCP < 0)
            {
                nowCP = 0;
            }
        }
    }

    private void CheckOverClockAndAddWeakElement()
    {
        Elements_int tempEl_int = additionWeakElements;
        for (int i = 0; i < isSkillOverClockList.Length; i++)
        {
            if (isSkillOverClockList[i])
            {
                if (skillList[i].skillElements.SOLAR)
                {
                    tempEl_int.SOLAR++;
                }
                else if (skillList[i].skillElements.LUMINOUS)
                {
                    tempEl_int.LUMINOUS++;
                }
                else if (skillList[i].skillElements.IGNITION)
                {
                    tempEl_int.IGNITION++;
                }
                else if (skillList[i].skillElements.HYDRO)
                {
                    tempEl_int.HYDRO++;
                }
                else if (skillList[i].skillElements.BIOLOGY)
                {
                    tempEl_int.BIOLOGY++;
                }
                else if (skillList[i].skillElements.METAL)
                {
                    tempEl_int.METAL++;
                }
                else if (skillList[i].skillElements.CLAY)
                {
                    tempEl_int.CLAY++;
                }
            }
        }
        additionWeakElements = tempEl_int;
    }

    public void CameraRotateToTarget(GameObject target)
    {
        Vector3 rotateDistance = target.transform.position - playerCamera.transform.position;
        Quaternion toRotate = Quaternion.LookRotation(rotateDistance, Vector3.up);
        //toRotate = Quaternion.Euler(toRotate.x, toRotate.y, 0);
        playerCamera.transform.rotation = toRotate;
        //playerCamera.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, 0);
    }

    private void CameraRotateToMousePointer()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

        yRotate = playerCamera.transform.eulerAngles.y + yRotateMove;
        xRotate = xRotate + xRotateMove;

        xRotate = Mathf.Clamp(xRotate, -90, 90);

        transform.eulerAngles = new Vector3(0, yRotate, 0);
        playerCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    public GameObject nowHitObject;
    private void CanBattleStartByRayCast()
    {
        //Debug.Log("Now Raying");

        RaycastHit hitData;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData))
        {
            if (hitData.transform.tag == "Enemy")
            {
                if (uIManager == null)
                { Debug.Log("uIManager is null!"); }
                else
                {
                    GameObject temp = hitData.transform.gameObject;
                }
                //Debug.Log("RayIn!!");
                if (hitData.distance <= battleStartRange)
                {
                    lastHitData = hitData.transform.gameObject;
                    hitData.transform.GetComponent<Outline>().eraseRenderer = false;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Battle Ready");
                        bool checkStun = lastHitData.transform.GetComponent<Enemy>().isStunned;
                        bool checkCareless = false;
                        if (checkStun || checkCareless)
                            BattleManager.instance.BattleStart(true, true, lastHitData);
                        else
                            BattleManager.instance.BattleStart(true, false, lastHitData);
                    }
                }
                else
                {
                    RayOutCheck();
                }
            }
            else
            {
                RayOutCheck();
            }
            // GameObject hitObj;
            // hitObj = Instantiate(hitObject, hitData.point, Quaternion.identity);
            // hitObj.transform.SetParent(null);
        }
        else
        {
            RayOutCheck();
        }
        // Debug.Log("last hit is " + lastHitData);
        // Debug.Log("now hit is" + hitData.transform.gameObject);
    }

    private void TargetCharacterRayCheck()
    {
        RaycastHit hitData;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData))
        {
            if (hitData.transform.tag == "Enemy")
            {
                if (uIManager == null)
                { Debug.Log("uIManager is null!"); }
                else
                {
                    GameObject temp = hitData.transform.gameObject;
                    uIManager.UIUpdate_TargetEnemyBase(temp, true);
                }
            }
            else
            {
                uIManager.UIUpdate_OffTargetEnemyBase();
            }
        }
        else
        {
            uIManager.UIUpdate_OffTargetEnemyBase();
        }
    }

    private void RayOutCheck()
    {
        //Debug.Log("Ray Checking");
        if (lastHitData != null)
        {
            Debug.Log("RayOut!!");
            lastHitData.transform.GetComponent<Outline>().eraseRenderer = true;
        }
        lastHitData = null;
    }

    private void CharacterMove()
    {
        float moveX = Input.GetAxisRaw("Vertical");
        float moveY = Input.GetAxisRaw("Horizontal");

        var pos = transform.position;

        pos += transform.forward * moveX * moveSpeed * Time.deltaTime;
        pos += transform.right * moveY * moveSpeed * Time.deltaTime;

        transform.position = pos;
    }
}