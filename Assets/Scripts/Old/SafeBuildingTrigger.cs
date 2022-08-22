using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBuildingTrigger : MonoBehaviour
{

    private GameObject timer;
    private int countDownTime;

    public GameObject warningText;





    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Timer timerScript = timer.GetComponent<Timer>();
            timerScript.countDownStartValue = 0;
            warningText.SetActive(true);
        }


    }
}


