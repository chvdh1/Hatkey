using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class HatKeyMAnager : MonoBehaviour
{
    public static HatKeyMAnager manager;


    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    public GameObject infoobj;
    public Transform parentObj;

    public Text m_Pos;
    public int count;
    public int startNum;
    public Info[] m_List;
    public Text stat;
    public InputField startNumText;

    public GameObject delayPop;
    public InputField delayText;

    public GameObject keyPop;

    public InputField repetitionText;

    public GameObject mousePop;
    public Text mouseinfo;

    public GameObject keyPopDU;
    public GameObject keyBtns;
    public Text keyInfo;
    public string keycode;



    public bool startM;

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    private void Awake()
    {
        manager = this;
        Application.runInBackground = true;
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


        repetitionText.onValueChanged.AddListener(
           (word) => repetitionText.text =
           Regex.Replace(word, @"[^0-9]", "")
       );

        startNumText.text = "1";
        repetitionText.text = "0";
    }

    private void Update()
    {
        if(keyPop.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && keyCode.ToString() != "Mouse1" && keyCode.ToString() != "Mouse0" && keyCode.ToString() != "Mouse2" && keyCode.ToString() != "Mouse3" && keyCode.ToString() != "Mouse4")
                    {
                        Debug.Log("눌린 키: " + keyCode);
                        SetKey(keyCode.ToString());
                    }
                }
            }
        }
        else if(keyPopDU.activeSelf && !keyBtns.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && keyCode.ToString() != "Mouse1" && keyCode.ToString() != "Mouse0" && keyCode.ToString() != "Mouse2" && keyCode.ToString() != "Mouse3" && keyCode.ToString() != "Mouse4")
                    {
                        Debug.Log("눌린 키: " + keyCode);
                        keycode = keyCode.ToString();
                        keyInfo.text = $"{keycode} 키를\n설정하세요.";
                        keyBtns.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                count++;
                m_List = new Info[count];
                Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
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
                    m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
                }
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                count++;
                m_List = new Info[count];
                Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
                info.CL();
                info.num.text = count.ToString();

                info.time = 0.033f;
                info.com.text = $"마우스 클릭(왼)";
                info.mx = 0;
                info.my = 0;
                info.click = true;

                for (int i = 0; i < m_List.Length; i++)
                {
                    m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
                }
            }

            if (Input.GetKeyDown(KeyCode.F1) )
            {
                if (!startM)
                    MacrosStart();
                else
                    startM = false;
            }

        }
       

    }

    //string keyInfo(string key)
    //{

    //}

    public void MacrosStart()
    {
        startM = true;
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
                StartCoroutine(StartM());
            }
        }
        else
        {
            stat.text = "시작 번호를 다르게 설정해주세요.";
        }
    }

    IEnumerator StartM()
    {
        int repetition = int.Parse(repetitionText.text);
        int start = startNum;
        yield return new WaitForFixedUpdate();
        while(start - 1 < m_List.Length && startM)
        {
            yield return new WaitForFixedUpdate();
            if (m_List[start - 1].click)
                m_List[start - 1].MouseEvent();
            else if (m_List[start - 1].move)
                m_List[start - 1].MoveCursor();
            else if (m_List[start - 1].key)
                m_List[start - 1].KeyMacro();

            else if (m_List[start - 1].m_Down)
                m_List[start - 1].MouseDown();
            else if (m_List[start - 1].m_Up)
                m_List[start - 1].MouseUp();
            else if (m_List[start - 1].k_Down)
                m_List[start - 1].KeyDown();
            else if (m_List[start - 1].k_Up)
                m_List[start - 1].KeyUp();

            yield return new WaitForSeconds(m_List[start - 1].time);
            start++;
        }
        yield return new WaitForFixedUpdate();
        if (repetition > 0 && startM)
        {
            repetitionText.text = (repetition - 1).ToString();
            MacrosStart();
        }
        else
        {
            startM = false;
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
            Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
            info.CL();
            info.num.text = count.ToString();

            info.time = float.Parse(delayText.text);
            info.com.text = $"{info.time}초 지연";
            info.mx = 0;
            info.my = 0;
            info.delays = true;

            for (int i = 0; i < m_List.Length; i++)
            {
                m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
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

    public void SetKey(string text)
    {
        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.time = 0.033f;
        info.com.text = $"'{text}' 입력";
        info.keybd = text;
        info.mx = 0;
        info.my = 0;
        info.key = true;
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        KeyPop();
    }

    public void Del()
    {
        GameObject Btn = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        Destroy(Btn);


        count--;
        m_List = new Info[count];

        StartCoroutine(Assign());
       
    }

    IEnumerator Assign()
    {

        yield return new WaitForFixedUpdate();
        for (int i = 0; i < parentObj.childCount; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
            m_List[i].num.text = (i + 1).ToString();
            yield return new WaitForFixedUpdate();
        }
    }

    public void MousePop()
    {
        if(mousePop.activeSelf)
            mousePop.SetActive(false);
        
        else
        {
            mousePop.SetActive(true);
            mouseinfo.text = "마우스 : 좌";
        }
            

    }

    public void MouseL()
    {
        mouseinfo.text = "마우스 : 좌";
    }
    public void MouseR() { mouseinfo.text = "마우스 : 우"; }
    public void MousePress()
    {
        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.mouse = mouseinfo.text == "마우스 : 우" ? 1 : 0;
        info.time = 0.033f;
        info.com.text = $"{mouseinfo.text} 누르기";
        info.m_Down = true;
        Debug.Log(info.keybd);
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        MousePop();
    }
    public void MouseUp()
    {
        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.mouse = mouseinfo.text == "마우스 : 우" ? 1 : 0;
        info.time = 0.033f;
        info.com.text = $"{mouseinfo.text} 때기";
        info.m_Up = true;
        Debug.Log(info.keybd);
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        MousePop();
    }


    public void KeyPopDU()
    {
        if(keyPopDU.activeSelf)
        {
            keyPopDU.SetActive(false);
            keycode = "";
        }
          

        else
        {
            keyPopDU.SetActive(true);
            keyInfo.text = "원하는 키를\n눌러주세요.";
            keyBtns.SetActive(false);
        }
    }


    public void KeyPress()
    {
        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.time = 0.033f;
        info.com.text = $"'{keycode}' 누르기";
        info.keybd = keycode;
        info.mx = 0;
        info.my = 0;
        info.k_Down = true;
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        KeyPopDU();

    }
    public void KeyUp()
    {
        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();

        info.time = 0.033f;
        info.com.text = $"'{keycode}' 때기";
        info.keybd = keycode;
        info.mx = 0;
        info.my = 0;
        info.k_Up = true;
        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        KeyPopDU();
    }

}
