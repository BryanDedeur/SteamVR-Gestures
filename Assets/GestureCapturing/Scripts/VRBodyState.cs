using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR.InteractionSystem;

[Serializable]
public class VRBodyState
{
    // ------------------- Tolerance Stuff ------------------------ 
    // the number of trials to gain running averages of data
    public int trials = 0;

    public float aveVectorDifLH = 0.00001f; // cannot be zero;
    public float totalVectorDifLH = 0;

    public float aveVectorDifRH = 0.00001f; // cannot be zero;
    public float totalVectorDifRH = 0;

    public float aveAngleDifLH = 0.00001f; // cannot be zero;
    public float totalAngleDifLH = 0;

    public float aveAngleDifRH = 0.00001f; // cannot be zero;
    public float totalAngleDifRH = 0;

    public float stateTime;
    // Head ---------------------------------------------------

    // Right Hand ---------------------------------------------------
    // high importance metrics
    public Vector3 RightHandEulerAngles;
    public Vector3 RightHandVectorFromHead;

    // Left Hand ---------------------------------------------------
    // high importance metrics
    public Vector3 LeftHandVectorFromHead;
    public Vector3 LeftHandEulerAngles;

    public void Capture(float time, Transform head, Transform leftHand, Transform rightHand)
    {
        stateTime = time;
        // Head ---------------------------------------------------

        // Right Hand ---------------------------------------------------

        // high importance metrics
        RightHandEulerAngles = rightHand.localEulerAngles;
        RightHandVectorFromHead = rightHand.localPosition;

        // Left Hand ---------------------------------------------------

        // high importance metrics
        LeftHandEulerAngles = leftHand.localEulerAngles;
        LeftHandVectorFromHead = leftHand.localPosition;
    }

    public void Tune(Transform head, Transform leftHand, Transform rightHand)
    {
        trials++;

        // vector dif tuning 
        totalVectorDifLH += Vector3.Distance(LeftHandVectorFromHead, leftHand.localPosition);
        aveVectorDifLH = totalVectorDifLH / trials;
        if (aveVectorDifLH == 0)
            aveVectorDifLH = 0.001f;

        totalVectorDifRH += Vector3.Distance(RightHandVectorFromHead, rightHand.localPosition);
        aveVectorDifRH = totalVectorDifRH / trials;
        if (aveVectorDifRH == 0)
            aveVectorDifRH = 0.001f;

        // angle dif tuning
        totalAngleDifLH += Vector3.Distance(LeftHandEulerAngles, leftHand.localEulerAngles);
        aveAngleDifLH = totalAngleDifLH / trials;
        if (aveAngleDifLH == 0)
            aveAngleDifLH = 0.001f;

        totalAngleDifRH += Vector3.Distance(RightHandEulerAngles, rightHand.localEulerAngles);
        aveAngleDifRH = totalAngleDifRH / trials;
        if (aveAngleDifRH == 0)
            aveAngleDifRH = 0.001f;

    }

    public bool CompareVectors(VRBodyState otherState, bool[] ignoreItems)
    {
        float evaluation = 0f;
        int numVectors = 0;

        for (int i = 0; i < ignoreItems.Length; ++i)
        {
            if (!ignoreItems[i])
            {
                numVectors++;
            }
        }

        if (!ignoreItems[1]) // don't ignore left hand
        {
            float distance = Vector3.Distance(LeftHandVectorFromHead, otherState.LeftHandVectorFromHead);
            if (distance <= aveVectorDifLH)
            {
                evaluation += (1 - distance/aveVectorDifLH) / numVectors;
            } else
            {
                return false;
            }
        }

        if (!ignoreItems[2]) // don't ignore right hand
        {
            float distance = Vector3.Distance(RightHandVectorFromHead, otherState.RightHandVectorFromHead);
            if (distance <= aveVectorDifRH)
            {
                evaluation += (1 - distance / aveVectorDifRH) / numVectors;
            } else
            {
                return false;
            }
        }

        return true; 
    }

    public bool CompareAngles(VRBodyState otherState, bool[] ignoreItems)
    {
        float evaluation = 0f;
        int numVectors = 0;

        for (int i = 0; i < ignoreItems.Length; ++i)
        {
            if (!ignoreItems[i])
            {

                numVectors++;
            }
        }

        if (!ignoreItems[1]) // don't ignore left hand
        {
            float distance = Vector3.Distance(LeftHandEulerAngles, otherState.LeftHandEulerAngles);
            if (distance < aveAngleDifLH)
            {
                evaluation += (1 - distance / aveAngleDifLH) / numVectors;
            } else
            {
                return false;
            }
        }

        if (!ignoreItems[2]) // don't ignore right hand
        {
            float distance = Vector3.Distance(RightHandEulerAngles, otherState.RightHandEulerAngles);
            if (distance < aveAngleDifRH)
            {
                evaluation += (1 - distance / aveAngleDifRH) / numVectors;
            } else
            {
                return false;
            }
        }

        return true;
    }
}
