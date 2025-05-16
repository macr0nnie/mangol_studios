using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; 

[RequireComponent(typeof(Collider2D))]
public class ItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
     public PuzzleIDs puzzleId;
    [SerializeField] private bool isDraggable = true;
    [SerializeField] private Camera dragCamera;
    [SerializeField] private float dragDepth = 0f;
    [SerializeField] private bool returnToStartIfDroppedOutsideZone = true;

    [SerializeField] private bool scaleOnDrag = true;
    [SerializeField] private float dragScale = 1.1f;
    [SerializeField] private bool tintOnDrag = false;
    [SerializeField] private Color dragTint = new Color(1f, 1f, 1f, 0.8f);

    // Events with context
    [System.Serializable] public class DragEvent : UnityEvent<ItemDraggable, Vector3> { } //changed the name to match
    
    [Header("Events")]
    public DragEvent onDragStart;
    public DragEvent onDragEnd;
    public UnityEvent onReset;

    // State
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isDragging = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool droppedInValidZone = false;

    private void Awake()
    {
        // Store original state
        originalPosition = transform.position;
        originalScale = transform.localScale;

        // Get references
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Default to main camera if none specified
        if (dragCamera == null)
        {
            dragCamera = Camera.main;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        isDragging = true;
        droppedInValidZone = false;

        // Apply visual feedback
        if (scaleOnDrag)
        {
            transform.localScale = originalScale * dragScale;
        }

        if (tintOnDrag && spriteRenderer != null)
        {
            spriteRenderer.color = dragTint;
        }

        // Fire event
        onDragStart?.Invoke(this, transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable || !isDragging) return;

        // Convert screen position to world position
        Vector3 worldPos = dragCamera.ScreenToWorldPoint(new Vector3(
            eventData.position.x,
            eventData.position.y,
            dragCamera.WorldToScreenPoint(transform.position).z + dragDepth
        ));

        // Update position
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable || !isDragging) return;

        isDragging = false;

        // Reset visual feedback
        if (scaleOnDrag)
        {   
            
            transform.localScale = originalScale;
        }

        if (tintOnDrag && spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Fire event
        onDragEnd?.Invoke(this, transform.position);

        // Return to start position if not dropped in a valid zone
        if (returnToStartIfDroppedOutsideZone && !droppedInValidZone)
        {
            ResetPosition();
        }
    }
    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.localScale = originalScale;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        isDragging = false;
        droppedInValidZone = false;
        
        onReset?.Invoke();
    }
    public void SetDroppedInValidZone(bool isValid)
    {
        droppedInValidZone = isValid;
    }
    public void SetDraggable(bool canDrag)
    {
        isDraggable = canDrag;
    }
    public void UpdateOriginalPosition()
    {
        originalPosition = transform.position;
    }
    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }
    public bool IsDragging()
    {
        return isDragging;
    }
}