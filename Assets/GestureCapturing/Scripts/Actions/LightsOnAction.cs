/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOnAction : Action
{
    LightsOnAction()
    {
        name = "Lights On";
    }
    public AudioSource switchAudio;


    public override void Play()
    {

    }

    public override void InterpolateToProgress(float percentage)
    {
        LightManager.inst.SetLightBrightness(percentage);
        if (percentage < .2)
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
