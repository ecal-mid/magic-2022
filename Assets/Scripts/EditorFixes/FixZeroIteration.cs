using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class FixZeroIteration : MonoBehaviour
{
    void Start()
    {
        if (Application.isEditor)
        {
            GetComponent<TrackedPoseDriver>().updateType = TrackedPoseDriver.UpdateType.Update;
        }
    }

}
