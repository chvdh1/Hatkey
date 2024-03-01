using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.Collections;

public class HatKeyMAnager : MonoBehaviour
{

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

   

    public Text m_Pos;
    public int count;
    public int startNum;
    public Info[] m_List;
    public Pool pool;
    public Text stat;
    public InputField startNumText;

    public GameObject delayPop;
    public InputField delayText;

    public GameObject keyPop;

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }


    private void Start()
    {
        int setWidth = 500; // 화면 너비
        int setHeight = 500; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, false);

        delayText.onValueChanged.AddListener(
           (word) => delayText.text =
           Regex.Replace(word, @"[^0.0-9.9]", "")
       );


        startNumText.onValueChanged.AddListener(
           (word) => startNumText.text =
           Regex.Replace(word, @"[^0-9]", "")
       );


        startNumText.text = "1";
    }

    private void Update()
    {
        if(!keyPop.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                count++;
                m_List = new Info[count];
                Info info = pool.Get(0).GetComponent<Info>();
                info.CL();
                info.num.text = count.ToString();

                info.time = 0.033f;
                POINT cursorPos;
                GetCursorPos(out cursorPos);
                info.com.text = $"마우스 위치값: {cursorPos.X},{cursorPos.Y}";
                info.mx = cursorPos.X;
                info.my = cursorPos.Y;
                info.move = true;
                for (int i = 0; i < m_List.Length; i++)
                {
                    m_List[i] = pool.transform.GetChild(i).GetComponent<Info>();
                }
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                count++;
                m_List = new Info[count];
                Info info = pool.Get(0).GetComponent<Info>();
                info.CL();
                info.num.text = count.ToString();

                info.time = 0.033f;
                info.com.text = $"마우스 클릭(왼)";
                info.mx = 0;
                info.my = 0;
                info.click = true;

                for (int i = 0; i < m_List.Length; i++)
                {
                    m_List[i] = pool.transform.GetChild(i).GetComponent<Info>();
                }
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        Debug.Log("눌린 키: " + keyCode);
                        int value = (byte)keyCode;
                        string hex = "0x" + value.ToString("X2");
                        byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(hex);
                        byte key = keyBytes[0];
                        Debug.Log(key);
                        SetKey(key, keyCode.ToString());
                    }
                }
            }
        }
       

    }

    public void MacrosStart()
    {
        startNumText.text = Regex.Replace(startNumText.text, @"[^0-9]", "");
        if(int.Parse(startNumText.text) > 0)
        {
            startNum = int.Parse(startNumText.text);

            if (startNum > m_List.Length+1 || startNum < 0)
            {
                stat.text = "시작 번호를 다르게 설정해주세요.";
            }
            else
            {
                stat.text = "";
                StartCoroutine(startM());
            }
        }
        else
        {
            stat.text = "시작 번호를 다르게 설정해주세요.";
        }
    }

    IEnumerator startM()
    {

        yield return new WaitForFixedUpdate();
        while(startNum-1 < m_List.Length)
        {
            yield return new WaitForFixedUpdate();
            if (m_List[startNum-1].click)
                m_List[startNum-1].MouseEvent();
            else if (m_List[startNum - 1].move)
                m_List[startNum - 1].MoveCursor();
            else if (m_List[startNum - 1].key)
                m_List[startNum - 1].KeyMacro();

            yield return new WaitForSeconds(m_List[startNum - 1].time);
            startNum++;
        }
    }

    //딜레이 시간 
    public void DelayPop()
    {
        if(delayPop.activeSelf)
        {
            delayPop.SetActive(false);
           
        }
        else
        {
            delayPop.SetActive(true);
            delayText.text = "";
        }
    }

    public void SetDelay()
    {
        if (float.Parse(delayText.text) > 0)
        {
            count++;
            m_List = new Info[count];
            Info info = pool.Get(0).GetComponent<Info>();
            info.CL();
            info.num.text = count.ToString();

            info.time = float.Parse(delayText.text);
            info.com.text = $"{info.time}초 지연";
            info.mx = 0;
            info.my = 0;
            info.delays = true;

            for (int i = 0; i < m_List.Length; i++)
            {
                m_List[i] = pool.transform.GetChild(i).GetComponent<Info>();
            }

            DelayPop();
        }
        else
            return;
        
    }

    //키보드 입력
    public void KeyPop()
    {
        if(keyPop.activeSelf)
            keyPop.SetActive(false);
        else
            keyPop.SetActive(true);
    }

    public void SetKey(byte key, string text)
    {
        count++;
        m_List = new Info[count];
        Info info = pool.Get(0).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.time = 0.033f;
        info.com.text = $"'{text}' 입력";
        info.keybd = key;
        info.mx = 0;
        info.my = 0;
        info.key = true;
        Debug.Log(info.keybd);
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = pool.transform.GetChild(i).GetComponent<Info>();
        }

        KeyPop();
    }

}
