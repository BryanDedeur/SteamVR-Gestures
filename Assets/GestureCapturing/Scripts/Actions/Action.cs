/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public string name;

    public virtual void Play()
    {
        
    }

    public virtual void InterpolateToProgress(float percentage)
    {

    }

    public virtual void Cancel()
    {
        
    }
}
