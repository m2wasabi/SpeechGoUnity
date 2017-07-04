using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class rotate : MonoBehaviour, IFocusable
{

    public float Speed = 100.0f;
    private bool isRotate = false;

    public AudioClip TargetFeedbackSound;
    private AudioSource audioSource;

    public AudioClip TargetFeedbackSound2;

    public Vector3 RotateVector3;


    private GameObject HoloLensCamera;
    private Vector3 _targetPos;

    // Windows KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    private speech _speech;
	// Use this for initialization
	void Start () {
        _speech = GetComponent<speech>();

        if(TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }

        // KeywordRecognizer
        keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("Cube Come Here", CubeMoveInCamera);
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // Find Camera
        HoloLensCamera = GameObject.Find("HoloLensCamera");
        _targetPos = HoloLensCamera.transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (isRotate)
        {
            transform.Rotate(RotateVector3 * Time.deltaTime * Speed);
        }

        transform.position = Vector3.Slerp(transform.position, _targetPos, Time.deltaTime);
    }

    void BeginRotate()
    {
        if(audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = TargetFeedbackSound;
            audioSource.Play();
        }
        this.RotateVector3 = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)).normalized;
        isRotate = true;
    }

    void StopRotate()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = TargetFeedbackSound2;
            audioSource.Play();
        }
        isRotate = false;
    }

    public void OnFocusEnter()
    {
        if (!isRotate)
        {
            BeginRotate();
            _speech.StartRecordButtonOnClickHandler();
        }
    }
    public void OnFocusExit()
    {
        if (isRotate)
        {
            _speech.StopRecordButtonOnClickHandler();
            StopRotate();
        }
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void CubeMoveInCamera(PhraseRecognizedEventArgs args)
    {
        _targetPos = HoloLensCamera.transform.position + (HoloLensCamera.transform.forward * 1);
    }
}
