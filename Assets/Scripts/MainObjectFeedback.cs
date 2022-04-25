using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainObjectFeedback : MonoBehaviour
{
    public InputAction encoder1;
    public InputAction encoder2;
    public InputAction button;
    Vector3 scaleVel;

    void Awake()
    {
        encoder1.Enable();
        encoder2.Enable();
        button.Enable();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0,encoder1.ReadValue<float>()*10000,encoder2.ReadValue<float>()*10000);
        Vector3 targetScale = Vector3.one * Mathf.Lerp(.1f,.2f, button.ReadValue<float>());
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref scaleVel, 0.2f);
    }
}
