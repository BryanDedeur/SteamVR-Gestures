using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureUITextUpdater : MonoBehaviour
{
    public List<Text> textElements;
    public float updateFrequency = 1f;
    private float currentTime;

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            if (GestureManager.inst.detectedRecording == null)
            {
                if (GestureManager.inst.timeRemaining > 0)
                {
                    textElements[0].text = "Gesture Completed! Wait...";
                }
                else
                {
                    textElements[0].text = "Listening for gesture...";
                }
            } else
            {
                textElements[0].text = "Gesture Detected: " + GestureManager.inst.detectedRecording.name;
            }

            currentTime = updateFrequency;
        }


    }
}
