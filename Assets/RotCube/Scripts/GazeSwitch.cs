using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GazeSwitch : MonoBehaviour {

    private GameObject HoloLensCamera;
    private Vector3 _targetPos;
    private Vector3 _targetRotation;


    // Windows KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    private speech _speech;
    
    // Use this for initialization
    void Start () {
        // KeywordRecognizer
        keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("Cannon Come Here", MoveInCamera);
        //keywordCollection.Add("Switch", ToggleBulletSource);
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // Find Camera
        HoloLensCamera = GameObject.Find("HoloLensCamera");
        _targetPos = HoloLensCamera.transform.position;
        _targetRotation = HoloLensCamera.transform.rotation.eulerAngles;

    }

    // Update is called once per frame
    void Update () {
        transform.position = Vector3.Slerp(transform.position, _targetPos, Time.deltaTime);
        transform.Rotate(_targetRotation * Time.deltaTime, Space.World);
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void MoveInCamera(PhraseRecognizedEventArgs args)
    {
        _targetPos = HoloLensCamera.transform.position + (HoloLensCamera.transform.forward * 1);
        _targetRotation = Vector3.Scale(HoloLensCamera.transform.forward, new Vector3(1,0,1)).normalized;
    }

}
