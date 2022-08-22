/**********************************************************************************************************************************************************
 * XRUX_SendMessageOnCollision
 * ---------------------------
 *
 * 2021-12-08
 * 
 * Send a message to the console on collision with something.
 * 
 * Roy Davies, Smart Digital Lab, University of Auckland.
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Public functions
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public interface IXRUX_SendMessageOnCollision
{
}
// ----------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Main class
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
[AddComponentMenu("OpenXR UX/Tools/XRUX Send Message on Collision")]
public class XRUX_SendMessageOnCollision : MonoBehaviour, IXRUX_SendMessageOnCollision
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public string theMessage = "";
    public string theEvent = "";
    public GameObject safePlace;
    public GameObject theXRRig;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private XRDeviceEvents eventQueue;  // The XR Event Queue
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set up the variables ready to go.
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        // Listen for events coming from the XR Controllers and other devices
        eventQueue = XRRig.EventQueue;
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Foot")
        {
            Send (new XRData(theMessage), new XRData(theEvent));
            if ((theXRRig != null) && (safePlace != null))
            {
                XRRig_CameraMover theCameraMover = theXRRig.GetComponent<XRRig_CameraMover>();
                if (theCameraMover != null)
                {
                    theCameraMover.MoveTo(safePlace.transform.position);
                }
            }
        }
    }

    // Warning text disappears when player leaves geofence boundary
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Foot")
        {
            Send (new XRData(""), new XRData(""));
        }
    }



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Send the data to the console
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Send (XRData theData, XRData theEvent)
    {
        XREvent eventToSend = new XREvent();
        eventToSend.eventType = XRDeviceEventTypes.console;
        eventToSend.eventAction = XRDeviceActions.CHANGE;
        eventToSend.data = theData;
        if (eventQueue != null) eventQueue.Invoke(eventToSend);

        XREvent eventToSend2 = new XREvent();
        eventToSend2.eventType = XRDeviceEventTypes.message;
        eventToSend2.eventAction = XRDeviceActions.CHANGE;        
        eventToSend2.data = theEvent;
        if (eventQueue != null) eventQueue.Invoke(eventToSend2);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}

