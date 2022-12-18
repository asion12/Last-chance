using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bgSource;
    public AudioClip[] bgList;
    public AudioSource backSource;
    public AudioClip[] backList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           
        }else if (instance != true)
        {
            Destroy(gameObject);
        }
    }
    private void Onsceneloded(SO_Skill useSkil)
    {
        for (int i=0;i<bgList.Length;i++)
        {
            if (useSkil.name == bgList[i].name)
            {
                SonudPlay(bgList[i]);
            }
        }
    }
    public void SonudPlay(AudioClip clip)
    {
        bgSource.clip = clip;
        bgSource.loop = true;
        bgSource.volume = 0.1f;
        bgSource.Play();
    }
    public void backgroundSoundpl(bool startbattle)
    {
        if (startbattle==true)
        {
            SonudPlay(backList[0]);
            stopback(backList[1]);
        }
        else
        {
            SonudPlay(backList[0]);
            stopback(backList[1]);
        }
    }
    public void stopback(AudioClip clip)
    {
        backSource.clip = clip;
        backSource.Stop();
    }
}
