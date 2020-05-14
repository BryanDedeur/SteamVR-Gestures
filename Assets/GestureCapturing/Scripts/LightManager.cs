/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager inst;

    private void Awake()
    {
        inst = this;
    }

    public List<Light> lights;

    public void SetLightBrightness(float percentage)
    {
        foreach(Light light in lights)
        {
            light.intensity = percentage;
        }
    }
}

