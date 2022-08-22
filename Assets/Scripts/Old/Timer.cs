using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    // Variables for timer
    public float countDownStartValue = 300;
    public Text timerUI;

    void Update()
    {
        // Timer counts down until out of time and cuts to end scene
        if (countDownStartValue > 1)
        {
            // Timer count down
            countDownStartValue -= Time.deltaTime;

            // Counter skips 50s
            if (Input.GetKeyDown(KeyCode.Z))
            {
                countDownStartValue -= 290;
            }

            // Calls DisplayTime function
            DisplayTime(countDownStartValue);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Displaying remaining time in minutes and seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Format for the timer
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (minutes < 1)
        {
            if (seconds < 1)
            {
                GameObject CollisionDetection = GameObject.Find("CollisionDetection");
                PlayerTracking ScreenshotScript = CollisionDetection.GetComponent<PlayerTracking>();
                ScreenshotScript.enabled = true;

                Invoke("OnPlay", 0.1f);
            }
        }
    }

    // Provides delay for the screenshot before switching scene
    void OnPlay()
    {
        SceneManager.LoadScene(3);
    }
}
