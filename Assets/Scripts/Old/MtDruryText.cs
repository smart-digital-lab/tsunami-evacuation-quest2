using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MtDruryText : MonoBehaviour
{
    public GameObject WarningText;
    // Start is called before the first frame update
    void Start()
    {
        // Text begins scene off
        WarningText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Warning Sphere"))
        {
            WarningText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WarningText.SetActive(false);
    }
}
