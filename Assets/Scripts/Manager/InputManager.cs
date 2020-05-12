using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _inputManager;
    // 是否接受输入
    private bool acceptInput = true;
    // 是否处于对话状态
    private bool isTalking = false;
    
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
        buttonTable.Add("Interact", KeyCode.F);
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

    // 是否接受输入
    public static bool CanInput()
    {
        return _inputManager.acceptInput;
    }

    // 设置是否接受输入
    public static void SetInputStatus(bool state)
    {
        _inputManager.acceptInput = state;
    }

    public static void EnterTalkingState()
    {
        _inputManager.acceptInput = false;
        _inputManager.isTalking = true;
    }
    
    public static void ExitTalkingState()
    {
        _inputManager.acceptInput = true;
        _inputManager.isTalking = false;
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
        if( _inputManager.isTalking && keyCode == KeyCode.F) return Input.GetKeyDown(keyCode);
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

    public static bool GetMouseButtonDown(int button)
    {
        return _inputManager.acceptInput && Input.GetMouseButtonDown(button);
    }
    
    public static bool GetMouseButton(int button)
    {
        return _inputManager.acceptInput && Input.GetMouseButton(button);
    }
    
    public static bool GetMouseButtonUp(int button)
    {
        return _inputManager.acceptInput && Input.GetMouseButtonUp(button);
    }
}
