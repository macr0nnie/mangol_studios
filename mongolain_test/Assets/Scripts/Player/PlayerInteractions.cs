using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRadius = 1.5f; // How close the player needs to be
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask interactableLayer;

    private Collider[] _nearbyInteractables = new Collider[5]; // Buffer for nearby objects
    private IInteractable _closestInteractable;
    private int _interactableCount;

    private void Update()
    {
        // Check for nearby interactables every frame
        FindClosestInteractable();

        if (Input.GetKeyDown(interactKey) && _closestInteractable != null)
        {
            _closestInteractable.Interact();
        }
    }

    private void FindClosestInteractable()
    {
        _interactableCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            interactionRadius,
            _nearbyInteractables,
            interactableLayer
        );

        _closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < _interactableCount; i++)
        {
            float distance = Vector3.Distance(transform.position, _nearbyInteractables[i].transform.position);
            if (distance < closestDistance)
            {
                if (_nearbyInteractables[i].TryGetComponent(out IInteractable interactable))
                {
                    _closestInteractable = interactable;
                    closestDistance = distance;
                }
            }
        }
    }

    // Visualize interaction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}


