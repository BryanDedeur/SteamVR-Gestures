/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    public static GestureManager inst;

    public GameObject player;

    public GameObject headVisual;
    public GameObject leftHandVisual;
    public GameObject rightHandVisual;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public GameObject pointPrefab;

    public float timeBetweenActions = 1;
    public float timeRemaining;

    public AudioSource tada;


    private void Awake()
    {
        inst = this;
        if (head == null)
        {
            head = player.transform.Find("VRCamera");
        }

        if (leftHand == null)
        {
            leftHand = player.transform.Find("LeftHand");
        }

        if (rightHand == null)
        {
            leftHand = player.transform.Find("RightHand");
        }

    }

    [Range(0,1)]
    //public float tollerance = .25f;

    public List<Recording> recordings;
    public Recording detectedRecording;
    private int detectedRecordingFrameIndex = 0;
    private float timeElaspedSinceRecordingFound = 0;

    public void ShowRecording(int index)
    {
        HideAllRecordings();
        if (index > -1 && index < recordings.Count)
        {
            recordings[index].SetVisuals(true);
        }
    }

    public void HideAllRecordings()
    {
        foreach(Recording recording in recordings)
        {
            recording.SetVisuals(false);
        }
    }

    private void Update()
    {
        timeElaspedSinceRecordingFound += Time.deltaTime;
        if (detectedRecording != null)
        {

            for (int i = detectedRecordingFrameIndex; i < detectedRecording.recordedStates.Count - 1; ++i)
            {
                if (detectedRecording.Compare(detectedRecordingFrameIndex, head, leftHand, rightHand))
                {
                    detectedRecordingFrameIndex++;

                }
                else if (timeElaspedSinceRecordingFound < detectedRecording.recordedStates[detectedRecordingFrameIndex].stateTime)
                {
                    break;
                }
                else
                {
                    detectedRecording.UnhighlightProgress();
                    detectedRecordingFrameIndex = 0;
                    detectedRecording = null;
                    break;
                }
            }

            if (detectedRecording != null)
            {
                // Check if the found recording is ready to be completed
                if (detectedRecordingFrameIndex >= detectedRecording.recordedStates.Count - 1)
                {
                    detectedRecording.UnhighlightProgress();
                    detectedRecording.action.Play();
                    detectedRecording.completions++;
                    RecordingUI.inst.showRecordingDetails();
                    timeRemaining = timeBetweenActions;
                    detectedRecordingFrameIndex = 0;
                    detectedRecording = null;
                }
            }
        } else
        {
            if (timeRemaining <= 0)
            {
                foreach (Recording recording in recordings)
                {
                    if (recording.capturingEnded)
                    {
                        if (recording.Compare(0, head, leftHand, rightHand))
                        {
                            detectedRecording = recording;
                            timeElaspedSinceRecordingFound = 0;
                        }
                    }
                }
            }
        }
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
        }

    }
}
