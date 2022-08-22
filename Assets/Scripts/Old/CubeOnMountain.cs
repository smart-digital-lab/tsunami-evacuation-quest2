using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CubeOnMountain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Accesses PlayerTracking gameobject -> script and enables
            GameObject CollisionDetection = GameObject.Find("CollisionDetection");
            PlayerTracking ScreenshotScript = CollisionDetection.GetComponent<PlayerTracking>();
            ScreenshotScript.enabled = true;

            // Skips to successful evacuation scene
            SceneManager.LoadScene(2);
        }
    }
}
