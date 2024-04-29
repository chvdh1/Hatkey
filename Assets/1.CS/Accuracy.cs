using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Accuracy : MonoBehaviour
{
    public Slider slider;
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = ((int)(slider.value*100)).ToString()+"%";
    }
}
