using System.Collections;
using UnityEditor.VisionOS;
using UnityEngine;

public class Moving_Walls : MonoBehaviour
{
    public GameObject Room; // Array of wall GameObjects
    public Transform centerPoint; // Center point for rotation
    public float rotationSpeed = 50f; // Speed of rotation

    //function to make the walls rotate 
    public void WallRotation()
    {
        //the room walls should rotate once
    
    }
    public void ShiftingHeights(){
        //shifting heights effect for the walls. 
    }

    public IEnumerator RotateWall()
    {
        // Rotate the wall around the center point
        float rotationAngle = 0f;
        
        while (rotationAngle < 90f) // Rotate for 90 degrees
        {
            float rotation = rotationSpeed * Time.deltaTime;
            Room.transform.RotateAround(centerPoint.position, Vector3.up, rotationAngle);
            rotationAngle += rotationAngle;
            yield return null; // Wait for the next frame
        }
       
    }
    public void Hide_Entrance(GameObject entry_point)
    {
        // Hide the entrance wall
        entry_point.SetActive(false);
    }
    public void Show_Entrance(GameObject entry_point)
    {
        // Show the entrance wall
        entry_point.SetActive(true);
    }
    void Update()
    {
        //call a courtine wall roation
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RotateWall());
        }
        else
        {
            StopCoroutine(RotateWall());
        }
    }

}
