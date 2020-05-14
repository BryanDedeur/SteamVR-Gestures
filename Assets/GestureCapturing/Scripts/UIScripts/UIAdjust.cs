/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdjust : MonoBehaviour
{
    public bool maximize = false;
    public float delay = 5;
    public float speed = .001f;
    public RectTransform rect;

    private float goal;

    // Start is called before the first frame update
    void Start()
    {
        if (maximize)
        {
            goal = rect.localScale.x;
            rect.localScale = Vector3.zero;
        } else
        {
            goal = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (delay < 0)
        {
            if (maximize)
            {
                if (rect.localScale.x < goal)
                {
                    rect.localScale += Vector3.one * speed * Time.deltaTime;
                }
                else
                {
                    Destroy(this);
                }
            }
            else
            {
                if (rect.localScale.x > goal)
                {
                    rect.localScale -= Vector3.one * speed * Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        delay -= Time.deltaTime;
    }
}
