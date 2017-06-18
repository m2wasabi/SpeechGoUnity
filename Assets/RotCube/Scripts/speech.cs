﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrostweepGames.SpeechRecognition.Utilites;
using FrostweepGames.SpeechRecognition.Google.Cloud;

public class speech : MonoBehaviour
{
    public TextMesh InformationMesh;
    private ILowLevelSpeechRecognition _speechRecognition;
    private InputField _contextPhrases;

    public AudioClip SpeechFeedbackSound;
    public AudioSource reactionAudioSource;

    // Use this for initialization
    void Start()
    {
        _speechRecognition = SpeechRecognitionModule.Instance;
        _speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

        if (SpeechFeedbackSound != null)
        {
            /*
            reactionAudioSource = GetComponent<AudioSource>();
            if (reactionAudioSource == null)
            {
                reactionAudioSource = gameObject.AddComponent<AudioSource>();
            }
            */
            reactionAudioSource.clip = SpeechFeedbackSound;
            reactionAudioSource.playOnAwake = false;
            reactionAudioSource.priority = 1;
            reactionAudioSource.spatialBlend = 1;
            reactionAudioSource.dopplerLevel = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        _speechRecognition.SpeechRecognizedSuccessEvent -= SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent -= SpeechRecognizedFailedEventHandler;
    }
    private void IsRuntimeDetectionOnValueChangedHandler(bool value)
    {
        StopRuntimeDetectionButtonOnClickHandler();

        (_speechRecognition as SpeechRecognitionModule).isRuntimeDetection = value;
    }

    private void ApplySpeechContextPhrases()
    {
        string[] phrases = _contextPhrases.text.Trim().Split(","[0]);

        if (phrases.Length > 0)
            _speechRecognition.SetSpeechContext(phrases);
    }

    private void SpeechRecognizedFailedEventHandler(string obj)
    {
        InformationMesh.text = "Speech Recognition failed with error: " + obj;
    }

    private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj)
    {
        if (obj != null && obj.results.Length > 0)
        {
            // InformationMesh.text = "Speech Recognition succeeded! Detected Most useful: " + obj.results[0].alternatives[0].transcript;
            InformationMesh.text = "" + obj.results[0].alternatives[0].transcript;
            /*
            string other = "\nDetected alternative: ";

            foreach (var result in obj.results)
            {
                foreach (var alternative in result.alternatives)
                {
                    if (obj.results[0].alternatives[0] != alternative)
                        other += alternative.transcript + ", ";
                }
            }

            InformationMesh.text += other;
            */
            SpeechReaction();
        }
        else
        {
            InformationMesh.text = "Speech Recognition succeeded! Words are no detected.";

        }
    }

    private void SpeechReaction()
    {
        if (InformationMesh.text.IndexOf("最高") != -1)
        {
            reactionAudioSource.Play();
        }
    }

    private void StartRuntimeDetectionButtonOnClickHandler()
    {
        //ApplySpeechContextPhrases();

        InformationMesh.text = "";
        _speechRecognition.StartRuntimeRecord();
    }

    private void StopRuntimeDetectionButtonOnClickHandler()
    {
        _speechRecognition.StopRuntimeRecord();
        InformationMesh.text = "";
    }

    public void StartRecordButtonOnClickHandler()
    {
        InformationMesh.text = "";
        _speechRecognition.StartRecord();
    }

    public void StopRecordButtonOnClickHandler()
    {
        //ApplySpeechContextPhrases();

        _speechRecognition.StopRecord();

    }
}