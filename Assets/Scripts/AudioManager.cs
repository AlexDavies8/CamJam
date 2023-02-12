using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<IDClip> clips = new();
    private AudioSource _src;

    private void Awake()
    {
        _src = GetComponent<AudioSource>();
    }

    public void PlaySound(string id, float volume = 1.0f)
    {
        _src.PlayOneShot(clips.Find(x => x.id == id).clip, volume);
    }

    [System.Serializable]
    public struct IDClip
    {
        public string id;
        public AudioClip clip;
    }
}
