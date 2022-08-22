using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerTracking : MonoBehaviour
{
    public int superSize = 2;
    private int _shotIndex = 0;
    public GameObject RouteCam;
    public GameObject MainCam1;
    public GameObject MainCam2;
    public GameObject MainCam3;

    private void Start()
    {
        // Begin with main cameras
        RouteCam.SetActive(false);
    }

    private void Update()
    {
        // Switches main camera to aerial camera
        RouteCam.SetActive(true);
        MainCam1.SetActive(false);
        MainCam2.SetActive(false);
        MainCam3.SetActive(false);

        // Takes a screenshot of the viewed camera and stores in the project folder as "Screenshot0"
        ScreenCapture.CaptureScreenshot($"Screenshot{_shotIndex}.png", superSize);
        _shotIndex++;
        Debug.Log("Screenshot Captured");
    }
}
