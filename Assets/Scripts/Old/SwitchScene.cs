using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    
    public int waitingTime = 20;
    public int switchTime = 30;
    private GameObject theCamera;
    private bool shake = true;
    private bool switching = true;
    public AudioSource audioSource;
    private float startTime;
    public CameraShake CameraShakescript;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //if (waitingTime > 0)
        //{
        //    waitingTime -= 1;
        //
        //}

        //if (waitingTime == 0 && shake)
        if (((Time.time - startTime) > waitingTime) && shake)
        {
            //theCamera = GameObject.FindGameObjectWithTag("CameraShake");
            //CameraShake CameraShakescript = theCamera.GetComponent<CameraShake>();
            CameraShakescript.DoShake();
            shake = false;
            //Play audio when the shaking starts
            audioSource.Play();

        }

        //if (switchTime > 0)
        //{
        //    switchTime -= 1;
        //}

        //if (switchTime == 0 && switching)
        if (((Time.time - startTime) > switchTime) && switching)
        {
            //SceneManager.LoadScene(1);
            shake = false;
        }

    }
}
