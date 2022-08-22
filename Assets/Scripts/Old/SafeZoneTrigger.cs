using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR;

public class SafeZoneTrigger : MonoBehaviour
{



    private GameObject timer;
    private int countDownTime;

    public GameObject SafeText;
    public GameObject DieText;

    public bool inTheSafeZone = false;




    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
    }



    void Update()
    {

        //OVRInput.Update();


        // press button "A" on right controller
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("get button down ");
            Timer timerScript = timer.GetComponent<Timer>();
            timerScript.countDownStartValue = 0;
            Debug.Log("timer gets to zero ");
            

            if (inTheSafeZone)
            {
                SafeText.SetActive(true);
                //DieText.SetActive(false);
                Debug.Log("safe");
            }

            //else 
            //{
                //DieText.SetActive(true);
               // SafeText.SetActive(false);
               // Debug.Log("die");
            //}

        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTheSafeZone = true;
            Debug.Log("in the safe zone ");
        }
    }


    //void OnTriggerExit(Collider other)
    //{
        //if (other.gameObject.tag == "Player")
        //{
         //   inTheSafeZone = false;
         //   Debug.Log("outside the safe zone ");
        //}
    //}
   }

