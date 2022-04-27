using UnityEngine;

public class RoomTracking : MonoBehaviour
{
	public static RoomTracking instance;
	public Transform roomTransform;

	void Awake()
	{
		instance = this;
	}
}