using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Cursor = HoloToolkit.Unity.InputModule.Cursor;

public class SpaceTapInputManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip ChargeSound;
    public AudioClip FireSound;

    public AnimatedCursor _cursor;
    private Cursor.CursorStateEnum _cursorState;

    public TextMesh InformationMesh;

    private bool _isCharge = false;

    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        //InformationMesh.text = "CursorStare: " + (int)_cursor.CursorState;
        // 無空間注視:2 あたり注視:3 クリック:4
        if (_cursorState != _cursor.CursorState)
        {
            OnCursorChanged(_cursorState, _cursor.CursorState);
            _cursorState = _cursor.CursorState;
        }


    }

    private void OnCursorChanged(Cursor.CursorStateEnum oldVal, Cursor.CursorStateEnum newVal)
    {
        if (_isCharge == false && oldVal != Cursor.CursorStateEnum.Select && newVal == Cursor.CursorStateEnum.Select)
        {
            _isCharge = true;
            _audioSource.clip = ChargeSound;
            _audioSource.Play();
        }
        else if (_isCharge == true && oldVal == Cursor.CursorStateEnum.Select && newVal != Cursor.CursorStateEnum.Select)
        {
            _isCharge = false;
            _audioSource.clip = FireSound;
            _audioSource.Play();
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        _audioSource.clip = ChargeSound;
        _audioSource.Play();
    }

    public void OnInputUp(InputEventData eventData)
    {
        _audioSource.clip = FireSound;
        _audioSource.Play();
    }

}
