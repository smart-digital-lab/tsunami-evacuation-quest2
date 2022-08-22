using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    private GameObject Camera;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Camera = GameObject.FindGameObjectWithTag("CameraShake");
            CameraShake CameraShakescript = Camera.GetComponent<CameraShake>();
            CameraShakescript.DoShake();
        }
    }

}
