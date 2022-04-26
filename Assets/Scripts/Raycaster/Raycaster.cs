using UnityEngine;

[DefaultExecutionOrder(-10)]
public class Raycaster : MonoBehaviour
{
	public float maxDist = float.PositiveInfinity;
	public LayerMask layerMask = ~0;
	public Ray ray { get; private set; }

	public RaycastHit hit { get; private set; } = default;
	public bool isHit { get; private set; }

	void Update()
	{
		ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out RaycastHit newHit, maxDist, layerMask))
		{
			isHit = true;
			hit = newHit;
		}
		else
		{
			isHit = false;
			hit = default;
		}
	}
}