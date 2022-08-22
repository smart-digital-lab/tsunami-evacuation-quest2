using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeoFenceBoundary : MonoBehaviour
{
    public GameObject BoundaryText;

    // Start is called before the first frame update
    void Start()
    {
        // Text begins off
        BoundaryText.SetActive(false);
    }

    // Warning text appears on screen when player is touching the geofence
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("GeoFence"))
        {
            BoundaryText.SetActive(true);
        }
    }

    // Warning text disappears when player leaves geofence boundary
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GeoFence"))
        {
            BoundaryText.SetActive(false);
        }
    }
}