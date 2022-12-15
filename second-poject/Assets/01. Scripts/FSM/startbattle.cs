using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startbattle : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("fsdfsd");
            BattleManager.instance.BattleStart(false, false, gameObject);
        }
    }
}
