using UnityEngine;
using cakeslice;

public class PlayerControl : MonoBehaviour
{
    public GameObject battleStartRangePoint = null;
    public GameObject hitObject = null;

    [SerializeField]
    private Camera playerCamera = null;
    [SerializeField]
    private float rotateSpeed = 500.0f;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float battleStartRange = 10f;
    private GameObject lastHitData = null;
    //ī�޶� ȸ�� ��ġ
    private float xRotate, yRotate, xRotateMove, yRotateMove;


    private void Start()
    {

    }

    void Update()
    {
        //Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.forward, Color.red);
        CameraRotate();
        CharacterMove();
        CanBattleStartByRayCast();
    }

    private void CameraRotate()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

        yRotate = playerCamera.transform.eulerAngles.y + yRotateMove;
        xRotate = xRotate + xRotateMove;

        xRotate = Mathf.Clamp(xRotate, -90, 90); // ��, �Ʒ� ����

        transform.eulerAngles = new Vector3(0, yRotate, 0); //ĳ���� Y�� ȸ��
        playerCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    private void CanBattleStartByRayCast()
    {
        Debug.Log("Now Raying");

        RaycastHit hitData;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData))
        {
            if (hitData.transform.tag == "Enemy")
            {
                Debug.Log("RayIn!!");
                lastHitData = hitData.transform.gameObject;
                hitData.transform.GetComponent<Outline>().eraseRenderer = false;
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

    private void RayOutCheck()
    {
        Debug.Log("Ray Checking");
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