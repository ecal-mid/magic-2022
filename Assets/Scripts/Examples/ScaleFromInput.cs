using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.MagicLeap;

public class ScaleFromInput : MonoBehaviour
{
    public InputAction button;
    public InputAction encoder;
   private float startScaleY;

    // Start is called before the first frame update
    void Start()
    {
        startScaleY = transform.localScale.y;
        button.Enable();
        encoder.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        scale.y = startScaleY + encoder.ReadValue<float>()*20f;
        transform.localScale = scale;


        var pos = transform.localPosition;
        pos.y = -button.ReadValue<float>()*.05f;
        transform.localPosition = pos; 
    }
}