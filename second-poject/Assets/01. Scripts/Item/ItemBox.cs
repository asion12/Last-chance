using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    GameObject player;
    bool isPlayerEnter;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isPlayerEnter = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerEnter && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("¿­·Á¶ó");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = false;
        }
    }
}
