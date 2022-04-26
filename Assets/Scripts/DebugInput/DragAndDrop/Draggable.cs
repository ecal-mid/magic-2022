using System;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	public DragAndDrop draggingHandle { get; set; }

	public List<DragAndDrop> possibleHandles { get; } = new List<DragAndDrop>();
}