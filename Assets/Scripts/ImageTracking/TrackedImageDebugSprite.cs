using System;
using UnityEngine;

public class TrackedImageDebugSprite : MonoBehaviour
{
    public GameObject trackedImageObj;
    private Sprite sprite;

    void Update()
    {
        var trackedImage = trackedImageObj.GetComponent<TrackedImageObject>();

        var imageTargetInfo = trackedImage.imageTrackingSystem.GetTargetInfo(trackedImage.trackingId);
        var img = imageTargetInfo.Image;
        if (!sprite || sprite.texture != img)
        {
            sprite = Sprite.Create(img,new Rect(0,0,img.width,img.height),new Vector2(0.5f,0.5f),Math.Max(img.width, img.height));
            GetComponent<SpriteRenderer>().sprite = sprite;

        }

        var size = imageTargetInfo.LongerDimension;
        transform.localScale = new Vector3(size, size, 1);
    }
}