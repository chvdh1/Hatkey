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
        // ���ø� �̹��� �ε�
        Mat templateImage1 = Cv2.ImRead("template.png", ImreadModes.Color); //templateImage;// 

        // ȭ�� �̹��� ĸó
        ScreenCapture.CaptureScreenshot("screenshot.png");
        Mat screenImage = Cv2.ImRead("screenshot.png", ImreadModes.Color);

        // ���ø� ��Ī ����
        Mat resultImage = screenImage.MatchTemplate(templateImage1, TemplateMatchModes.CCoeffNormed);

        // ��� �̹������� ���� ���� ��ġ���� ���� ��ġ ã��
        double minVal, maxVal;
        OpenCvSharp.Point minLoc, maxLoc;
        Cv2.MinMaxLoc(resultImage, out minVal, out maxVal, out minLoc, out maxLoc);

        // ���� ���� ��ġ���� ���� ��ġ�� ���콺 Ŀ�� �̵�
        //SetCursorPos(maxLoc.X, maxLoc.Y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
