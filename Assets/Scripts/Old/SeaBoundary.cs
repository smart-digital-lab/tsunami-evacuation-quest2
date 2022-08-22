using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeaBoundary : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            enter = true;
        }

    }

    private void Update()
    {

        if (enter)
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


