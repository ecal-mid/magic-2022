using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example_Input : MonoBehaviour
{
    public InputAction encoder;
    public InputAction button;

    void Awake()
    {
        encoder.Enable();
        button.Enable();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0,encoder.ReadValue<float>()*360,0);
        transform.localScale = Vector3.one * Mathf.Lerp(.1f,.2f, button.ReadValue<float>());
    }
}
