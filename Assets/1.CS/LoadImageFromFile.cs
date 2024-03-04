using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class LoadImageFromFile : MonoBehaviour
{
    public string imagePath; // 이미지 파일 경로
    
    public Texture2D imageTexture; // 구분할 이미지 텍스처

    void Start()
    {
        // 텍스처를 Mat 형식으로 변환
        Mat imageMat = TextureToMat(imageTexture);

        // 이미지 처리 및 구분
        // 여기에서는 예시로 이미지의 색상 정보를 분석하여 분류합니다.
        Scalar lowerColor = new Scalar(0, 0, 0); // 구분하고자 하는 색 범위의 하한값
        Scalar upperColor = new Scalar(255, 255, 255); // 구분하고자 하는 색 범위의 상한값

        Mat mask = new Mat();
        Cv2.InRange(imageMat, lowerColor, upperColor, mask);

        // 구분된 이미지를 화면에 표시
        Texture2D resultTexture = MatToTexture(mask);
        GetComponent<Renderer>().material.mainTexture = resultTexture;

        // 매크로 동작을 수행하거나 구분된 이미지를 활용한 추가 작업을 수행할 수 있습니다.
        // 원하는 동작을 여기에 추가하세요.
    
        // 이미지 파일 로드
        Texture2D loadedTexture = LoadTextureFromFile(imagePath);

        if (loadedTexture != null)
        {
            // 이미지 텍스처 할당
            imageTexture = loadedTexture;
        }
        else
        {
            Debug.LogError("이미지 파일을 로드할 수 없습니다.");
        }
    }
    Mat TextureToMat(Texture2D texture)
    {
        byte[] textureData = texture.EncodeToPNG(); // Texture2D를 바이트 배열로 변환
        Mat mat = Cv2.ImDecode(textureData, ImreadModes.Color); // 바이트 배열을 Mat으로 디코딩

        return mat;
    }
    Texture2D MatToTexture(Mat mat)
    {
        Texture2D texture = new Texture2D(mat.Width, mat.Height, TextureFormat.RGBA32, false);

        // Mat 데이터를 Texture로 복사
        byte[] matData = new byte[mat.Total() * mat.Channels()];
        mat.GetArray(0, 0, matData);

        texture.LoadRawTextureData(matData);
        texture.Apply();

        return texture;
    }
    Texture2D LoadTextureFromFile(string filePath)
    {
        Texture2D tex = null;

        if (System.IO.File.Exists(filePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        else
        {
            Debug.LogError("이미지 파일이 존재하지 않습니다.");
        }

        return tex;
    }
}
