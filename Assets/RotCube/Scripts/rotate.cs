using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour, IFocusable
{

    public float Speed = 100.0f;
    private bool isRotate = false;

    public AudioClip TargetFeedbackSound;
    private AudioSource audioSource;

    public AudioClip TargetFeedbackSound2;

    public Vector3 RotateVector3;

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
    }

    // Update is called once per frame
    void Update () {
        if (isRotate)
        {
            transform.Rotate(RotateVector3 * Time.deltaTime * Speed);
        }
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
}
