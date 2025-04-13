using UnityEngine;

public class Pickup : MonoBehaviour,IInteractable
{
   [SerializeField] private string itemName = "Health Potion";
    [SerializeField] private AudioClip pickupSound;

    public void Interact()
    {
        Debug.Log($"Picked up: {itemName}");
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        
        Destroy(gameObject); // Remove the item after pickup
    }
}
