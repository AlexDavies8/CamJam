using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<IDClip> clips = new();
    private AudioSource _src;

    private void Awake()
    {
        _src = GetComponent<AudioSource>();
    }

    public void PlayMusic(string id)
    {
        _src.Stop();
        _src.clip = clips.Find(x => x.id == id).clip;
        _src.Play();
    }
    
    [System.Serializable]
    public struct IDClip
    {
        public string id;
        public AudioClip clip;
    }
}
