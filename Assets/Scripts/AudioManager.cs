using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] Sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;
        }
    }
    private void Start()
    {
        Play("Main");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
        s.source.Play();
        
    }
}
