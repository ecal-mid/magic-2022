using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class RoomTransform : MonoBehaviour
{
	bool delayActivation => !Application.isEditor;
	void Awake()
	{
		if(delayActivation)
		{
			gameObject.SetActive(false);

			MLPersistentCoordinateFrames.OnLocalized += HandleOnLocalized;
		}
	}

	void OnDestroy()
	{
		if (delayActivation)
		{
			MLPersistentCoordinateFrames.OnLocalized -= HandleOnLocalized;
		}
	}

	void HandleOnLocalized(bool localized)
	{
		if(localized)
		{
			Update();
			gameObject.SetActive(true);
		}
	}


	void Update()
	{
		var roomTransform = RoomTracking.instance.roomTransform;
		transform.position = roomTransform.position;
		transform.rotation = roomTransform.rotation;
	}
}