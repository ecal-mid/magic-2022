using UnityEngine;

public class DragFeedback : MonoBehaviour
{
	public Material draggingMaterial;
	public Material inRangeMaterial;
	Material originalMaterial;

	void Start()
	{
		originalMaterial = GetComponent<Renderer>().sharedMaterial;
	}

	void Update()
	{
		var draggable = GetComponent<Draggable>();
		
		var mat = originalMaterial;
		if (draggable.draggingHandle)
			mat = draggingMaterial;
		else if (draggable.possibleHandles.Count > 0)
			mat = inRangeMaterial;

		GetComponent<Renderer>().sharedMaterial = mat;

	}
}