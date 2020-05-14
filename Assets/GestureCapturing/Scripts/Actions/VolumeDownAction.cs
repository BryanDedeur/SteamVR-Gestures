/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeDownAction : Action
{
    VolumeDownAction()
    {
        name = "Volume Down";
    }

    public AudioSource audio;


    public override void Play()
    {

    }

    public override void InterpolateToProgress(float percentage)
    {
        if (1 - percentage < audio.volume)
        {
            audio.volume = 1 - percentage;
        }
    }

    public override void Cancel()
    {

    }
}
