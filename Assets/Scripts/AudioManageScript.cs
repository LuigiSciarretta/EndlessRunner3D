using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManageScript : MonoBehaviour
{

    [SerializeField] private AudioClip audioIntro;
    [SerializeField] private AudioClip audioRunning;
    [SerializeField] private AudioClip audioGameOver;

    private AudioSource audioSource;
    
    public static AudioManageScript current; //per interfecciare con altri script, in moda da richiamarla

    private void Awake() //la void viene letta quando il gioco parte
    {
        current = this; //è un istanza di questo script, serve per interfacciare altri script con questo
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioIntro;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.clip = clip;

        //if (clip.name == "diamondFx")
        //{
        //    audioSource.volume = 0.3f;
        //}
        //else
        //{
        //    audioSource.volume = 0.5f;
        //}
        //audioSource.Play();

        audioSource.PlayOneShot(clip);
    }

    public void PlayRunning()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioRunning;
        audioSource.Play();
    }

    public void PlayGameOver()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioGameOver;
        audioSource.loop = false;
        audioSource.Play();
    }
}
