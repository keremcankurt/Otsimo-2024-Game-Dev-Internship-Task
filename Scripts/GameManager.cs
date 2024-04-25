using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button penButton;
    [SerializeField] Button rubberButton;
    [SerializeField] Button fillButton;
    [SerializeField] Button paintBallButton;
    [SerializeField] Button colorButton;
    [SerializeField] Button colorPickerDoneButton;

    [SerializeField] GameObject colorPicker;

    public GameObject paintball;
    public LineRenderer trailPrefab;
    public GameObject canvas;

    public Color color;
    public string selected = "Pen";

    public SoundController soundController;


    private void Start()
    {
        penButton.onClick.AddListener(Pen);
        rubberButton.onClick.AddListener(Rubber);
        fillButton.onClick.AddListener(Fill);
        paintBallButton.onClick.AddListener(PaintBall);
        colorButton.onClick.AddListener(SetColor);
        colorPickerDoneButton.onClick.AddListener(ColorPickerDone);
        soundController = GameObject.FindObjectOfType<SoundController>();
    }

    void Pen()
    {
        soundController.Playback("touch");
        deactiveSelected();
        penButton.GetComponent<Outline>().enabled = true;
        selected = "Pen";
    }
    void Rubber()
    {
        soundController.Playback("touch");
        deactiveSelected();
        rubberButton.GetComponent<Outline>().enabled = true;
        selected = "Rubber";
    }
    void Fill()
    {
        soundController.Playback("touch");
        deactiveSelected();
        fillButton.GetComponent<Outline>().enabled = true;
        selected = "Fill";
    }
    void PaintBall()
    {
        soundController.Playback("touch");
        deactiveSelected();
        paintBallButton.GetComponent<Outline>().enabled = true;
        selected = "Paintball";
    }

    void SetColor()
    {
        soundController.Playback("touch");
        colorPicker.SetActive(true);
    }
    void ColorPickerDone()
    {
        soundController.Playback("touch");
        colorPicker.SetActive(false);
    }
    void deactiveSelected()
    {
        if (selected == "Pen")
        {
            penButton.GetComponent<Outline>().enabled = false;
        }
        else if(selected == "Rubber")
        {
            rubberButton.GetComponent<Outline>().enabled = false;
        }
        else if (selected == "Fill")
        {
            fillButton.GetComponent<Outline>().enabled = false;
        }
        else if (selected == "Paintball")
        {
            paintBallButton.GetComponent<Outline>().enabled = false;
        }
    }
}
