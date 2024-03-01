using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    // ��������� ������ ����
    public GameObject[] Prefabs;

    // Ǯ�� ����ϴ� ����Ʈ��
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //������ ���� ��Ȱ��ȭ �� ���ӿ�����Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            { // �߰��ϸ� select������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        //��ã����?
        if (!select)
        {//���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(Prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}