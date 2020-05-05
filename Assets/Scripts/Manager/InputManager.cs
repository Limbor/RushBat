using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _inputManager;

    private bool acceptInput = true;
    private Hashtable buttonTable = new Hashtable();
    private void Awake()
    {
        if (_inputManager == null)
        {
            _inputManager = this;
            _inputManager.InitButtonTable();
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void InitButtonTable()
    {
        buttonTable.Add("Jump", KeyCode.Space);
        buttonTable.Add("Dash", KeyCode.LeftShift);
        buttonTable.Add("Skill2", KeyCode.Q);
        buttonTable.Add("Skill3", KeyCode.E);
        buttonTable.Add("Skill4", KeyCode.R);
    }

    public static void SetButton(String buttonName, KeyCode keyCode)
    {
        if (_inputManager.buttonTable.Contains(buttonName))
        {
            _inputManager.buttonTable[buttonName] = keyCode;
        }
    }

    public static KeyCode GetKeyCode(String button)
    {
        if (_inputManager.buttonTable.Contains(button))
        {
            return _inputManager.buttonTable[button] is KeyCode ? (KeyCode) _inputManager.buttonTable[button] : KeyCode.None;
        }
        return KeyCode.None;
    }

    public static bool CanInput()
    {
        return _inputManager.acceptInput;
    }

    public static void SetInputStatus(bool state)
    {
        _inputManager.acceptInput = state;
    }

    public static bool GetKeyDown(KeyCode keyCode)
    {
        return _inputManager.acceptInput && Input.GetKeyDown(keyCode);
    }
    
    public static bool GetKey(KeyCode keyCode)
    {
        return _inputManager.acceptInput && Input.GetKey(keyCode);
    }
    
    public static bool GetKeyUp(KeyCode keyCode)
    {
        return _inputManager.acceptInput && Input.GetKeyUp(keyCode);
    }

    public static float GetAxis(String axis)
    {
        return _inputManager.acceptInput ? Input.GetAxis(axis) : 0;
    }
    
    public static float GetAxisRaw(String axis)
    {
        return _inputManager.acceptInput ? Input.GetAxisRaw(axis) : 0;
    }

    public static bool GetButtonDown(String button)
    {
        if (!_inputManager.buttonTable.Contains(button)) return false;
        var keyCode = _inputManager.buttonTable[button] is KeyCode ? (KeyCode) _inputManager.buttonTable[button] : KeyCode.None;
        return _inputManager.acceptInput && Input.GetKeyDown(keyCode);
    }
    
    public static bool GetButton(String button)
    {
        if (!_inputManager.buttonTable.Contains(button)) return false;
        var keyCode = _inputManager.buttonTable[button] is KeyCode ? (KeyCode) _inputManager.buttonTable[button] : KeyCode.None;
        return _inputManager.acceptInput && Input.GetKey(keyCode);
    }
    
    public static bool GetButtonUp(String button)
    {
        if (!_inputManager.buttonTable.Contains(button)) return false;
        var keyCode = _inputManager.buttonTable[button] is KeyCode ? (KeyCode) _inputManager.buttonTable[button] : KeyCode.None;
        return _inputManager.acceptInput && Input.GetKeyUp(keyCode);
    }
}
