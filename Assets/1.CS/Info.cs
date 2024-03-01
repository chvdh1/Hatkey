using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags);


    // user32.dll���� ������ keybd_event �Լ�
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    private const int KEYEVENTF_KEYDOWN = 0x0000;// Ű�� ������ ��� ��
    private const int KEYEVENTF_KEYUP = 0x0002; // Ű�� ���� ��� ��

    const uint LBUTTONDOWN = 0x0002;    // ���� ���콺 ��ư ����
    const uint LBUTTONUP = 0x0004;      // ���� ���콺 ��ư ��


    public Text num;
    public Text com;
    public byte keybd;

    public int mx;
    public int my;
    public float time;

    public bool move;
    public bool click;
    public bool key;
    public bool delays;

    private void Awake()
    {
        num = transform.GetChild(0).GetChild(0).GetComponent<Text>();

        com = transform.GetChild(1).GetChild(0).GetComponent<Text>();
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

    // ��ũ�� ���� �Լ�
    public void KeyMacro()
    {
        //keybd = 0x61;

        //Debug.Log(keybd);
        // keybd_event �Լ��� ����Ͽ� Ű�� ������ ���� ��ũ�θ� �����մϴ�.
        keybd_event(keybd, 0, KEYEVENTF_KEYDOWN, 0);
        keybd_event(keybd, 0, KEYEVENTF_KEYUP, 0);
    }

    public void CL()
    {
        mx = 0;
        my = 0;
        time = 0.33f;
        num.text = "";
        com.text = "";

        move = false;
        click = false;
        key = false;
        delays = false;
    }

}
