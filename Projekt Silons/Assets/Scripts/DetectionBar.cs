using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    //Variables 
    public Slider detectionSlider;
    public Gradient gradient;
    public Image fill;
    private Vector3 openedUI = new Vector3(1, 1, 1);
    private Vector3 closedUI = new Vector3(0, 0, 0);

    public void SetDetectionValue(float detectionValue)
    {
        detectionValue = detectionValue * 0.1f;
        detectionSlider.value = detectionValue;

        fill.color = gradient.Evaluate(detectionSlider.normalizedValue);
    }

    public void ShowDetectionBar()
    {
        transform.localScale = openedUI;
    }
    public void HideDetectionBar()
    {
        transform.localScale = closedUI;
    }
}
