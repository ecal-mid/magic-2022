using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class TrackedImageObjectTransform : MonoBehaviour
{
    private bool wasInitialized;


    void Update()
    {
        var trackedImageObject = GetComponent<TrackedImageObject>();
        var trackingState = trackedImageObject.GetTrackingStateOrDefault();
        // If tracked, update position / rotation and move following object to that position / rotation
        switch (trackingState.Status)
        {
            case MLImageTracker.Target.TrackingStatus.Tracked:

                var lerp = wasInitialized ? Time.deltaTime * 1f : 1;
                transform.position = Vector3.Lerp(transform.position, trackingState.Position, lerp);
                transform.rotation = Quaternion.Slerp(transform.rotation, trackingState.Rotation, lerp);

                wasInitialized = true;
                break;

            case MLImageTracker.Target.TrackingStatus.NotTracked:
                wasInitialized = false;
                // Additional Logic can be added here for when the image is not detected
                break;
        }
    }
}