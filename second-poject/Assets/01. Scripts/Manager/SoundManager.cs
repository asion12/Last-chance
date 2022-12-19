using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    //public AudioSource bgSource;
    //public AudioClip[] bgList;
    public AudioSource backSource;
    public AudioClip[] backList;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
           
        }else if (_instance != true)
        {
            Destroy(gameObject);
        }
    }
    //private void Onsceneloded(SO_Skill useSkil)
    //{
    //    for (int i=0;i<bgList.Length;i++)
    //    {
    //        if (useSkil.name == bgList[i].name)
    //        {
    //            SonudPlay(bgList[i]);
    //        }
    //    }
    //}
    public void SonudPlay(AudioClip clip)
    {
        backSource.clip = clip;
        backSource.Play();
    }
    public void backgroundSoundpl(bool startbattle)
    { 
        if (startbattle)
        {
            Debug.Log(backSource);
            Debug.Log(backSource.clip);
            Debug.Log(backList[0]);
            SonudPlay(backList[0]);
            Debug.Log("XsS");
        }
        else
        {
            Debug.Log(backSource);
            Debug.Log(backSource.clip);
            Debug.Log(backList[1]);
            SonudPlay(backList[1]);
            Debug.Log("XsS");
        }
    }
    public void stopback(AudioClip clip)
    {
        backSource.clip = clip;
        backSource.Stop();    
    }
    public void changebg()
    {
        SonudPlay(backList[Random.Range(0,2)]);
    }
    public void mora()
    {
        SonudPlay(backList[2]);
    }


}
