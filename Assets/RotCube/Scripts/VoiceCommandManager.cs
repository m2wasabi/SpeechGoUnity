using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommandManager : MonoBehaviour {

    // Windows KeywordRecognizer
    private KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    private GazeSwitch _gazeSwitch;

    // Use this for initialization
    void Start () {
        _gazeSwitch = GameObject.Find("MagicCube").GetComponent<GazeSwitch>();

        // KeywordRecognizer
        keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("Cannon Come Here", _gazeSwitch.MoveInCamera);
        //keywordCollection.Add("Switch", ToggleBulletSource);
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }


}
