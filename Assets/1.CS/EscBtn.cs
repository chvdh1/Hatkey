using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscBtn : MonoBehaviour
{
    public GameObject obj;

    private void Update()
    {
        // Esc 키를 눌렀는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            obj.SetActive(false);
        }
    }
}
