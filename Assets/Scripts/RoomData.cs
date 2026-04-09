using System;
using Unity.VisualScripting;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public bool isOverlapping = false;
    public bool hasVisited = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            isOverlapping = true;
        }
        
        if (other.CompareTag("Player") && !hasVisited)
        {
            hasVisited = true;
            SetWallsColor(Color.chartreuse);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            isOverlapping = true;
        }
    }
    
    private void SetWallsColor(Color color)
    {
        Transform wallsParent = transform.GetChild(1);

        foreach (Transform wall in wallsParent)
        {
            SpriteRenderer sr = wall.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = color;
            }
        }
    }
}
