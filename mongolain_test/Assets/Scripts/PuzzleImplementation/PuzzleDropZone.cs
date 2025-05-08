using UnityEngine;
using System.Collections.Generic;

public class PuzzleDropZone : MonoBehaviour
{
    [Header("Puzzle Drop Zone Settings")]
    [SerializeField] private float dropZoneRadius = 1.5f; // Radius of the drop zone
    [SerializeField] private LayerMask draggableLayer; // Layer mask to filter draggable items

    private List<DraggableItem> currentDraggables = new List<DraggableItem>(); // List of currently dropped items

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a draggable item and within the drop zone radius
        if (other.CompareTag("Draggable") && IsWithinDropZone(other.transform.position))
        {
            DraggableItem draggableItem = other.GetComponent<DraggableItem>();
            if (draggableItem != null && !currentDraggables.Contains(draggableItem))
            {
                currentDraggables.Add(draggableItem);
                OnItemDropped(draggableItem);
            }
        }
    }