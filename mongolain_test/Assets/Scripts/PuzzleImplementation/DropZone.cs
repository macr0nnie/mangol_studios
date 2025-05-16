using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DropZone : MonoBehaviour
{
    public PuzzleIDs puzzleId;
    public UnityEvent<GameObject> onItemDropped;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var draggable = other.GetComponent<ItemDraggable>();
        if (draggable != null && draggable.puzzleId == this.puzzleId)
        {
            draggable.SetDroppedInValidZone(true);
            onItemDropped?.Invoke(draggable.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var draggable = other.GetComponent<ItemDraggable>();
        if (draggable != null && draggable.puzzleId == this.puzzleId)
        {
            draggable.SetDroppedInValidZone(false);
        }
    }
}