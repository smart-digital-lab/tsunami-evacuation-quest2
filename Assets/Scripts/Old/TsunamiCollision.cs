using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TsunamiCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // If the object collides with an object with tag wave, loads the gameover scene
        if (other.CompareTag("Wave"))
        {
            GameObject CollisionDetection = GameObject.Find("CollisionDetection");
            PlayerTracking ScreenshotScript = CollisionDetection.GetComponent<PlayerTracking>();
            ScreenshotScript.enabled = true;

            SceneManager.LoadScene(3);
        }
    }
}
