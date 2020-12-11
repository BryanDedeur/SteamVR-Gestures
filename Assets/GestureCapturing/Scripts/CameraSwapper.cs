using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CameraSwapper : MonoBehaviour
{
    public List<Camera> cameras;
    private int index = 0;

    void Update()
    {
        if (SteamVR_Actions._default.Camera.GetStateDown(SteamVR_Input_Sources.Any))
        {
            cameras[index].gameObject.SetActive(false);
            index++;
            if (index > cameras.Count - 1)
            {
                index = 0;
            }
            cameras[index].gameObject.SetActive(true);
        }
    }
}

