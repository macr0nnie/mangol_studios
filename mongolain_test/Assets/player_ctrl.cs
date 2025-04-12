using UnityEngine;
using UnityEngine.EventSystems;

public class player_ctrl : MonoBehaviour 
{
    public float move_speed;
  //  public Rigidbody2D rb;
    
    private Vector2 moveDirection;
    void Start()
    {
       // rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        processInput(); 

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction(); 
        }
    }
    public void CheckInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f);
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    //use for all the physics speed/movement calculations 
    private void FixedUpdate()
    {
        move();
    }
    // method for player input
    void processInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY); // stores movement input as a vector
    }

    //used to update player movement based on calculations 
    void move()
    {
       // rb.linearVelocity = new Vector2(moveDirection.x * move_speed, moveDirection.y * move_speed); 
    }


}
