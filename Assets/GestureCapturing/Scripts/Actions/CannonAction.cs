using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAction : Action
{
    CannonAction()
    {
        name = "Cannon Fire";
    }

    public AudioSource audio;
    public GameObject cannon1;
    public GameObject cannon2;
    public GameObject explosionPrefab;



    public override void Play()
    {
        audio.Play();
        Instantiate(explosionPrefab, cannon1.transform.position, cannon1.transform.rotation, cannon1.transform);
        Instantiate(explosionPrefab, cannon2.transform.position, cannon2.transform.rotation, cannon2.transform);

    }

    public override void InterpolateToProgress(float percentage)
    {

    }

    public override void Cancel()
    {

    }
}
