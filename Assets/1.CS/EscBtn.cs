using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscBtn : MonoBehaviour
{
    public GameObject obj;

    private void Update()
    {
        // Esc Ű�� �������� Ȯ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            obj.SetActive(false);
        }
    }
}
