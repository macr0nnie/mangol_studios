using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent onBeginDrag;
    public UnityEvent onDrag;
    public UnityEvent onEndDrag;

    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position; // Store the original position
        onBeginDrag?.Invoke(); // Invoke the event if there are any listeners
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Update the position of the item to follow the mouse
        onDrag?.Invoke(); // Invoke the event if there are any listeners
    }
}