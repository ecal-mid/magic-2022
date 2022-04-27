using UnityEngine;

public class SetActiveStateOnStart : MonoBehaviour
{
	public bool active;
	public GameObject target;
	void Start()
	{
		target.SetActive(active);
	}
}