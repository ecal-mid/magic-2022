using UnityEngine;

public interface IRaycastAction
{
	void DoAction(RaycastActionData actionData);
}

public struct RaycastActionData
{
	public Vector3 position;
	public Vector3 direction;
}