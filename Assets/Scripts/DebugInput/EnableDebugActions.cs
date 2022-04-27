using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnableDebugActions : MonoBehaviour
{
    public InputActionAsset actionAsset;
    void OnEnable()
    {
        actionAsset.Enable();
    }
}
