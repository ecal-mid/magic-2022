using UnityEngine;

public class RoomTransform : MonoBehaviour
{
	void Update()
	{
		var roomTransform = RoomTracking.instance.roomTransform;
		transform.position = roomTransform.position;
		transform.rotation = roomTransform.rotation;
	}
}