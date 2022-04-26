using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class RaycastActionTrigger : MonoBehaviour
{
	public Raycaster raycaster;
	public InputActionProperty mainAction;

	void OnEnable()
	{
		mainAction.action.Enable();
	}

	void Update()
	{
		if (raycaster.isHit && mainAction.action.WasPerformedThisFrame())
		{
			var collider = raycaster.hit.collider;
			var actionData = new RaycastActionData
			{
				position=raycaster.hit.point,
				direction= raycaster.ray.direction
			};
			
			IRaycastAction action;
			if(!collider.TryGetComponent(out action))
			{
				action = collider.GetComponentInParent<IRaycastAction>();
			}

			if (action != null)
				action.DoAction(actionData);
		}
	}
}