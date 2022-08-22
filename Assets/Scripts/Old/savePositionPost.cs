using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class savePositionPost : MonoBehaviour
{

    int time = 0;
    int interval = 50;
    string path = "Assets/Record movement/positionFilePost.txt";
    public float Timer = 0;

    // Use this for initialization
    void Start()
    {
        // Separates each run
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine(" ");
        sw.WriteLine("PLAYER RUN STARTS");
        sw.WriteLine("X Coordinates, Z Coordinates, Time (Seconds)");
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > interval)
        {
            // Obtains players coordinates at each interval and prints x and z coordinates
            float x = transform.position.x;
            float z = transform.position.z;

            // Copies the x and z coordinates to a text file
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(x + "," + z + "," + Timer);
            sw.Close();
            Timer += Time.time;
            time = 0;
        }
        else
            time++;
            Timer += Time.deltaTime;   
    }

    private void OnApplicationQuit()
    {
        // Ends each players run
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine("PLAYER RUN ENDS");
        sw.WriteLine(" ");
        sw.Close();
    }
}

