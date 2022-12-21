using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    // public AudioSource bgSource;
    // public AudioClip[] bgList;
    public AudioSource backSource;
    public AudioClip[] backList;

    public AudioSource attack;
    public AudioClip[] attackList;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

        }
        else if (_instance != true)
        {
            Destroy(gameObject);
        }
    }
    // public void Onsceneloded(GameObject useSkil)
    // {
    //     for (int i = 0; i < bgList.Length; i++)
    //     {
    //         if (useSkil.name == bgList[i].name)
    //         {
    //             SonudPlay(bgList[i]);
    //         }
    //     }
    // }
    public void SonudPlay(AudioClip clip)
    {
        backSource.clip = clip;
        backSource.Play();
    }
    // public void SonudPlayef(AudioClip clip)
    // {
    //     bgSource.clip = clip;
    //     bgSource.Play();
    // }

    public void changebg()//���� ����
    {
        SonudPlay(backList[Random.Range(0, 2)]);
    }
    public void mora()//���󺹱�
    {
        SonudPlay(backList[2]);
    }
    public void changeat()
    {
        SonudPlay(attackList[Random.Range(0, attackList.Length)]);
    }

}
