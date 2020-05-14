/*
 * Author: Bryan Dedeurwaerder
 * Project: Unity Gesture Recognition
 * Date: 5/12/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollidableButton: MonoBehaviour
{
    private Button button;
    private RectTransform rect;
    private BoxCollider boxCollider;
    private float timer;

    private void Awake()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(rect.rect.width, rect.rect.height, 0.05f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (timer < 0)
        {
            button.onClick.Invoke();
            timer = .5f;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

}
