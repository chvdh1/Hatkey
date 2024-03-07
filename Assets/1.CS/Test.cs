
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Test : MonoBehaviour
{
    public InputField testInputField;

    public byte te;

    private void Awake()
    {
        testInputField.onValueChanged.AddListener(
            (word) => testInputField.text = 
            Regex.Replace(word, @"[^0-9a-zA-Z가-힣]", "")
        );
    }

    private void Start()
    {
        Debug.Log(te);
    }


    //1. 숫자만 얻기
    // - 정규식 : [^0-9]
    //    string str = "Englsh@korea$101299**한글";

    //    // 숫자만 0-9
    //    str = Regex.Replace(str, @"[^0-9]", "");

    //// 결과 : 101299

    //2. 영문자만 얻기
    // - 정규식 : [^a-zA-Z]
    //    string str = "Englsh@korea$101299**한글";

    //    // 영문자 a-z A-Z
    //    str = Regex.Replace(str, @"[^a-zA-Z]", "");

    //// 결과 : Englshkorea


    //3. 한글만 얻기
    // - 정규식 : [^가-힣]
    //    string str = "Englsh@korea$101299**한글";

    //    // 한글만 가-힣
    //    str = Regex.Replace(str, @"[^가-힣]", "");

    //// 결과 : 한글


    //4. 특수문자 제거
    //  - 정규식 : [^0-9a-zA-Z가-힣]
    //    string str = "Englsh@korea$101299**한글";
    //    // 특수문자 제거
    //    str = Regex.Replace(str, @"[^0-9a-zA-Z가-힣]", "");

    //// 결과 : Englshkorea101299한글


    //※ 위의 예에서와 같이 정규식을 잘 이용하면 얻고자 하는 문자를 쉽게 처리 할 수 있습니다.
}
