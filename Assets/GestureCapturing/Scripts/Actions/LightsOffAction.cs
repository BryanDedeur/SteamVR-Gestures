/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOffAction : Action
{
    LightsOffAction()
    {
        name = "Lights Off";
    }

    public AudioSource switchAudio;

    public override void Play()
    {

    }

    public override void InterpolateToProgress(float percentage)
    {
        LightManager.inst.SetLightBrightness(1 - percentage);
        if (1 - percentage < .2f)
        {
            if (!switchAudio.isPlaying)
            {
                switchAudio.Play();
            }
        }
    }

    public override void Cancel()
    {

    }
}
