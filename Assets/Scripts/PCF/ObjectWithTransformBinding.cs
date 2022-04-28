using System;
using System.Collections.Generic;
using MagicLeap.Core;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ObjectWithTransformBinding : MonoBehaviour
{
	public string id;
	TransformBinding transformBinding;

	bool wasLocalized;
	bool isDragging;

	void Start()
	{
		MLResult result = MLPersistentCoordinateFrames.Start();
		if (!result.IsOk)
		{
			Debug.LogErrorFormat("Error: PCFExample failed starting MLPersistentCoordinateFrames, disabling script. Reason: {0}", result);
			enabled = false;
			return;
		}

		MLPersistentCoordinateFrames.OnLocalized += HandleOnLocalized;
	}

	void OnDestroy()
	{
		MLPersistentCoordinateFrames.OnLocalized -= HandleOnLocalized;
		MLPersistentCoordinateFrames.Stop();
	}


	void Update()
	{
		var newIsDragging = GetComponent<Draggable>().draggingHandle != null;
		if (isDragging && !newIsDragging)
		{
			transformBinding.Update();
			print("updated transform binding after drag");
		}

		isDragging = newIsDragging;
	}

	private void HandleOnLocalized(bool localized)
	{
		if (!localized || wasLocalized)
			return;

		print("localized (initial)");
		TransformBinding.storage.LoadFromFile();

		bool found = false;
		foreach (var storedBinding in TransformBinding.storage.Bindings)
		{
			if (storedBinding.Id != id)
				continue;

			// Try to find the PCF with the stored CFUID.
			MLResult result = MLPersistentCoordinateFrames.FindPCFByCFUID(storedBinding.PCF.CFUID, out MLPersistentCoordinateFrames.PCF storedPcf);

			if (storedPcf != null && MLResult.IsOK(storedPcf.CurrentResultCode)) // found
			{
				transformBinding = storedBinding;
				transformBinding.Bind(storedPcf, transform, true);
				print($"found stored transform binding for {id}");
				found = true;
				break;
			}
		}

		if (!found)
		{
			MLPersistentCoordinateFrames.FindClosestPCF(transform.position, out MLPersistentCoordinateFrames.PCF closestPcf);

			transformBinding = new TransformBinding(id, "manual");
			transformBinding.Bind(closestPcf, transform);
			transformBinding.Update();
			print($"create new transform binding for {id}");
		}

		wasLocalized = true;
	}
}