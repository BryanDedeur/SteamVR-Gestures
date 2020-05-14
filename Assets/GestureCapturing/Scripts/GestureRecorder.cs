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


public class GestureRecorder : MonoBehaviour
{
    private bool recording = false;
    private Recording currentRecording;

    public float captureFrequencySeconds;
    private float captureTimer;

    public Material visualRenderMaterial;
    public Gradient visuallineColor;
    public Material lineRenderMaterial;
    public Gradient lineColor;

    private LineRenderer headLineRenderer;
    private LineRenderer leftHandLineRenderer;
    private LineRenderer rightHandLineRenderer;

    public AudioSource recordingAudio;

    private void setLineRendererProperties(LineRenderer lr, Material mat)
    {
        lr.material = mat;
        lr.colorGradient = lineColor;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
    }

    private void Start()
    {
        recordingAudio = GetComponent<AudioSource>(); // may need to be more specific
        currentRecording = new Recording();
        if (headLineRenderer == null)
        {
            //headLineRenderer = head.gameObject.AddComponent<LineRenderer>();
            //headLineRenderer.colorGradient = lineColor;
        }

        if (leftHandLineRenderer == null)
        {
            leftHandLineRenderer = GestureManager.inst.leftHand.gameObject.AddComponent<LineRenderer>();
            setLineRendererProperties(leftHandLineRenderer, lineRenderMaterial);
        }

        if (rightHandLineRenderer == null)
        {
            rightHandLineRenderer = GestureManager.inst.rightHand.gameObject.AddComponent<LineRenderer>();
            setLineRendererProperties(rightHandLineRenderer, lineRenderMaterial);
        }
    }


    public void ToggleRecording()
    {
        // creates new recording gameObject with appropriate components
        if (currentRecording == null)
        {
            GameObject go = new GameObject();
            go.transform.position = transform.position;
            currentRecording = go.AddComponent<Recording>();
        } else if (recording == false && currentRecording.recordedStates.Count > 0)
        {
            GestureManager.inst.HideAllRecordings();
            GameObject go = new GameObject();
            go.transform.position = transform.position;
            currentRecording = go.AddComponent<Recording>();
        }

        recording = !recording;
        //recordingAudio.Play(0);

        if (currentRecording.recordedStates.Count == 0)
        {
            print("Recording started.");
            recordingAudio.Play();
        }
        else
        {
            currentRecording.EndRecording();
            leftHandLineRenderer.positionCount = 0;
            rightHandLineRenderer.positionCount = 0;
            RecordingUI.inst.SelectLast();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.Record.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ToggleRecording();
        }

        if (recording)
        {
            if (captureTimer < 0)
            {
                captureTimer = captureFrequencySeconds;
                currentRecording.CaptureState(GestureManager.inst.head, GestureManager.inst.leftHand, GestureManager.inst.rightHand);

                if (currentRecording.recordedStates.Count > 1)
                {
                    leftHandLineRenderer.positionCount = currentRecording.recordedStates.Count - 1;
                    rightHandLineRenderer.positionCount = currentRecording.recordedStates.Count - 1;

                    leftHandLineRenderer.SetPosition(currentRecording.recordedStates.Count - 2, GestureManager.inst.leftHand.position);
                    rightHandLineRenderer.SetPosition(currentRecording.recordedStates.Count - 2, GestureManager.inst.rightHand.position);
                }
            }
            captureTimer -= Time.deltaTime;

        }
    }
}
