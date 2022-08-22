using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaterHeight : MonoBehaviour
{
    // public BuildingTrigger script;
    public float Waterheight;
    public float Waterspeed;
    public float Timer = 300;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Timer = 20;
        }

        if (Timer > 20)
        {
            Timer -= Time.deltaTime;
        }

        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * Waterspeed * Waterheight);
        }
    }
}
