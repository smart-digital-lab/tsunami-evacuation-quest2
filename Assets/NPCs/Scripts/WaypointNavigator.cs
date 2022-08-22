using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    NPCController controller;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        controller = GetComponent<NPCController>();
    }

    // Start is called before the first frame update
    void Start()
    {               
        controller.destination = currentWaypoint.transform.position;
    }

    void Update()
    {
        if(controller.reachDestination)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
        }
        

    }

}
