/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class GestureTrainer : MonoBehaviour
{
    public Recording recordingToTrain;

    private float startTime;
    private int currentStateIndex = 0;

    public AudioSource trainingAudio;

    private void Start()
    {
        trainingAudio = GetComponent<AudioSource>(); // may need to be more specific
        recordingToTrain = null;
    }

    private float GetCurrentTime()
    {
        return Time.time - startTime;
    }

    private void ToggleTraining()
    {
        recordingToTrain = null;
        foreach (Recording recording in GestureManager.inst.recordings)
        {
            if (recording.visible)
            {
                recordingToTrain = recording;
                break;
            }
        }

        if (recordingToTrain != null)
        {
            startTime = Time.time;
            currentStateIndex = 0;
            recordingToTrain.Tune(currentStateIndex, GestureManager.inst.head, GestureManager.inst.leftHand, GestureManager.inst.rightHand);
            currentStateIndex++;
            trainingAudio.Play();
            recordingToTrain.adjustments++;
            RecordingUI.inst.showRecordingDetails();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.Train.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ToggleTraining();
        }

        if (recordingToTrain != null)
        {
            if (currentStateIndex >= recordingToTrain.recordedStates.Count) {
                recordingToTrain = null;
                trainingAudio.Play();
            }
            else if (GetCurrentTime() > recordingToTrain.recordedStates[currentStateIndex].stateTime)
            {
                recordingToTrain.Tune(currentStateIndex, GestureManager.inst.head, GestureManager.inst.leftHand, GestureManager.inst.rightHand);
                currentStateIndex++;
            }

        }
    }
}
