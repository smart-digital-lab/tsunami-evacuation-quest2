using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    public float movementSpeed = 20f;
    public bool reachDestination = false;
    public Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == destination)
        {
            reachDestination = true;
            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);

        }
    }
}

