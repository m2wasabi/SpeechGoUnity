using System.Collections;
using System.Collections.Generic;
using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using UnityEngine;
using HoloToolkit.Unity;

public class ReactionManager : Singleton<ReactionManager>
{

    public AudioClip SpeechFeedbackSound;
    private AudioSource reactionAudioSource;
    public GameObject TextObject;
    public GameObject TextBombObject;
    public float BulletSpeed = 1000;
    public int BulletSourceIndex = 0;
    public GameObject[] BulletSources;

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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTextObject("Hello!", TextBombObject);
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

        if (text.IndexOf("爆発") != -1)
        {
            GenerateTextObject(text, TextBombObject);
        }
        else
        {
            GenerateTextObject(text, TextObject);
        }

        if (text.IndexOf("最高") != -1)
        {
            reactionAudioSource.Play();
        }
    }

    private void GenerateTextObject(string word, GameObject prefab)
    {
        GameObject wordBullet = GameObject.Instantiate(prefab);
        Vector3 force;
        force = BulletSources[BulletSourceIndex].transform.forward * BulletSpeed;
        wordBullet.GetComponent<Rigidbody>().AddForce(force);
        wordBullet.transform.position = BulletSources[BulletSourceIndex].transform.position;
        TextMesh _text = wordBullet.GetComponent<TextMesh>();
        _text.text = word;
    }
}
