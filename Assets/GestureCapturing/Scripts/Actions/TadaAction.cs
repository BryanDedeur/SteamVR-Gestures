/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadaAction : Action
{
    TadaAction()
    {
        name = "Tada Sound";
    }

    public AudioSource audio;

    public override void Play()
    {
        audio.Play();
    }

    public override void InterpolateToProgress(float percentage)
    {

    }

    public override void Cancel()
    {

    }
}
