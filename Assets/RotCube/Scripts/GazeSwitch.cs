using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GazeSwitch : MonoBehaviour, IFocusable
{

    private GameObject HoloLensCamera;
    private Vector3 _targetPos;
    private Vector3 _targetLookAt;

    // Stat of Switch
    public enum SwitchState { Off, On }
    public SwitchState SwitchStatus { get; private set; }

    // clolor material
    public Material[] Materials;
    private int _materialIndex = 0;
    public GameObject SwitchObject;

    // Windows KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    // Action Driver
    private speech _speech;
    private SoundManager _soundManager;
    private ReactionManager _reactioManager;


    // Use this for initialization
    void Start () {
        _speech = GameObject.Find("InputManager").GetComponent<speech>();
        _soundManager = GetComponent<SoundManager>();
        _reactioManager = GameObject.Find("InputManager").GetComponent<ReactionManager>();

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

        SwitchStatus = SwitchState.Off;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            MoveInCamera(new PhraseRecognizedEventArgs());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            EffectSwitchOn();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            EffectSwitchOff();
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

    public void EffectSwitchOn()
    {
        Renderer _renderer = SwitchObject.GetComponent<Renderer>();
        _renderer.material = Materials[1];
    }

    public void EffectSwitchOff()
    {
        Renderer _renderer = SwitchObject.GetComponent<Renderer>();
        _renderer.material = Materials[0];
    }

    public void OnFocusEnter()
    {
        if (SwitchStatus == SwitchState.Off && _speech.Status == speech.State.Stop)
        {
            EffectSwitchOn();
            _soundManager.Play(0);
            _speech.StartRecordButtonOnClickHandler();
            _reactioManager.BulletSourceIndex = 1;

            SwitchStatus = SwitchState.On;
        }
    }
    public void OnFocusExit()
    {
        if (SwitchStatus == SwitchState.On)
        {
            if (_speech.Status == speech.State.Recording)
            {
                _speech.StopRecordButtonOnClickHandler();
            }
            _soundManager.Play(1);
            EffectSwitchOff();
            SwitchStatus = SwitchState.Off;
        }
    }

}
