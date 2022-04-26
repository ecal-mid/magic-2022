using UnityEngine;

public class RaycastAction_ToggleKinematic : MonoBehaviour, IRaycastAction
{
	public void DoAction(RaycastActionData actionData)
	{
		var rb = GetComponent<Rigidbody>();
		rb.isKinematic = !rb.isKinematic;
	}
}