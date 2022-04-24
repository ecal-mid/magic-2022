using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class TrackedImageObject : MonoBehaviour
{
    public ImageTrackingSystem imageTrackingSystem;
    public string trackingId;

    public MLImageTracker.Target.Result GetTrackingStateOrDefault()
    {
        if (imageTrackingSystem.HasTrackingState(trackingId))
            return imageTrackingSystem.GetTrackingState(trackingId);
        else
            return new MLImageTracker.Target.Result
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Status = MLImageTracker.Target.TrackingStatus.NotTracked
            };
    }
}