using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance;
    public AudioSource backSource;
    public AudioClip[] backList;

    public AudioSource attack;
    public AudioClip[] attackList;


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

    public void SonudPlay(AudioClip clip)
    {
        backSource.clip = clip;
        backSource.Play();
    }
   
 
    public void changebg()//랜덤 실행 내부
    {
 
        SonudPlay(backList[Random.Range(0,2)]);
        StartCoroutine(FadeIn(backSource, 1f));

    }
    public void mora()//원상복귀 상점으로
    {
        SonudPlay(backList[2]);
    }
    public void changeat()//전투신
    {
       
        SonudPlay(attackList[Random.Range(0, attackList.Length)]);
        StartCoroutine(FadeIn(attack, 1f));
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
