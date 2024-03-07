using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using OpenCvSharp;

public class LoadImageFromFile : MonoBehaviour
{

    Texture2D templateImage;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    IEnumerator TemplateImage()
    {
        string filePath = "file:///C:/path_to_your_image/template.png";

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(filePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            templateImage = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

        yield return new WaitForFixedUpdate();
        // 템플릿 이미지 로드
        Mat templateImage1 = Cv2.ImRead("template.png", ImreadModes.Color); //templateImage;// 

        // 화면 이미지 캡처
        ScreenCapture.CaptureScreenshot("screenshot.png");
        Mat screenImage = Cv2.ImRead("screenshot.png", ImreadModes.Color);

        // 템플릿 매칭 수행
        Mat resultImage = screenImage.MatchTemplate(templateImage1, TemplateMatchModes.CCoeffNormed);

        // 결과 이미지에서 가장 높은 일치도를 가진 위치 찾기
        double minVal, maxVal;
        OpenCvSharp.Point minLoc, maxLoc;
        Cv2.MinMaxLoc(resultImage, out minVal, out maxVal, out minLoc, out maxLoc);

        // 가장 높은 일치도를 가진 위치로 마우스 커서 이동
        //SetCursorPos(maxLoc.X, maxLoc.Y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
