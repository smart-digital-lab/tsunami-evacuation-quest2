using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float power = 0.7f;
    public float duration = 10.0f;
    // public Camera mainCamera;
    public float slowDownAmount = 1.0f;
    private bool shouldShake = false;

    private Vector3 startPosition;
    private float initialDuration;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        initialDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShake)
        {
            if(duration > 0)
            {
                transform.localPosition = startPosition + new Vector3 (Random.Range(-2,3), 0, Random.Range(-2, 3)) * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                transform.localPosition = startPosition;
            }
        }
    }

    public void DoShake()
    {
        startPosition = transform.localPosition;
        shouldShake = true;
    }
}
