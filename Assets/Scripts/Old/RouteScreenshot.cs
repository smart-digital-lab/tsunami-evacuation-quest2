using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteScreenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // static void CaptureCamera(Camera camera, string path, int width, int height)
    static void CaptureCamera(Camera camera, string path, int width, int height)
    {
        var renderTexture = new RenderTexture(width, height, 16);
        var texture2D = new Texture2D(width, height);

        var target = camera.targetTexture;
        camera.targetTexture = renderTexture;
        camera.Render();
        camera.targetTexture = target;

        var active = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        RenderTexture.active = active;

        texture2D.Apply();
        System.IO.File.WriteAllBytes(path, texture2D.EncodeToJPG());
    }
}
