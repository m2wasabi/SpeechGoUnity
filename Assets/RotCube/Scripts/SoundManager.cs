using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public enum State { Stop, Busy, Loop }
    public State Status { get; private set; }

    private AudioSource _audioSource;
    public AudioClip[] Sounds;


    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1;
        _audioSource.dopplerLevel = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Play(int index)
    {
        if (Sounds.Length > index && Sounds[index] != null)
        {
            _audioSource.Stop();
            _audioSource.clip = Sounds[index];
            _audioSource.Play();
        }
    }
}
