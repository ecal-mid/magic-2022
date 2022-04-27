using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;

public class DragAndDrop : MonoBehaviour
{
	public InputActionProperty dragAction;
	
	Draggable draggedObject;
	Vector3 draggedObjectRelativePosition;
	Quaternion draggedObjectRelativeRotation;

	List<Collider> triggerObjects = new List<Collider>();

	void OnTriggerEnter(Collider other)
	{
		triggerObjects.Add(other);

		var draggable = GetDraggable(other);
		if (draggable)
		{
			draggable.possibleHandles.Add(this);
		}
	}

	void OnTriggerExit(Collider other)
	{
		triggerObjects.Remove(other);

		var draggable = GetDraggable(other);
		if (draggable)
		{
			draggable.possibleHandles.Remove(this);
		}
	}
 
	void Update()
	{
		var isPressed = dragAction.action.IsPressed();
		if (!draggedObject)
		{
			if (isPressed)
			{
				Draggable foundObject = null;
				for (int i = 0; i < triggerObjects.Count; i++)
				{
					foundObject = GetDraggable(triggerObjects[i]);
					if(foundObject != null)
						break;
				}

				if (foundObject)
				{
					draggedObject = foundObject;
					draggedObject.draggingHandle = this;
					Vector3 relativeDirection = draggedObject.transform.position - transform.position;
					draggedObjectRelativePosition = transform.InverseTransformDirection(relativeDirection);
					draggedObjectRelativeRotation = Quaternion.Inverse(transform.rotation) * draggedObject.transform.rotation;
				}
			}
		}
		else
		{
			if (!isPressed)
			{
				draggedObject.draggingHandle = null;
				draggedObject = null;
			}
		}

		if (draggedObject)
		{
			draggedObject.transform.position = transform.position + transform.TransformDirection(draggedObjectRelativePosition);
			draggedObject.transform.rotation = transform.rotation * draggedObjectRelativeRotation;
		}
	}

	static Draggable GetDraggable(Collider collider)
	{
		if (collider.TryGetComponent<Draggable>(out var draggable))
			return draggable;

		return collider.GetComponentInParent<Draggable>();
	}
}