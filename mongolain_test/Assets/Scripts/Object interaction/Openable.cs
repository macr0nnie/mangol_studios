using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Openable : Interactable
{
    public Sprite open;
    public Sprite closed;

    private SpriteRenderer sr;
    private bool isOpen;

    public override void Interact()
    {
        if (isOpen)
        {

        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
