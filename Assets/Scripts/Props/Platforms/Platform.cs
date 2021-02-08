using UnityEngine;

public enum PlatformMovement
{
   horizontal, vertical
}

public class Platform : MonoBehaviour
{

    public PlatformMovement movementType;
    public float platformSpeed = 2.5f;
    public float maxDistanceMovement = 3.0f;

    private Vector2 pointA;
    private Vector2 pointB;
    private bool movingLeft;
    private bool movingUp;


    void Start()
    {
        switch (movementType)
        {
            case PlatformMovement.horizontal:
                pointA = new Vector2(transform.position.x + maxDistanceMovement, transform.position.y);
                pointB = new Vector2(transform.position.x - maxDistanceMovement, transform.position.y);
                break;
            case PlatformMovement.vertical:
                pointA = new Vector2(transform.position.x, transform.position.y + maxDistanceMovement);
                pointB = new Vector2(transform.position.x, transform.position.y - maxDistanceMovement);
                break;
            
        }


    }


    void FixedUpdate()
    {
        switch (movementType)
        {
            case PlatformMovement.horizontal:
                HorizontalMovement();
                break;
            case PlatformMovement.vertical:
                VerticalMovement();
                break;
        }

    }

    private void HorizontalMovement()
    {
        if (movingLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB, platformSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA, platformSpeed * Time.deltaTime);
        }
        if (transform.position.x >= pointA.x)
        {
            movingLeft = true;
        }
        else if (transform.position.x <= pointB.x)
        {
            movingLeft = false;
        }
    }

    private void VerticalMovement()
    {
        if (movingUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB, platformSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA, platformSpeed * Time.deltaTime);
        }
        if (transform.position.y >= pointA.y)
        {
            movingUp = true;
        }
        else if (transform.position.y <= pointB.y)
        {
            movingUp = false;
        }

    }
}
