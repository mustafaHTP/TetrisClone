using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    [Header("Screen Config")]
    [Space(5)]
    [SerializeField] private int _screenWidth = 1080;
    [SerializeField] private int _screenHeight = 1920;

    private void Awake()
    {
        SetResolution();
    }

    private void SetResolution()
    {
        Screen.SetResolution(_screenWidth, _screenHeight, true);
    }
}
