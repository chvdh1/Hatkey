using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterKey : MonoBehaviour
{
    Button targetButton;


    private void Awake()
    {
        targetButton = GetComponent<Button>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            targetButton.onClick.Invoke();
        }
    }
}
