using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTrigger : MonoBehaviour
{
    public int penalise = 30;
    public bool enter;

    public int FixedWarningTime = 200;
    private int warningTime;
    public GameObject warningText;

    // Start is called before the first frame update
    void Start()
    {
        warningTime = FixedWarningTime;
        enter = false;   
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player interacts with the cube
        if (other.CompareTag("Player"))
        {
            // Reduces the evacuation time by 30s
            GameObject OVRPlayerController = GameObject.Find("OVRPlayerController");
            Timer timerScript = OVRPlayerController.GetComponent<Timer>();
            timerScript.countDownStartValue -= penalise;
            enter = true;

            // Reduces the time till tsunami wave by 30s
            GameObject TsunamiWave = GameObject.Find("TsunamiWave");
            WaterHeight tsunamiScript = TsunamiWave.GetComponent<WaterHeight>();
            tsunamiScript.Timer -= penalise;
        }
    }

    private void Update()
    {
        if (enter == true)
        {
            if (warningTime > 0)
            {
                warningTime -= 1;
                warningText.SetActive(true);
            }    
        }

        if (warningTime == 0)
        {
            warningText.SetActive(false);
            enter = false;
            warningTime = FixedWarningTime;
        }
    }
}


