using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingBlade : MonoBehaviour
{
    public float bladeSpeed = 2.5f;
    public float maxDistanceMovement = 3.0f;

    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 bladeAttack;
    private bool movingLeft;

	void Start()
    {
        pointA = new Vector2(transform.position.x + maxDistanceMovement, transform.position.y);
        pointB = new Vector2(transform.position.x - maxDistanceMovement, transform.position.y);

    }

    void Update()
    {
        BladeMovement();
    }

    private void BladeMovement()
    {
        if (movingLeft)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB, bladeSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA, bladeSpeed * Time.deltaTime);
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

   
}
