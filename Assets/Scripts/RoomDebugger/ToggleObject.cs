using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleObject : MonoBehaviour
{
    public InputActionProperty toggleAction;
    public GameObject targetObject;


    void Update()
    {
        if (toggleAction.action.WasPerformedThisFrame())
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}