using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
    [SerializeField]
    public float movementSpeed = 20f;
    public bool reachedDestination = false;

    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;

        if(currentPosition != destination)
        {
            reachedDestination = false;
            transform.position = Vector3.MoveTowards(currentPosition, destination, movementSpeed * Time.deltaTime);
           
            
        }
        else
        {
            reachedDestination = true;
        }


    }
}



