using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingUI : MonoBehaviour
{
    public static RecordingUI inst;
    public Recording currentRecording = null;
    public List<Text> updatingText;
    private int recordingIndex = -1;

    public void Awake()
    {
        inst = this;
    }

    public void Start()
    {
        showRecordingDetails();

    }

    public void SelectNext()
    {
        recordingIndex++;
        if (recordingIndex > GestureManager.inst.recordings.Count - 1)
        {
            recordingIndex = -1;
            currentRecording = null;
        } else
        {
            currentRecording = GestureManager.inst.recordings[recordingIndex];

        }
        showRecordingDetails();
        GestureManager.inst.ShowRecording(recordingIndex);
    }

    public void SelectPrevious()
    {
        recordingIndex--;
        if (recordingIndex < -1)
        {
            recordingIndex = GestureManager.inst.recordings.Count - 1;
        }

        if (recordingIndex == -1)
        {
            currentRecording = null;
        }
        else
        {
            currentRecording = GestureManager.inst.recordings[recordingIndex];
        }
        showRecordingDetails();
        GestureManager.inst.ShowRecording(recordingIndex);
    }

    public void SelectNextAction()
    {
        if (currentRecording != null)
        {
            for (int i = 0; i < ActionManager.inst.actions.Count; ++i)
            {
                if (ActionManager.inst.actions[i] == currentRecording.action)
                {
                    i++;
                    if (i == ActionManager.inst.actions.Count)
                    {
                        i = 0;
                    }
                    currentRecording.action = ActionManager.inst.actions[i];
                    updatingText[6].text = "Action: " + currentRecording.action.name;
                    break;
                }
            }
        }
    }

    public void SelectPreviousAction()
    {
        if (currentRecording != null)
        {
            for (int i = 0; i < ActionManager.inst.actions.Count; ++i)
            {
                if (ActionManager.inst.actions[i] == currentRecording.action)
                {
                    i--;
                    if (i < 0)
                    {
                        i = ActionManager.inst.actions.Count - 1;
                    }
                    currentRecording.action = ActionManager.inst.actions[i];
                    updatingText[6].text = "Action: " + currentRecording.action.name;
                    break;
                }
            }
        }
    }

    public void SelectLast()
    {
        recordingIndex = GestureManager.inst.recordings.Count - 1;
        currentRecording = GestureManager.inst.recordings[recordingIndex];
        showRecordingDetails();
    }

    public void ShowHideRecording()
    {
        currentRecording.SetVisuals(!currentRecording.visible);
    }

    public void IgnoreLeftHand()
    {
        currentRecording.ignoreLeftHand = !currentRecording.ignoreLeftHand;
        currentRecording.SetVisuals(true);

    }

    public void IgnoreRightHand()
    {
        currentRecording.ignoreRightHand = !currentRecording.ignoreRightHand;
        currentRecording.SetVisuals(true);
    }

    public void DeleteRecording()
    {
        GestureManager.inst.recordings.Remove(currentRecording);
        Destroy(currentRecording.gameObject);
        SelectNext();
    }

    public void showRecordingDetails()
    {
        if (recordingIndex == -1)
        {
            updatingText[0].text = "Recording: None";
            updatingText[1].text = "";
            updatingText[2].text = "";
            updatingText[3].text = "";
            updatingText[4].text = "";
            updatingText[5].text = "";
            updatingText[6].text = "";

        }
        else
        {
            updatingText[0].text = "Recording: " + currentRecording.name;
            updatingText[1].text = "Left Hand Tracking: " + (!currentRecording.ignoreLeftHand).ToString();
            updatingText[2].text = "Right Hand Tracking: " + (!currentRecording.ignoreRightHand).ToString();
            updatingText[3].text = "Number of adjustments: " + currentRecording.adjustments.ToString();
            updatingText[4].text = "Number of frames: " + currentRecording.recordedStates.Count.ToString();
            updatingText[5].text = "Number of completions: " + currentRecording.completions.ToString();
            updatingText[6].text = "Action: " + currentRecording.action.name;
        }
    }
}
