/**********************************************************************************************************************************************************
 * XRData_Random
 * -------------
 *
 * 2021-09-01
 *
 * Creates a random number between a given range.
 *
 * Roy Davies, Smart Digital Lab, University of Auckland.
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Public functions
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public interface IXRData_Random
{
    void Go();
}
// ----------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Main class
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
[AddComponentMenu("OpenXR UX/Connectors/XRData Random")]
public class XRData_Random : MonoBehaviour, IXRData_Random
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public float minValue = 0.0f;
    public float maxValue = 10.0f;
    public bool sendOnStart = false;
     public UnityXRDataEvent onChange;

    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private bool firstTime = true;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (firstTime)
        {
            firstTime = false;
            if (sendOnStart) Go();
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------


    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Send a random number
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Go()
    {
        float randomNumber = Random.Range(minValue, maxValue);
        if (onChange != null) onChange.Invoke(new XRData(randomNumber));
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}