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
        }   
    }
}
public class DataManager : MonoBehaviour
{
    public HatKeyMAnager manager;

    // ������ ������Ʈ��� ��ũ��Ʈ��
    public List<MonoBehaviour> targetScripts;

    public InputField filePath;
    public Text stat;
    public Text datapos;


    private void Start()
    {
        manager = HatKeyMAnager.manager;

        string path = Path.Combine(Application.dataPath, filePath.text);

        datapos.text = "��� : " + path;
    }
  
    // �����ϱ�
    public void SaveToJson()
    {
        if(filePath.text == "")
        {
            stat.color = Color.red;
            stat.text = "���� �̸��� �ۼ����ּ���.";
            return;
        }
        stat.color = new Color(0.2f,0.2f,0.2f,0.5f);
        stat.text = "���� �̸�";

        StartCoroutine(Save());
       
    }

    IEnumerator Save()
    {
        yield return new WaitForFixedUpdate();
        targetScripts.Clear();

        string path = Path.Combine(Application.dataPath, filePath.text);


        JData data = new JData(manager);
        string json = JsonUtility.ToJson(data);

        //// �ϼ��� json string ���ڿ��� 8��Ʈ ��ȣ���� ������ ��ȯ
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        //// ��ȯ�� ����Ʈ�迭�� base-64 ���ڵ��� ���ڿ��� ��ȯ
        //string encodedJson = System.Convert.ToBase64String(bytes);

        // ��ȯ�� ���� ����
        File.WriteAllText(path, json);

        Debug.Log(path);
        datapos.text = "��� : " + path;
    }

    // �ҷ�����
    public void LoadFromJson()
    {
        if (filePath.text == "")
        {
            stat.color = Color.red;
            stat.text = "���� �̸��� �ۼ����ּ���.";
            return;
        }
        if (!File.Exists(Path.Combine(Application.dataPath, filePath.text)))
        {
            filePath.text = "���� �̸��� Ȯ�����ּ���.";
            return;
        }
        stat.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        stat.text = "���� �̸�";

        StartCoroutine(LoadStart());
    }
    IEnumerator LoadStart()
    {
        string path = Path.Combine(Application.dataPath, filePath.text);

        // json���� ����� ���ڿ��� �ε��Ѵ�.
        string jsonFromFile = File.ReadAllText(path);

        //// �о�� base-64 ���ڵ� ���ڿ��� ����Ʈ�迭�� ��ȯ
        //byte[] bytes = System.Convert.FromBase64String(jsonFromFile);

        //// 8��Ʈ ��ȣ���� ������ json ���ڿ��� ��ȯ
        //string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);


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

            string mtext = info.mouse == 1 ? "���콺 : ��" : "���콺 : ��";

            info.com.text = info.move ? $"���콺 ��ġ��: {info.mx},{info.my}" :
               info.click ? $"���콺 Ŭ��(��)" :
               info.m_Down ? $"{mtext} ������" :
               info.m_Up ? $"{mtext} ����" :
                info.key ? $"'{info.keybd}' �Է�" :
                 info.k_Down ? $"'{info.keybd}' ������" :
                  info.k_Up ? $"'{info.keybd}' ����" :
               $"{info.time}�� ����";

            manager.m_List[i] = info;
           yield return null;
        }

        datapos.text = "��� : " + path;
    }
}