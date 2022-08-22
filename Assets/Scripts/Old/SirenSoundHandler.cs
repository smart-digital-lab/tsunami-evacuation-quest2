using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenSoundHandler : MonoBehaviour
{

    public List<AudioSource> sirenSounds;
    private GameObject player;
    public float volumeReductionSpeed = .5f;

    void Start()
    {
        //Find all of the Siren AudioSources 
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SirenSound"))
        {
            sirenSounds.Add(obj.GetComponent<AudioSource>());
        }

        player = GameObject.Find("FPSController");
    }

    void Update()
    {
        DetermineClosestSirenSound();
    }

    void DetermineClosestSirenSound()
    {
        float distance = float.MaxValue;
        AudioSource closestSirenSound = null;
        //Determine which Siren sound is the closest
        foreach (AudioSource SirenSound in sirenSounds)
        {
            float newDistance = Vector3.Distance(player.transform.position, SirenSound.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                closestSirenSound = SirenSound;
            }
        }

        DisableDistantSirenSounds(closestSirenSound);
    }

    //Scale up the closest Siren sound's volume until it's reached 1.
    //Scale down the volume of any Siren sound that isn't closest.
    void DisableDistantSirenSounds(AudioSource closestSirenSource)
    {
        foreach (AudioSource SirenSound in sirenSounds)
        {
            if (SirenSound != closestSirenSource)
            {
                SirenSound.volume -= volumeReductionSpeed * Time.deltaTime;
            }
            else
            {
                SirenSound.volume += volumeReductionSpeed * Time.deltaTime;
            }
        }
    }
}
