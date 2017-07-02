using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrostweepGames.SpeechRecognition.Google.Cloud;
using HoloToolkit.Unity;

public class ReactionManager : Singleton<ReactionManager>
{

    public AudioClip SpeechFeedbackSound;
    private AudioSource reactionAudioSource;
    public GameObject TextObject;
    private GameObject Camera;
    public float BulletSpeed = 1000;

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

        Camera = GameObject.Find("HoloLensCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTextObject("Hello!");
        }
    }

    public void Action (RecognitionResponse obj)
    {
        string text = obj.results[0].alternatives[0].transcript;
        //RaycastHit hit;

        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        //{
        //    Instantiate(TextObject, hit.point, Quaternion.identity);
        //}

        GenerateTextObject(text);

        if (text.IndexOf("最高") != -1)
        {
            reactionAudioSource.Play();
        }
    }

    private void GenerateTextObject(string word)
    {
        GameObject wordBullet = GameObject.Instantiate(TextObject);
        Vector3 force;
        force = Camera.transform.forward * BulletSpeed;
        wordBullet.GetComponent<Rigidbody>().AddForce(force);
        wordBullet.transform.position = Camera.transform.position;
        TextMesh _text = wordBullet.GetComponent<TextMesh>();
        _text.text = word;
    }
}
