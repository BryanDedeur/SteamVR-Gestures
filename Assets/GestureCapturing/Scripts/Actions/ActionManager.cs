/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager inst;

    public void Awake()
    {
        inst = this;
        foreach(Action action in GetComponents(typeof(Action)))
        {
            actions.Add(action);
        }
    }

    public List<Action> actions;

}
