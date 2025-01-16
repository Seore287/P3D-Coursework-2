using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraRotationSettings 
{
    [SerializeField] private float horizontalAngle = 0f;
    [SerializeField] private float verticalAngle = 20f;

    public float HorizontalAngle
    {
        get => horizontalAngle;
        set => horizontalAngle = Mathf.Clamp(value, -360f, 360);
    }

    public float VerticalAngle
    {
        get => verticalAngle;
        set => verticalAngle = Mathf.Clamp(value, -90f, 90f);
    }
}
