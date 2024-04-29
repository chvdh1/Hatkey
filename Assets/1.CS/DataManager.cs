using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JData
{
    public int count;

    public Info[] m_List;
    public float[] time;
    public string[] keybd;
    public int[] mx;
    public int[] my;
    public int[] mouse;


    public bool[] move;
    public bool[] click;
    public bool[] m_Down;
    public bool[] m_Up;
    public bool[] key;
    public bool[] k_Down;
    public bool[] k_Up;
    public bool[] delays;
    public bool[] image;
    public float[] accuracy;
    public string[] paths;
    public string[] imagename;

    public int[] imagex;
    public int[] imagey;
    public int[] imagelength;
    public int[] imagehHeight;

    public JData(HatKeyMAnager manager)
    {
        count = manager.count;

        m_List = new Info[count];

        time = new float[count];
        keybd = new string[count];
        mx = new int[count];
        my = new int[count];
        mouse = new int[count];

        move = new bool[count];
        click = new bool[count];
        m_Down = new bool[count];
        m_Up = new bool[count];
        key = new bool[count];
        k_Down = new bool[count];
        k_Up = new bool[count];
        delays = new bool[count];
        image = new bool[count];
        accuracy = new float[count];
        paths = new string[count];
        imagename = new string[count];


        imagex = new int[count];
        imagey = new int[count];
        imagelength = new int[count];
        imagehHeight = new int[count];


        for (int i = 0; i < count; i++)
        {
            m_List[i] = manager.m_List[i];

            time[i] = manager.m_List[i].time;
            keybd[i] = manager.m_List[i].keybd;
            mx[i] = manager.m_List[i].mx;
            my[i] = manager.m_List[i].my;
            mouse[i] = manager.m_List[i].mouse;

            move[i] = manager.m_List[i].move;
            click[i] = manager.m_List[i].click;
            m_Down[i] = manager.m_List[i].m_Down;
            m_Up[i] = manager.m_List[i].m_Up;
            key[i] = manager.m_List[i].key;
            k_Down[i] = manager.m_List[i].k_Down;
            k_Up[i] = manager.m_List[i].k_Up;
            delays[i] = manager.m_List[i].delays;
            image[i] = manager.m_List[i].image;
            accuracy[i] = manager.m_List[i].accuracy;
            paths[i] = manager.m_List[i].paths;
            imagename[i] = manager.m_List[i].im_name; ;
        }   
    }
}
public class DataManager : MonoBehaviour
{
    public HatKeyMAnager manager;

    // 저장할 오브젝트들과 스크립트들
    public List<MonoBehaviour> targetScripts;

    public InputField filePath;
    public Text stat;
    public Text datapos;

    public static string exepath = $"{System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)}\\" + 
         System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

    private void Start()
    {
        manager = HatKeyMAnager.manager;

        string path = Path.Combine(Application.dataPath, filePath.text);

        datapos.text = "경로 : " + path;
    }
  
    // 저장하기
    public void SaveToJson()
    {
        if(filePath.text == "")
        {
            stat.color = Color.red;
            stat.text = "파일 이름을 작성해주세요.";
            return;
        }
        stat.color = new Color(0.2f,0.2f,0.2f,0.5f);
        stat.text = "파일 이름";

        StartCoroutine(Save());
       
    }

    IEnumerator Save()
    {
        yield return new WaitForFixedUpdate();
        targetScripts.Clear();

        string path = Path.Combine(Application.dataPath, filePath.text);


        JData data = new JData(manager);
        string json = JsonUtility.ToJson(data);

        // 변환된 값을 저장
        File.WriteAllText(path, json);

        Debug.Log(path);
        datapos.text = "경로 : " + path;
    }

    // 불러오기
    public void LoadFromJson()
    {
        if (filePath.text == "")
        {
            stat.color = Color.red;
            stat.text = "파일 이름을 작성해주세요.";
            return;
        }
        if (!File.Exists(Path.Combine(Application.dataPath, filePath.text)))
        {
            filePath.text = "파일 이름을 확인해주세요.";
            return;
        }
        stat.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        stat.text = "파일 이름";

        StartCoroutine(LoadStart());
    }
    IEnumerator LoadStart()
    {
        string path = Path.Combine(Application.dataPath, filePath.text);

        // json으로 저장된 문자열을 로드한다.
        string jsonFromFile = File.ReadAllText(path);

        JData data = JsonUtility.FromJson<JData>(jsonFromFile);

        manager.count = data.count;
        yield return new WaitForFixedUpdate();
        for (int i = manager.parentObj.childCount - 1; i >= 0; i--)
        {
            Destroy(manager.parentObj.GetChild(i).gameObject);
            yield return null;
        }

        manager.m_List = new Info[manager.count];

        yield return data;
        for (int i = 0; i < manager.count; i++)
        {
            Info info = Instantiate(manager.infoobj, manager.parentObj).GetComponent<Info>();
            info.CL();
            info.num.text = (i + 1).ToString();

            info.time = data.time[i];
            info.keybd = data.keybd[i];
            info.mx = data.mx[i];
            info.my = data.my[i];
            info.mouse = data.mouse[i];
            info.move = data.move[i];
            info.click = data.click[i];
            info.m_Down = data.m_Down[i];
            info.m_Up = data.m_Up[i];
            info.key = data.key[i];
            info.k_Down = data.k_Down[i];
            info.k_Up = data.k_Up[i];
            info.delays = data.delays[i];

            info.image = data.image[i];
            info.accuracy = data.accuracy[i];
            info.paths = data.paths[i];
            info.im_name = data.imagename[i];

            string mtext = info.mouse == 1 ? "마우스 : 우" : "마우스 : 좌";

            info.com.text = info.move ? $"위치값: {info.mx},{info.my}" :
               info.click ? $"마우스 클릭(왼)" :
               info.m_Down ? $"{mtext} 누르기" :
               info.m_Up ? $"{mtext} 때기" :
                info.key ? $"'{info.keybd}' 입력" :
                 info.k_Down ? $"'{info.keybd}' 누르기" :
                  info.k_Up ? $"'{info.keybd}' 때기" :
                   info.image ? $"{(int)(info.accuracy * 100)}% 일치하는 '{info.im_name}'이미지 찾기":
            $"{info.time}초 지연";



            manager.m_List[i] = info;
           yield return null;
        }

        datapos.text = "경로 : " + path;
    }
}
