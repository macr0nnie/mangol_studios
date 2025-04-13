using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    
    private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    private void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    public void Interact()
    {
        _isOpen = !_isOpen;
        Debug.Log("Door " + (_isOpen ? "opened" : "closed"));
        
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            _isOpen ? _openRotation : _closedRotation,
            openSpeed * Time.deltaTime
        );
    }
}
