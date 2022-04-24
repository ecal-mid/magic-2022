using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ImageTrackingSystem : MonoBehaviour
{
    [System.Serializable]
    public class ImageTargetInfo
    {
        public string Name;
        public Texture2D Image;
        public bool isStationary;
        public float LongerDimension;
    }
    
    public GameObject prefab;

    private bool targetsInitialized = false;
    private Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
    public List<ImageTargetInfo> imageTargetInfos = new List<ImageTargetInfo>();
    public List<MLImageTracker.Target> imageTargets = new List<MLImageTracker.Target>();
    private Dictionary<string, MLImageTracker.Target.Result> trackingState = new Dictionary<string, MLImageTracker.Target.Result>();

    bool trackingActive = true;

    enum State
    {
        Inactive,
        Active,
        PrivilegeDenied,
        CameraUnavailable,
    }

    State currentTrackingState;

    enum PrivilegeState
    {
        Inactive,
        WaitingForPrivilege,
        PrivilegeAccepted,
        PrivilegeDenied,
    }

    PrivilegeState currentPrivilegeState = PrivilegeState.Inactive;

    void Start()
    {
        ActivatePrivileges();
    }

    void Update()
    {
        UpdateTrackingState();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        trackingActive = !pauseStatus;
    }

    private void ActivatePrivileges()
    {
        // If privilege was not already denied by User:
        if (currentPrivilegeState != PrivilegeState.PrivilegeDenied)
        {
            currentPrivilegeState = PrivilegeState.WaitingForPrivilege;
            // Try to get the component to request privileges
            MLPrivilegeRequesterBehavior requesterBehavior = GetComponent<MLPrivilegeRequesterBehavior>();
            if (requesterBehavior == null)
            {
                // No Privilege Requester was found, add one and setup for a CameraCapture request
                requesterBehavior = gameObject.AddComponent<MLPrivilegeRequesterBehavior>();
                requesterBehavior.Privileges = new[]
                {
                    MLPrivileges.RuntimeRequestId.CameraCapture
                };
            }

            // Listen for the privileges done event
            requesterBehavior.OnPrivilegesDone += HandlePrivilegesDone;
            requesterBehavior.enabled = true; // Component should be disabled in the editor until requested, this is discussed further below
        }
    }

    void HandlePrivilegesDone(MLResult result)
    {
        // Unsubscribe from future requests for privileges now that this one has been handled.
        GetComponent<MLPrivilegeRequesterBehavior>().OnPrivilegesDone -= HandlePrivilegesDone;

        if (result.IsOk)
        {
            // The privilege was accepted, the service can begin
            currentPrivilegeState = PrivilegeState.PrivilegeAccepted;
        }
        else
        {
            Debug.LogError("Camera Privilege Denied or Not Present in Manifest");
            currentPrivilegeState = PrivilegeState.PrivilegeDenied;
        }
    }


    void UpdateTrackingState()
    {
        State targetState = currentTrackingState;
        switch (currentTrackingState)
        {
            case State.Inactive:
                if (trackingActive)
                {
                    switch (currentPrivilegeState)
                    {
                        case PrivilegeState.PrivilegeAccepted:
                        {
                            var result = MLImageTracker.Enable();
                            if (result.IsOk)
                                targetState = State.Active;
                            else
                                targetState = State.CameraUnavailable;
                            break;
                        }
                        case PrivilegeState.PrivilegeDenied:
                            targetState = State.PrivilegeDenied;
                            break;
                    }
                }

                break;
            case State.Active:
                if (!trackingActive)
                {
                    MLImageTracker.Disable();
                    targetState = State.Inactive;
                }

                break;
            case State.PrivilegeDenied:
                break;
            case State.CameraUnavailable:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (targetState != currentTrackingState)
        {
            // LEAVE STATE
            switch (currentTrackingState)
            {
                case State.Inactive:
                    break;
                case State.Active:
                    break;
                case State.PrivilegeDenied:
                    break;
                case State.CameraUnavailable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentTrackingState = targetState;
            // ENTER STATE
            switch (currentTrackingState)
            {
                case State.Inactive:
                    Debug.Log("Image Tracking inactive");
                    break;
                case State.Active:
                    if (!targetsInitialized)
                    {
                        for (var i = 0; i < imageTargetInfos.Count; i++)
                        {
                            var target = MLImageTracker.AddTarget(
                                imageTargetInfos[i].Name,
                                imageTargetInfos[i].Image,
                                imageTargetInfos[i].LongerDimension,
                                OnTrackedObjectUpdate,
                                imageTargetInfos[i].isStationary);
                            imageTargets.Add(target);
                        }

                        targetsInitialized = true;
                    }

                    Debug.Log("Image Tracking active");
                    break;
                case State.PrivilegeDenied:
                    Debug.LogWarning("Image Tracking error: Privilege Denied");
                    break;
                case State.CameraUnavailable:
                    Debug.LogWarning("Image Tracking error: Can't activate camera");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnTrackedObjectUpdate(MLImageTracker.Target imageTarget, MLImageTracker.Target.Result imageTargetResult)
    {
        var trackingId = imageTarget.TargetSettings.Name;
        if (imageTargetResult.Status == MLImageTracker.Target.TrackingStatus.Tracked)
        {
            if (prefab)
            {
                if (!objects.ContainsKey(trackingId))
                {
                    var newObj = Instantiate(prefab);
                    newObj.name = $"Tracked Object: {trackingId}";
                    objects.Add(trackingId, newObj);
                }

                var trackedImageObject = objects[trackingId].GetComponent<TrackedImageObject>();

                trackedImageObject.imageTrackingSystem = this;
                trackedImageObject.trackingId = trackingId;
            }

            trackingState[trackingId] = imageTargetResult;
        }
    }

    public ImageTargetInfo GetTargetInfo(string id)
    {
        for (int i = 0; i < imageTargetInfos.Count; i++)
        {
            if (imageTargetInfos[i].Name == id)
                return imageTargetInfos[i];
        }

        return null;
    }

    public bool HasTrackingState(string trackingId)
    {
        return trackingState.ContainsKey(trackingId);
    }
    public MLImageTracker.Target.Result GetTrackingState(string trackingId)
    {
        if (!HasTrackingState(trackingId))
            throw new UnityException($"No Tracking state for tracking id '{trackingId}' found");
        
        return trackingState[trackingId];
    }
}