using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

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

    public Text turn;
    public Text startNstop;

    public Info modify;
    public GameObject mMovePop;
    public GameObject mKeyPop;
    public GameObject mMousePop;
    public GameObject mDelayPop;
    public GameObject mMKeyPopDU;
    public GameObject mImagePop;

    public GameObject mKeyBtns;
    public Text mKeyInfo;

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
        turn.text = "";
        startNstop.text = "시작하기";
    }
    //public GameObject mImagePop;
    private void Update()
    {
        if(keyPop.activeSelf || mKeyPop.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && keyCode.ToString() != "Mouse1" 
                        && keyCode.ToString() != "Mouse0" && keyCode.ToString() != "Mouse2"
                        && keyCode.ToString() != "Mouse3" && keyCode.ToString() != "Mouse4")
                    {
                        if(!mKeyPop.activeSelf)
                        {
                            Debug.Log("눌린 키: " + keyCode);
                            SetKey(keyCode.ToString());
                        }
                        else
                        {
                            //수정 내용
                            modify.com.text = $"'{keyCode}' 입력";
                            modify.keybd = keyCode.ToString();
                            ModifyExit();
                        }
                    }
                }
            }
        }
        else if((keyPopDU.activeSelf && !keyBtns.activeSelf) ||
            (mMKeyPopDU.activeSelf&& !mKeyBtns.activeSelf))
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && keyCode.ToString() != "Mouse1" 
                        && keyCode.ToString() != "Mouse0" && keyCode.ToString() != "Mouse2"
                        && keyCode.ToString() != "Mouse3" && keyCode.ToString() != "Mouse4")
                    {
                        if (!mMKeyPopDU.activeSelf)
                        {
                            Debug.Log("눌린 키: " + keyCode);
                            keycode = keyCode.ToString();
                            keyInfo.text = $"{keycode} 키를\n설정하세요.";
                            keyBtns.SetActive(true);
                        }
                        else
                        {
                            //수정 내용
                            keycode = keyCode.ToString();
                            mKeyInfo.text = $"{keycode} 키를\n설정하세요.";
                            mKeyBtns.SetActive(true);
                        }
                    }
                }
            }
        }
        else if(mMovePop.activeSelf && Input.GetKeyDown(KeyCode.F10))
        {
            POINT cursorPos;
            GetCursorPos(out cursorPos);
            modify.com.text = $"위치값: {cursorPos.X},{cursorPos.Y}";
            modify.mx = cursorPos.X;
            modify.my = cursorPos.Y;
            ModifyExit();
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
                info.com.text = $"위치값: {cursorPos.X},{cursorPos.Y}";
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

    public Image startBtn;
    
    public void MacrosStart()
    {
        if(!startM)
        {
            startNumText.text = Regex.Replace(startNumText.text, @"[^0-9]", "");
            if (int.Parse(startNumText.text) > 0)
            {
                startNum = int.Parse(startNumText.text);

                if (startNum > m_List.Length + 1 || startNum < 0)
                {
                    stat.text = "시작 번호를 다르게 설정해주세요.";
                }
                else
                {
                    stat.text = "";
                    startM = true;
                    startBtn.color = Color.red;
                    startNstop.color = Color.white;
                    startNstop.text = "종료하기";
                    StartCoroutine(StartM());
                }
            }
            else
            {
                stat.text = "시작 번호를 다르게 설정해주세요.";
            }
        }
        else
        {
            startM = false;
        }
       
    }

    IEnumerator StartM()
    {
        int repetition = int.Parse(repetitionText.text);
        int start = startNum;
        float wait = 0;
        yield return new WaitForFixedUpdate();
       
        while (start - 1 < m_List.Length && startM)
        {
            turn.text = $"진행중 : {start}";
            
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
            else if (m_List[start - 1].image)
                m_List[start - 1].Image();

            wait = m_List[start - 1].time;
            while (wait > 0 && startM)
            {
                wait -= Time.deltaTime;
                stat.text = $"남은 대기 시간 : {(int)wait}";
                yield return null;
            }
            start++;
        }
        yield return new WaitForFixedUpdate();
        if (repetition > 0 && startM)
        {
            repetitionText.text = (repetition - 1).ToString();
            StartCoroutine(StartM());
        }
        else
        {
            startM = false;
            startBtn.color = Color.white;
            startNstop.color = new Color(0.196f, 0.196f, 0.196f, 1);
            startNstop.text = "시작하기";
            turn.text = "";
            stat.text = "";
        }
    }

    //딜레이 시간 
    public void DelayPop()
    {
        if (delayPop.activeSelf) { delayPop.SetActive(false); }
        else
        {
            delayPop.SetActive(true);
            delayText.Select();
            delayText.text = "";
        }
    }
    public void SetDelay()
    {
        if (delayText.text == "")
        { delayText.text = "0"; }
          

        if (float.Parse(delayText.text) > 0.033f)
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
        {
            count++;
            m_List = new Info[count];
            Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
            info.CL();
            info.num.text = count.ToString();

            info.time = 0.033f;
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
    }

    public InputField mdelayText;

    public void SetMDelay() // 수정 내용
    {
        if (mdelayText.text == "")
        { mdelayText.text = "0"; }


        if (float.Parse(mdelayText.text) > 0.033f)
        {
            modify.time = float.Parse(mdelayText.text);
            modify.com.text = $"{modify.time}초 지연";
            ModifyExit();
        }
        else
        {
            modify.time = 0.033f;
            modify.com.text = $"{modify.time}초 지연";
            ModifyExit();
        }
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


    //목록 지우기 기능
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


    //마우스 좌, 우 , 누르기, 때기 기능
    public void MousePop()
    {
        if (mousePop.activeSelf)
        {
            mousePop.SetActive(false);
        }
        else
        {
            mousePop.SetActive(true);
            mouseinfo.text = "마우스 : ";
            Direction.SetActive(true);
            Press.SetActive(false);
        }
    }
    public GameObject Direction;
    public GameObject Press;
    public void MouseL() { mouseinfo.text = "마우스 : 좌"; Direction.SetActive(false); Press.SetActive(true); }
    public void MouseR() { mouseinfo.text = "마우스 : 우"; Direction.SetActive(false); Press.SetActive(true); }
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
    //수정 내용
    public Text mMouseinfo;
    public GameObject mDirection;
    public GameObject mPress;

    public void MMouseL() { mMouseinfo.text = "마우스 : 좌"; mDirection.SetActive(false); mPress.SetActive(true); }
    public void MMouseR() { mMouseinfo.text = "마우스 : 우"; mDirection.SetActive(false); mPress.SetActive(true); }
    public void MMMousePress()
    {
        modify.mouse = mMouseinfo.text == "마우스 : 우" ? 1 : 0;
        modify.com.text = $"{mMouseinfo.text} 누르기";
        modify.m_Down = true;
        modify.m_Up = false;
        ModifyExit();
    }
    public void MMouseUp()
    {
        modify.mouse = mMouseinfo.text == "마우스 : 우" ? 1 : 0;
        modify.com.text = $"{mMouseinfo.text} 때기";
        modify.m_Down = false;
        modify.m_Up = true;
        ModifyExit();
    }



    //키보드 누르기, 때기 기능
    public void KeyPopDU()
    {
        if(keyPopDU.activeSelf)
        {
            keycode = "";
            keyPopDU.SetActive(false);
        }
        else
        {
            keyPopDU.SetActive(true);
            keyInfo.text = "원하는 키를\n눌러주세요.";
            if(keyBtns.activeSelf)
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
    //수정 내용
    public GameObject mkeyPopDU;
    public void MKeyPress()
    {
        modify.com.text = $"'{keycode}' 누르기";
        modify.keybd = keycode;
        modify.k_Down = true;
        modify.k_Up = false;
        mMKeyPopDU.SetActive(false);
        modify = null;
    }
    public void MKeyUp()
    {
        modify.com.text = $"'{keycode}' 때기";
        modify.keybd = keycode;
        modify.k_Down = false;
        modify.k_Up = true;
        ModifyExit();
    }


    //이미지 찾아 마우스 위치하기 기능
    public GameObject imagePop;
    public InputField imageInputField;
    public Text imagestat;

    public void ImagePop()
    {
        if (imagePop.activeSelf)
        {
            imagePop.SetActive(false);
        }
        else
        {
            imagePop.SetActive(true);
            imagestat.text = "이미지의 파일 경로를 적어주세요.";
            imageInputField.text = "";
            imageInputField.Select();
        }
    }

    public void ImageSet()
    {
        if (imageInputField.text == "")
            return;

        count++;
        m_List = new Info[count];
        Info info = Instantiate(infoobj, parentObj).GetComponent<Info>();
        info.CL();
        info.num.text = count.ToString();
        info.paths = imageInputField.text;

        info.time = 1f;
        info.com.text = $"'{imageInputField.text}' 유사 이미지 찾기";

        info.image = true;

        for (int i = 0; i < m_List.Length; i++)
        {
            m_List[i] = parentObj.GetChild(i).GetComponent<Info>();
        }

        ImagePop();
    }
    //수정내용
    public InputField mimageInputField;
    public void MImageSet()
    {
        if (mimageInputField.text == "")
            return;

        modify.paths = mimageInputField.text;
        modify.time = 1f;
        modify.com.text = $"'{mimageInputField.text}' 유사 이미지 찾기";
        ModifyExit();
    }


    public void InfoModify()
    {
        if (modify.move) { mMovePop.SetActive(true); }
        else if (modify.m_Down || modify.m_Up) 
        { 
            mMousePop.SetActive(true);
            mDirection.SetActive(true);
            mPress.SetActive(false);
        }
        else if (modify.k_Down || modify.k_Up)
        {
            mMKeyPopDU.SetActive(true);
            mKeyBtns.SetActive(false);
        }
        else if (modify.key) { mKeyPop.SetActive(true); }
        else if (modify.delays) { mDelayPop.SetActive(true); mdelayText.Select(); }
        else if (modify.image) { mImagePop.SetActive(true); mimageInputField.Select(); }
    }

    public void ModifyExit()
    {
        if (modify.move) { mMovePop.SetActive(false); }
        else if (modify.m_Down || modify.m_Up) { mMousePop.SetActive(false); }
        else if (modify.k_Down || modify.k_Up){ mMKeyPopDU.SetActive(false);}
        else if (modify.key) { mKeyPop.SetActive(false); }
        else if (modify.delays) { mDelayPop.SetActive(false);  }
        else if (modify.image) { mImagePop.SetActive(false); }

        modify = null;
    }


}
