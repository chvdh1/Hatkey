using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags);


    // user32.dll에서 가져온 keybd_event 함수
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    private const int KEYEVENTF_KEYDOWN = 0x0000;// 키를 누르는 상수 값
    private const int KEYEVENTF_KEYUP = 0x0002; // 키를 떼는 상수 값

    const uint LBUTTONDOWN = 0x0002;    // 왼쪽 마우스 버튼 누름
    const uint LBUTTONUP = 0x0004;      // 왼쪽 마우스 버튼 땜

    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;
   
    public Text num;
    public Text com;
    public string keybd;

    public int mx;
    public int my;
    public float time;

    public int mouse;
    public bool move;
    public bool click;
    public bool m_Down;
    public bool m_Up;
    public bool key;
    public bool k_Down;
    public bool k_Up;
    public bool delays;

    public HatKeyMAnager manager;

    private void Awake()
    {
        num = transform.GetChild(0).GetChild(0).GetComponent<Text>();

        com = transform.GetChild(1).GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        manager = HatKeyMAnager.manager;
    }
    public void NumCK(string numck)
    {
        num.text = numck;
    }

    public void ComCK(string comck)
    {
        com.text = comck;
    }

    public void MoveCursor()
    {
        SetCursorPos(mx, my);
    }

    public void MouseEvent()
    {
        mouse_event(LBUTTONDOWN);
        mouse_event(LBUTTONUP);
    }
    public void MouseDown()
    {
        if(mouse == 0)
            keybd_event(VK_LBUTTON, 0, KEYEVENTF_KEYDOWN, 0);
        else
            keybd_event(VK_RBUTTON, 0, KEYEVENTF_KEYDOWN, 0);
    }
    public void MouseUp()
    {
        if (mouse == 0)
            keybd_event(VK_LBUTTON, 0, KEYEVENTF_KEYUP, 0);
        else
            keybd_event(VK_RBUTTON, 0, KEYEVENTF_KEYUP, 0);
    }

    // 매크로 실행 함수
    public void KeyMacro()
    {
        // keybd_event 함수를 사용하여 키를 누르고 떼는 매크로를 실행합니다.
        keybd_event(Key(), 0, KEYEVENTF_KEYDOWN, 0);
        keybd_event(Key(), 0, KEYEVENTF_KEYUP, 0);
    }

    public void KeyDown()
    {
        keybd_event(Key(), 0, KEYEVENTF_KEYDOWN, 0);
    }
    public void KeyUp()
    {
        keybd_event(Key(), 0, KEYEVENTF_KEYUP, 0);
    }

    public void CL()
    {
        mx = 0;
        my = 0;
        mouse = 0;
        time = 0.33f;
        num.text = "";
        com.text = "";

        move = false;
        click = false;
        key = false;
        delays = false;

        m_Down = false;
        m_Up = false;
        k_Down = false;
        k_Up = false;

    }

    public void del()
    {
        manager.Del();
    }

    byte Key()
    {
        byte key = 0;

        switch(keybd)
        {
            case "A":
                key = 0x41;
                break;
            case "B":
                key = 0x42;
                break;
            case "C":
                key = 0x43;
                break;
            case "D":
                key = 0x44;
                break;
            case "E":
                key = 0x45;
                break;
            case "F":
                key = 0x46;
                break;
            case "G":
                key = 0x47;
                break;
            case "H":
                key = 0x48;
                break;
            case "I":
                key = 0x49;
                break;
            case "J":
                key = 0x4A;
                break;
            case "K":
                key = 0x4B;
                break;
            case "L":
                key = 0x4C;
                break;
            case "M":
                key = 0x4D;
                break;
            case "N":
                key = 0x4E;
                break;
            case "O":
                key = 0x4F;
                break;
            case "P":
                key = 0x50;
                break;
            case "Q":
                key = 0x51;
                break;
            case "R":
                key = 0x52;
                break;
            case "S":
                key = 0x53;
                break;
            case "T":
                key = 0x54;
                break;
            case "U":
                key = 0x55;
                break;
            case "V":
                key = 0x56;
                break;
            case "W":
                key = 0x57;
                break;
            case "X":
                key = 0x58;
                break;
            case "Y":
                key = 0x59;
                break;
            case "Z":
                key = 0x5A;
                break;

            case "a":
                key = 0x61;
                break;
            case "b":
                key = 0x62;
                break;
            case "c":
                key = 0x63;
                break;
            case "d":
                key = 0x64;
                break;
            case "e":
                key = 0x65;
                break;
            case "f":
                key = 0x66;
                break;
            case "g":
                key = 0x67;
                break;
            case "h":
                key = 0x68;
                break;
            case "i":
                key = 0x69;
                break;
            case "j":
                key = 0x6A;
                break;
            case "k":
                key = 0x6B;
                break;
            case "l":
                key = 0x6C;
                break;
            case "m":
                key = 0x6D;
                break;
            case "n":
                key = 0x6E;
                break;
            case "o":
                key = 0x6F;
                break;
            case "p":
                key = 0x70;
                break;
            case "q":
                key = 0x71;
                break;
            case "r":
                key = 0x72;
                break;
            case "s":
                key = 0x73;
                break;
            case "t":
                key = 0x74;
                break;
            case "u":
                key = 0x75;
                break;
            case "v":
                key = 0x76;
                break;
            case "w":
                key = 0x77;
                break;
            case "x":
                key = 0x78;
                break;
            case "y":
                key = 0x79;
                break;
            case "z":
                key = 0x7A;
                break;

            case "Alpha0":
            case "Keypad0":
                key = 0x30;
                break;
            case "Alpha1":
            case "Keypad1":
                key = 0x31;
                break;
            case "Alpha2":
            case "Keypad2":
                key = 0x32;
                break;
            case "Alpha3":
            case "Keypad3":
                key = 0x33;
                break;
            case "Alpha4":
            case "Keypad4":
                key = 0x34;
                break;
            case "Alpha5":
            case "Keypad5":
                key = 0x35;
                break;
            case "Alpha6":
            case "Keypad6":
                key = 0x36;
                break;
            case "Alpha7":
            case "Keypad7":
                key = 0x37;
                break;
            case "Alpha8":
            case "Keypad8":
                key = 0x38;
                break;
            case "Alpha9":
            case "Keypad9":
                key = 0x39;
                break;

            case "F1":
                key = 0x70;
                break;
            case "F2":
                key = 0x71;
                break;
            case "F3":
                key = 0x72;
                break;
            case "F4":
                key = 0x73;
                break;
            case "F5":
                key = 0x74;
                break;
            case "F6":
                key = 0x75;
                break;
            case "F7":
                key = 0x76;
                break;
            case "F8":
                key = 0x77;
                break;
            case "F9":
                key = 0x78;
                break;
            case "F10":
                key = 0x79;
                break;
            case "F11":
                key = 0x7A;
                break;
            case "F12":
                key = 0x7B;
                break;

            case "Minus":
            case "KeypadMinus":
                key = 0xBD;
                break;
            case "BackQuote":
                key = 0xC0;
                break;
            case "Escape":
                key = 0x1B;
                break;
            case "Print":
                key = 0x1B;
                break;
            case "ScrollLock":
                key = 0x91;
                break;
            case "Pause":
                key = 0x13;
                break;

            case "Insert":
                key = 0x2D;
                break;
            case "Home":
                key = 0x24;
                break;
            case "PageUp":
                key = 0x21;
                break;
            case "Delete":
                key = 0x2E;
                break;
            case "End":
                key = 0x23;
                break;
            case "PageDown":
                key = 0x22;
                break;


            case "LeftBracket":
                key = 0xDB;
                break;
            case "RightBracket":
                key = 0xDD;
                break;
            case "Semicolon":
                key = 0xBA;
                break;
            case "Quote":
                key = 0xDE;
                break;
            case "Comma":
                key = 0xBC;
                break;
            case "Period":
                key = 0xBE;
                break;
            case "Slash":
                key = 0xBF;
                break;



            case "Equals":
                key = 0xBB;
                break;
            case "Tab":
                key = 0x09;
                break;
            case "CapsLock":
                key = 0x14;
                break;
            case "LeftShift":
                key = 0xA0;
                break;
            case "LeftControl":
                key = 0xA2;
                break;
            case "LeftApple":
            case "LeftWindows":
                key = 0x5B;
                break;
            case "LeftAlt":
                key = 0xA4;
                break;


            case "Space":
                key = 0x20;
                break;
            case "RightAlt":
                key = 0xA5;
                break;
            case "Menu":
                key = 0x5D;
                break;
            case "RightControl":
                key = 0xA3;
                break;
            case "RightShift":
                key = 0xA1;
                break;
            case "Return":
                key = 0x0D;
                break;
            case "Backslash":
                key = 0xDC;
                break;
            case "Backspace":
                key = 0x08;
                break;
            case "LeftArrow":
                key = 0x25;
                break;
            case "UpArrow":
                key = 0x26;
                break;
            case "RightArrow":
                key = 0x27;
                break;
            case "DownArrow":
                key = 0x28;
                break;

            case "Numlock":
                key = 0x90;
                break;
            case "KeypadDivide":
                key = 0x6F;
                break;
            case "KeypadMultiply":
                key = 0x6A;
                break;
            case "KeypadPlus":
                key = 0x6B;
                break;
            case "KeypadEnter":
                key = 0x0D;
                break;
            case "KeypadPeriod":
                key = 0x6E;
                break;

        }

        return key;
    }


}
