using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePenalty : MonoBehaviour
{
    public int penalise = 30;
    public bool enter;

    public int FixedWarningTime = 200;
    private int warningTime;
    public GameObject VehiclePenalisation;

    private void Start()
    {
        VehiclePenalisation.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            // Reduces the evacuation time by 30s
            GameObject OVRPlayerController = GameObject.Find("OVRPlayerController");
            Timer timerScript = OVRPlayerController.GetComponent<Timer>();
            timerScript.countDownStartValue -= penalise;

            // Reduces the time till tsunami wave by 30s
            GameObject TsunamiWave = GameObject.Find("TsunamiWave");
            WaterHeight tsunamiScript = TsunamiWave.GetComponent<WaterHeight>();
            tsunamiScript.Timer -= penalise;

            enter = true;
            // Removes "touch vehicle to evacuate" text
            GameObject CollisionDetection = GameObject.Find("CollisionDetection");
            VehicleBoundary boundaryScript = CollisionDetection.GetComponent<VehicleBoundary>();
            boundaryScript.VehicleText.SetActive(false);
        }  
    }

    private void Update()
    {
        if (enter == true)
        {
            if (warningTime > 0)
            {
                warningTime -= 1;
                VehiclePenalisation.SetActive(true);
            }
        }

        if (warningTime == 0)
        {
            VehiclePenalisation.SetActive(false);
            enter = false;
            warningTime = FixedWarningTime;
        }
    }
}
