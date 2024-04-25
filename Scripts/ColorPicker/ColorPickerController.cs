using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPickerController : MonoBehaviour
{
    public float currentHue, currentSat, currentVal;
    [SerializeField] RawImage hueImage, satValImage, outPutImage;
    [SerializeField] Slider hueSlider;
    [SerializeField] TMP_InputField hexInputFiled;
    Texture2D hueTexture, svTexture, outputTexture;

    [SerializeField] Image changeThisColor;
    GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();
        UpdateOutputImage();
    }
    void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.height; i++)
        {
            hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, /*0.05f*/ 1));
        }
        hueTexture.Apply();
        currentHue = 0;
        hueImage.texture = hueTexture;
    }

    void CreateSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SetValTexture";
        for (int i = 0; i < svTexture.height; i++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, i, Color.HSVToRGB(
                    currentHue,
                    (float)x / svTexture.width,
                    (float)i / svTexture.height));
            }
        }
        svTexture.Apply();
        currentSat = 0;
        currentVal = 0;
        satValImage.texture = svTexture;
    }

    void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }
        outputTexture.Apply();
        outPutImage.texture = outputTexture;
    }

    void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);
        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }
        outputTexture.Apply();
        changeThisColor.color = currentColor;
        gameManager.color = currentColor;
    }

    public void SetSV(float s, float v)
    {
        currentSat = s;
        currentVal = v;
        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = hueSlider.value;
        for (int i = 0; i < svTexture.height; i++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, i, Color.HSVToRGB(
                    currentHue,
                    (float)x / svTexture.width,
                    (float)i / svTexture.height));
            }
        }
        svTexture.Apply();
        UpdateOutputImage();
    }
}
