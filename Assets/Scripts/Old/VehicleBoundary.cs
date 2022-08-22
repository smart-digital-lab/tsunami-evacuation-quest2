using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBoundary : MonoBehaviour
{
    public GameObject VehicleText;
    // Start is called before the first frame update
    void Start()
    {
        // Text begins scene off
        VehicleText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VehicleBoundary"))
        {
            // Text is viewed on screen
            VehicleText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VehicleBoundary"))
        {
            // Text disappears off screen
            VehicleText.SetActive(false);
        }
    }


}
