using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioClip[] audioList;
    AudioSource a;
    private void Start()
    {
        a = GetComponent<AudioSource>();
    }
    public void play(int audioNum)
    {
        a.clip = audioList[audioNum];
        a.Play();
    }
}
