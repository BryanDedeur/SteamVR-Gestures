/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording : MonoBehaviour
{
    private static int count;
    public int ID;
    public List<VRBodyState> recordedStates;
    public bool visible = false;
    public bool capturingEnded = false;
    private int replayStateIndex = 0;
    private float replayTimeLocation = 0;

    public float progress;

    // transforms used for recording and comparing
    public Transform virtualHead = null;
    public Transform virtualLeftHand = null;
    public Transform virtualRightHand = null;

    // transforms used for visuals
    private Transform visualHead = null;
    private Transform visualLeftHand = null;
    private Transform visualRightHand = null;

    private Vector3 LHtravelVector;
    private Vector3 RHtravelVector;

    private List<Renderer> LHpoints;
    private List<Renderer> RHpoints;
    private GameObject LHPointFolder;
    private GameObject RHPointFolder;

    public int adjustments;
    public int completions;


    [Range(0,1)]
    public float prematureCompletionThreshold = .75f;

    //public float averageComparison;
    //private int comparisonAttempts;
    //private float totalComparisons;

    // ---------------------- options -------------------------//
    public bool ignoreHead = true;
    public bool ignoreLeftHand = false;
    public bool ignoreRightHand = false;

    private float recordingTime;

    // ---------------------- Actions -------------------------//

    public Action action;

    public void Awake()
    {
        count++;
        ID = count;
        name = "Recording" + ID;
        visualHead = Instantiate(GestureManager.inst.headVisual, transform).transform;
        visualLeftHand = Instantiate(GestureManager.inst.leftHandVisual, visualHead).transform;
        visualRightHand = Instantiate(GestureManager.inst.rightHandVisual, visualHead).transform;

        virtualHead = new GameObject().transform;
        virtualHead.name = "VirtualHead";
        virtualHead.parent = transform;

        virtualLeftHand = new GameObject().transform;
        virtualLeftHand.name = "VirtualLeftHand";
        virtualLeftHand.parent = virtualHead;

        virtualRightHand = new GameObject().transform;
        virtualRightHand.name = "VirtualRightHand";
        virtualRightHand.parent = virtualHead;

        LHpoints = new List<Renderer>();
        RHpoints = new List<Renderer>();

        LHPointFolder = new GameObject();
        LHPointFolder.transform.parent = visualHead;
        LHPointFolder.name = "LeftHandPointFolder";
        LHPointFolder.transform.localPosition = Vector3.zero;
        LHPointFolder.transform.localEulerAngles = Vector3.zero;
        RHPointFolder = new GameObject();
        RHPointFolder.transform.parent = visualHead;
        RHPointFolder.name = "RightHandPointFolder";
        RHPointFolder.transform.localPosition = Vector3.zero;
        RHPointFolder.transform.localEulerAngles = Vector3.zero;

        action = ActionManager.inst.actions[0];

        recordedStates = new List<VRBodyState>();
        recordingTime = 0;
    }

    public void GlobalMirrorTransform(Transform changingTransform, Transform staticTransform)
    {
        changingTransform.position = staticTransform.position;
        changingTransform.eulerAngles = staticTransform.eulerAngles;
    }

    public void MirrorVirtualComponents(Transform head, Transform leftHand, Transform rightHand)
    {
        GlobalMirrorTransform(virtualHead, head);
        GlobalMirrorTransform(virtualLeftHand, leftHand);
        GlobalMirrorTransform(virtualRightHand, rightHand);
    }

    public void MirrorVisualComponents(Transform head, VRBodyState otherState)
    {
        visualHead.position = head.position;
        visualHead.eulerAngles = head.eulerAngles;
        visualLeftHand.localEulerAngles = otherState.LeftHandEulerAngles;
        visualLeftHand.localPosition = otherState.LeftHandVectorFromHead;
        visualRightHand.localEulerAngles = otherState.RightHandEulerAngles;
        visualRightHand.localPosition = otherState.RightHandVectorFromHead;
    }

    public void Tune(int index, Transform head, Transform leftHand, Transform rightHand)
    {
        MirrorVirtualComponents(head, leftHand, rightHand);
        recordedStates[index].Tune(virtualHead, virtualLeftHand, virtualRightHand);

        LHpoints[index].transform.localScale = Vector3.one * recordedStates[index].aveVectorDifLH;
        RHpoints[index].transform.localScale = Vector3.one * recordedStates[index].aveVectorDifRH;
    }

    private void PlotPoints()
    {
        GameObject LHpoint = Instantiate(GestureManager.inst.pointPrefab, LHPointFolder.transform);
        MeshRenderer LHrenderer = LHpoint.GetComponent<MeshRenderer>();
        LHpoints.Add(LHrenderer);

        LHpoint.transform.localPosition = recordedStates[recordedStates.Count - 1].LeftHandVectorFromHead;
        LHpoint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        LHpoint.SetActive(!ignoreLeftHand);

        GameObject RHpoint = Instantiate(GestureManager.inst.pointPrefab, RHPointFolder.transform);
        MeshRenderer RHrenderer = RHpoint.GetComponent<MeshRenderer>();
        RHpoints.Add(RHrenderer);

        RHpoint.transform.localPosition = recordedStates[recordedStates.Count - 1].RightHandVectorFromHead;
        RHpoint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        RHpoint.SetActive(!ignoreLeftHand);

        UnhighlightLeftHandProgress();
        UnhighlightRightHandProgress();
    }

    private Vector3 lastLHPos = new Vector3();
    private Vector3 lastRHPos = new Vector3();
    public void CaptureState(Transform head, Transform leftHand, Transform rightHand)
    {
        VRBodyState newState = new VRBodyState();
        MirrorVirtualComponents(head, leftHand, rightHand);
        newState.Capture(recordingTime, virtualHead, virtualLeftHand, virtualRightHand);
        recordedStates.Add(newState);

        PlotPoints();
    }

    public void HighlightLeftHandProgress(int index)
    {
        if (!ignoreLeftHand && visible)
        {
            if (index < LHpoints.Count - 1)
            {
                LHpoints[index].material.SetColor("_Color", new Color(1, 0, 0, .5f));
                LHpoints[index].material.SetColor("_EmissionColor", new Color(1, 0, 0, .5f));

            }
        }
    }

    public void HighlightRightHandProgress(int index)
    {
        if (!ignoreRightHand && visible)
        {
            if (index < RHpoints.Count - 1)
            {
                RHpoints[index].material.SetColor("_Color", new Color(1, 0, 0, .5f));
                RHpoints[index].material.SetColor("_EmissionColor", new Color(1, 0, 0, .5f));
            }
        }
    }

    public void UnhighlightLeftHandProgress()
    {
        if (!ignoreLeftHand)
        {
            foreach (MeshRenderer mr in LHpoints)
            {
                mr.material.SetColor("_Color", new Color(1, 1, 1, .1f));
                mr.material.SetColor("_EmissionColor", new Color(1, 1, 1, .1f));

            }
        }
    }

    public void UnhighlightRightHandProgress()
    {
        if (!ignoreRightHand)
        {
            foreach(MeshRenderer mr in RHpoints)
            {
                mr.material.SetColor("_Color", new Color(1, 1, 1, .1f));
                mr.material.SetColor("_EmissionColor", new Color(1, 1, 1, .1f));
            }
        }
    }

    public void UnhighlightProgress()
    {
        if (visible) {
            UnhighlightRightHandProgress();
            UnhighlightLeftHandProgress();
            progress = 0;
        }

    }

    // Evaluates the recording to adjust properties once all states are recorded
    public void EndRecording()
    {

        Vector3 lastLHVec = Vector3.zero;
        Vector3 lastRHVec = Vector3.zero;
        foreach (VRBodyState state in recordedStates)
        { 
            if (state != recordedStates[0])
            {
                LHtravelVector += (lastLHVec - state.LeftHandVectorFromHead);
                RHtravelVector += (lastRHVec - state.RightHandVectorFromHead);
            }
            lastLHVec = state.LeftHandVectorFromHead;
            lastRHVec = state.RightHandVectorFromHead;
        }

        if (LHtravelVector.magnitude / recordingTime < .035f)
        {
            ignoreLeftHand = true;
        }
        print("Left Hand Travel Distance Over Time:" + LHtravelVector.magnitude / recordingTime);

        if (RHtravelVector.magnitude / recordingTime < .035f)
        {
            ignoreRightHand = true;
        }
        print("Right Hand Travel Distance Over Time:" + RHtravelVector.magnitude / recordingTime);

        capturingEnded = true;
        visible = true;
        SetVisuals(true);
        GestureManager.inst.recordings.Add(this);

    }

    public void SetVisuals(bool state)
    {
        if (state)
        {
            visualHead.gameObject.SetActive(state);
            visualLeftHand.gameObject.SetActive(!ignoreLeftHand);
            visualRightHand.gameObject.SetActive(!ignoreRightHand);
            LHPointFolder.SetActive(!ignoreLeftHand);
            RHPointFolder.SetActive(!ignoreRightHand);
        } else
        {
            visualHead.gameObject.SetActive(state);
        }
        visible = state;
    }

    // returns hows close the state is to the specified state index
    public bool Compare(int stateIndex, Transform head, Transform leftHand, Transform rightHand)
    {
        if (ignoreHead && ignoreLeftHand && ignoreRightHand)
        {
            return false;
        }

        bool[] ignoreItems = new bool[3];
        ignoreItems[0] = ignoreHead;
        ignoreItems[1] = ignoreLeftHand;
        ignoreItems[2] = ignoreRightHand;

        VRBodyState otherState = new VRBodyState();
        MirrorVirtualComponents(head, leftHand, rightHand);
        otherState.Capture(recordedStates[stateIndex].stateTime, virtualHead, virtualLeftHand, virtualRightHand);

        if (!recordedStates[stateIndex].CompareVectors(otherState, ignoreItems))
        {
            return false;
        } else
        {
            HighlightLeftHandProgress(stateIndex);
            HighlightRightHandProgress(stateIndex);
            progress = (float) stateIndex / (float) recordedStates.Count;
            action.InterpolateToProgress(progress);
        }

        // also compare angles

        return true;
    }

    // Plays the recording back if replay is set to true
    private void Update()
    {
        recordingTime += Time.deltaTime;

        if (visible)
        {
            replayTimeLocation += Time.deltaTime;
            if (replayTimeLocation > recordedStates[replayStateIndex].stateTime)
            {
                replayStateIndex++;
            }

            if (replayStateIndex >= recordedStates.Count)
            {
                replayStateIndex = 0;
                replayTimeLocation = 0;
            }

            MirrorVisualComponents(GestureManager.inst.head, recordedStates[replayStateIndex]);
        }
        //MirrorVirtualComponents(GestureManager.inst.head, GestureManager.inst.leftHand, GestureManager.inst.rightHand);
    }

}
