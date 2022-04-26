using UnityEngine;

public class RaycasterTargetVisualization : MonoBehaviour
{
	public Transform upTransform;
	public GameObject visualizationObj;
	public float maxDistance = 5;
	void Update()
	{
		var raycaster = GetComponent<Raycaster>();
		var active = false;
		if (raycaster.isHit)
		{
			visualizationObj.transform.position = raycaster.hit.point;
			visualizationObj.transform.rotation = Quaternion.LookRotation(raycaster.hit.normal,upTransform ? upTransform.up : Vector3.up);
			active = true;
		}
		else
		{
			visualizationObj.transform.position = raycaster.ray.origin + raycaster.ray.direction.normalized * maxDistance;
			visualizationObj.transform.rotation = Quaternion.LookRotation(raycaster.ray.direction);
		}

		if (visualizationObj.activeSelf != active)
			visualizationObj.SetActive(active);
	}
}