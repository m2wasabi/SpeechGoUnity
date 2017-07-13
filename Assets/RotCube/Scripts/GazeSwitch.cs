using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GazeSwitch : MonoBehaviour {

    private GameObject HoloLensCamera;
    private Vector3 _targetPos;
    private Vector3 _targetLookAt;

    // clolor material
    public Material[] Materials;
    private int _materialIndex = 0;
    public GameObject SwitchObject;

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
        _targetPos = HoloLensCamera.transform.position + (HoloLensCamera.transform.TransformDirection(Vector3.forward) * 2);
        _targetLookAt = HoloLensCamera.transform.position + (HoloLensCamera.transform.TransformDirection(Vector3.forward) * 4);

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            MoveInCamera(new PhraseRecognizedEventArgs());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchOn();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchOff();
        }

        transform.position = Vector3.Slerp(transform.position, _targetPos, Time.deltaTime);
        transform.LookAt(_targetLookAt);
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
        _targetPos = HoloLensCamera.transform.position + (HoloLensCamera.transform.TransformDirection(Vector3.forward) * 2);
        _targetLookAt = HoloLensCamera.transform.position + (HoloLensCamera.transform.TransformDirection(Vector3.forward) * 4);
    }

    public void SwitchOn()
    {
        Renderer _renderer = SwitchObject.GetComponent<Renderer>();
        _renderer.material = Materials[1];
    }

    public void SwitchOff()
    {
        Renderer _renderer = SwitchObject.GetComponent<Renderer>();
        _renderer.material = Materials[0];
    }

}
