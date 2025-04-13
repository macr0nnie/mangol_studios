using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    [SerializeField] private float pushForce = 5f;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        // Push the crate in the player's facing direction
        Vector3 pushDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        _rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
    }
}