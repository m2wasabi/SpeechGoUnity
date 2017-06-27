using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrostweepGames.SpeechRecognition.Google.Cloud;
using HoloToolkit.Unity;

public class ReactionManager : Singleton<ReactionManager>
{

    public AudioClip SpeechFeedbackSound;
    private AudioSource reactionAudioSource;

    // Use this for initialization
    void Start () {

        if (SpeechFeedbackSound != null)
        {
            reactionAudioSource = GetComponent<AudioSource>();
            if (reactionAudioSource == null)
            {
                reactionAudioSource = gameObject.AddComponent<AudioSource>();
            }
            reactionAudioSource.clip = SpeechFeedbackSound;
            reactionAudioSource.playOnAwake = false;
            reactionAudioSource.priority = 1;
            reactionAudioSource.spatialBlend = 1;
            reactionAudioSource.dopplerLevel = 0;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Action (RecognitionResponse obj)
    {
        string text = obj.results[0].alternatives[0].transcript;

        if (text.IndexOf("最高") != -1)
        {
            reactionAudioSource.Play();
        }
    }
}
