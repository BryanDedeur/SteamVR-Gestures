using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeUpAction : Action
{
    VolumeUpAction()
    {
        name = "Volume Up";
    }

    public AudioSource audio;

    private void Awake()
    {
        audio.time = 1800;
    }
    public override void Play()
    {

    }

    public override void InterpolateToProgress(float percentage)
    {
        if (percentage > audio.volume)
        {
            audio.volume = percentage;
        }
    }

    public override void Cancel()
    {

    }
}
