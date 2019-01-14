using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{    
    override public Vector3 GetNextMovement()
    {        
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }
        return position;        
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        this.CancelInvoke();
        // If collided with an enemy allow continuing movement if not moving into the Enemy square
        if (collision.collider.CompareTag("Enemy"))
        {
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
            position = lastPosition;
        }
        
        Debug.Log("COLLIDED!!!!!!");
    }
}
