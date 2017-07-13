using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrostweepGames.SpeechRecognition.Utilites;
using FrostweepGames.SpeechRecognition.Google.Cloud;
using UnityEngine.VR.WSA.Input;
using System;

public class speech : MonoBehaviour
{
    public TextMesh InformationMesh;
    private ILowLevelSpeechRecognition _speechRecognition;
    //private InputField _contextPhrases;

    private ReactionManager _reactionManager;

    public GestureRecognizer InputActionRecognizer { get; private set; }

    public enum State { Stop, Recording, Analyzing }
    public State Status { get; private set; }

    // 仮
    private AudioSource _audioSource;
    public AudioClip ChargeSound;
    public AudioClip FireSound;
    public AudioClip ThinkSound;
    public AudioClip FailSound;

    // 仮

    void Awake()
    {
        InputActionRecognizer = new GestureRecognizer();
        InputActionRecognizer.SetRecognizableGestures(GestureSettings.Hold);

        InputActionRecognizer.HoldStartedEvent += InputActionRecognizer_HoldStartEvent;
        InputActionRecognizer.HoldCompletedEvent += InputActionRecognizer_HoldCompletedEvent;
    }

    private void InputActionRecognizer_HoldStartEvent(InteractionSourceKind source, Ray headRay)
    {
        _audioSource.clip = ChargeSound;
        _audioSource.Play();

        //StartRecordButtonOnClickHandler();
    }

    private void InputActionRecognizer_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {
        _audioSource.clip = FireSound;
        _audioSource.Play();
        //StopRecordButtonOnClickHandler();
    }

    // Use this for initialization
    void Start()
    {
        _speechRecognition = SpeechRecognitionModule.Instance;
        _speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
        _speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

        _reactionManager = ReactionManager.Instance;

        // 仮
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1;
        _audioSource.dopplerLevel = 0;
        // 仮
        Status = State.Stop;
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

    //private void ApplySpeechContextPhrases()
    //{
    //    string[] phrases = _contextPhrases.text.Trim().Split(","[0]);

    //    if (phrases.Length > 0)
    //        _speechRecognition.SetSpeechContext(phrases);
    //}

    private void SpeechRecognizedFailedEventHandler(string obj)
    {
        InformationMesh.text = "Speech Recognition failed with error: " + obj;
        _audioSource.clip = FailSound;
        _audioSource.Play();
        Status = State.Stop;
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
            SpeechReaction(obj);
        }
        else
        {
            InformationMesh.text = "Speech Recognition succeeded! Words are no detected.";
            _audioSource.clip = FailSound;
            _audioSource.Play();

        }
        Status = State.Stop;
    }

    private void SpeechReaction(RecognitionResponse obj)
    {
        _reactionManager.Action(obj);
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
        if (Status == State.Stop)
        {
            InformationMesh.text = "";
            _speechRecognition.StartRecord();
            Status = State.Recording;
        }
    }

    public void StopRecordButtonOnClickHandler()
    {
        if (Status == State.Recording)
        {
            //ApplySpeechContextPhrases();
            _speechRecognition.StopRecord();
            Status = State.Analyzing;
        }
    }
}