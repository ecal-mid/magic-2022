using UnityEngine;

public class LineBetweenPoints : MonoBehaviour
{
	public GameObject object1;
	public GameObject object2;

	void LateUpdate()
	{
		var line = GetComponent<LineRenderer>();
		line.useWorldSpace = true;
		line.SetPosition(0, object1.transform.position);
		line.SetPosition(1, object2.transform.position);
	}
}