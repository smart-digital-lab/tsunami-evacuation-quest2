using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CubeOnHill : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject CollisionDetection = GameObject.Find("CollisionDetection");

            // Removes "touch the cube to evacuate" text
            MtDruryText MtDruryScript = CollisionDetection.GetComponent<MtDruryText>();
            MtDruryScript.WarningText.SetActive(false);

            // Accesses PlayerTracking gameobject -> script and enables
            PlayerTracking ScreenshotScript = CollisionDetection.GetComponent<PlayerTracking>();
            ScreenshotScript.enabled = true;
            SceneManager.LoadScene(3);
        }
    }
}
