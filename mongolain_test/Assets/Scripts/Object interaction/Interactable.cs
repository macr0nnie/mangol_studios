using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<CharacterController>().OpenInteractableIcon();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<CharacterController>().CloseInteractableIcon();
    }
    
}
