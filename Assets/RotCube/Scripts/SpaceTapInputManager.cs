using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Cursor = HoloToolkit.Unity.InputModule.Cursor;

public class SpaceTapInputManager : MonoBehaviour
{
    public enum TapSpeechState { Stop, Active }
    public TapSpeechState TapSpeechStatus { get; private set; }

    private AnimatedCursor _cursor;
    private CursorStateEnum _cursorState;

    public TextMesh InformationMesh;

    private bool _isCharge = false;

    // Action Driver
    private speech _speech;
    private SoundManager _soundManager;
    private ReactionManager _reactioManager;

    // Use this for initialization
    void Start () {
        _speech = GameObject.Find("InputManager").GetComponent<speech>();
        _soundManager = GetComponent<SoundManager>();
        _reactioManager = GameObject.Find("InputManager").GetComponent<ReactionManager>();

        _cursor = GameObject.Find("DefaultCursor").GetComponent<AnimatedCursor>();

        TapSpeechStatus = TapSpeechState.Stop;
    }

    // Update is called once per frame
    void Update ()
    {
        //InformationMesh.text = "CursorStare: " + (int)_cursor.CursorState;
        // 無空間注視:2 あたり注視:3 クリック:4
        if (_cursor.CursorState != CursorStateEnum.None && _cursorState != _cursor.CursorState)
        {
            OnCursorChanged(_cursorState, _cursor.CursorState);
            _cursorState = _cursor.CursorState;
        }


    }

    private void OnCursorChanged(CursorStateEnum oldVal, CursorStateEnum newVal)
    {
        InformationMesh.text = "CurSor old:" + oldVal + " NewVal:" + newVal;
        if (_isCharge == false && oldVal != CursorStateEnum.Select && newVal == CursorStateEnum.Select)
        {
            if (TapSpeechStatus == TapSpeechState.Stop && _speech.Status == speech.State.Stop)
            {
                _soundManager.Play(0);
                _speech.StartRecordButtonOnClickHandler();
                _reactioManager.BulletSourceIndex = 0;

                TapSpeechStatus = TapSpeechState.Active;
            }
            else
            {
                _soundManager.Play(2);
            }
            _isCharge = true;
        }
        else if (_isCharge == true && oldVal == CursorStateEnum.Select && newVal != CursorStateEnum.Select)
        {
            if (TapSpeechStatus == TapSpeechState.Active && _speech.Status == speech.State.Recording)
            {
                _speech.StopRecordButtonOnClickHandler();
                TapSpeechStatus = TapSpeechState.Stop;
            }
            _isCharge = false;
            _soundManager.Play(3);
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (TapSpeechStatus == TapSpeechState.Stop && _speech.Status == speech.State.Stop)
        {
            _speech.StartRecordButtonOnClickHandler();
            _reactioManager.BulletSourceIndex = 0;

            TapSpeechStatus = TapSpeechState.Active;
        }
        else
        {
            _soundManager.Play(2);
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (TapSpeechStatus == TapSpeechState.Active && _speech.Status == speech.State.Recording)
        {
            _speech.StopRecordButtonOnClickHandler();
        }
        _soundManager.Play(3);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //
    }
}
