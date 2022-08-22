using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeightAndRotation : MonoBehaviour
{
    public float lowest = 0.9f;
    public float highest = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        float startingHeight = transform.localScale.y;
        transform.localScale = new Vector3(transform.localScale.x * Random.Range(0.9f, 1.1f), transform.localScale.y * Random.Range(lowest, highest), transform.localScale.z * Random.Range(0.9f, 1.1f));
        transform.localEulerAngles = new Vector3(0, Random.Range(0.0f, 90.0f), 0);
        // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 2.0f * (startingHeight - transform.localScale.y), transform.localPosition.z);
    }
}
