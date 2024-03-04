using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class LoadImageFromFile : MonoBehaviour
{
    public string imagePath; // �̹��� ���� ���
    
    public Texture2D imageTexture; // ������ �̹��� �ؽ�ó

    void Start()
    {
        // �ؽ�ó�� Mat �������� ��ȯ
        Mat imageMat = TextureToMat(imageTexture);

        // �̹��� ó�� �� ����
        // ���⿡���� ���÷� �̹����� ���� ������ �м��Ͽ� �з��մϴ�.
        Scalar lowerColor = new Scalar(0, 0, 0); // �����ϰ��� �ϴ� �� ������ ���Ѱ�
        Scalar upperColor = new Scalar(255, 255, 255); // �����ϰ��� �ϴ� �� ������ ���Ѱ�

        Mat mask = new Mat();
        Cv2.InRange(imageMat, lowerColor, upperColor, mask);

        // ���е� �̹����� ȭ�鿡 ǥ��
        Texture2D resultTexture = MatToTexture(mask);
        GetComponent<Renderer>().material.mainTexture = resultTexture;

        // ��ũ�� ������ �����ϰų� ���е� �̹����� Ȱ���� �߰� �۾��� ������ �� �ֽ��ϴ�.
        // ���ϴ� ������ ���⿡ �߰��ϼ���.
    
        // �̹��� ���� �ε�
        Texture2D loadedTexture = LoadTextureFromFile(imagePath);

        if (loadedTexture != null)
        {
            // �̹��� �ؽ�ó �Ҵ�
            imageTexture = loadedTexture;
        }
        else
        {
            Debug.LogError("�̹��� ������ �ε��� �� �����ϴ�.");
        }
    }
    Mat TextureToMat(Texture2D texture)
    {
        byte[] textureData = texture.EncodeToPNG(); // Texture2D�� ����Ʈ �迭�� ��ȯ
        Mat mat = Cv2.ImDecode(textureData, ImreadModes.Color); // ����Ʈ �迭�� Mat���� ���ڵ�

        return mat;
    }
    Texture2D MatToTexture(Mat mat)
    {
        Texture2D texture = new Texture2D(mat.Width, mat.Height, TextureFormat.RGBA32, false);

        // Mat �����͸� Texture�� ����
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
            Debug.LogError("�̹��� ������ �������� �ʽ��ϴ�.");
        }

        return tex;
    }
}
