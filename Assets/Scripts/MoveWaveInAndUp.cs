/**********************************************************************************************************************************************************
 * TinyMovements
 * -------------
 *
 * 2021-10-05
 * 
 * Makes tiny rotational and positional movements somewhat randomly whilst moving a texture offset in a particular direction.
 * Quite good to give a large surface representing the sea a sense of movement.
 * 
 * Roy Davies, Smart Digital Lab, University of Auckland.
 **********************************************************************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct waveStep {
    public Vector3 position;
    public float timeSpan;
}

// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Main Class
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public class MoveWaveInAndUp : MonoBehaviour
{

    public waveStep[] waveSteps;
    public GameObject mainWave;

    private float startTime;                                        // Start of current time period
    private MeshRenderer theRenderer;                               // The object to move
    private Vector2 offset = Vector2.zero;                          // Texture offset
    private int currentStage;
    private bool rolling = false;



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set things up
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        theRenderer = mainWave.GetComponent<MeshRenderer>();
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Go()
    {
        currentStage = 0;
        startTime = Time.time;
        rolling = true;
        AudioSource waveSound = GetComponent<AudioSource>();
        if (waveSound != null)
        {
            waveSound.Play();
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Do the slow, tiny movements
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (rolling)
        {
            // Calculate the physical movement and rotation
            float step = (Time.time - startTime) / waveSteps[currentStage].timeSpan;
            transform.localPosition = Vector3.Lerp(waveSteps[currentStage].position, waveSteps[currentStage+1].position, step);

            // Calculate the texture movement
            if (mainWave != null)
            {
                float step2 = 0.1f * Time.deltaTime;
                offset = Vector2.MoveTowards(offset, Vector2.one, step2);
                if (offset == Vector2.one) offset = Vector2.zero;

                theRenderer.material.SetTextureOffset("_MainTex", offset);
            }

            // Check if time for a new set of numbers
            if (Time.time-startTime > waveSteps[currentStage].timeSpan)
            {
                if (currentStage < waveSteps.Length-2)
                {
                    currentStage ++;
                    startTime = Time.time;
                }
            } 
        } 
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}
// ----------------------------------------------------------------------------------------------------------------------------------------------------------