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

    public void SetDetectionValue(float detectionValue)
    {
        detectionValue = detectionValue * 0.1f;
        detectionSlider.value = detectionValue;

        fill.color = gradient.Evaluate(detectionSlider.normalizedValue);
    }
}
