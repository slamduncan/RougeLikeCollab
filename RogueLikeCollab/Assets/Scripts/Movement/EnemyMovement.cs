using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    private bool hitWall = false;
    private enum lastMovedEnum
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    private int lastMoved = (int)lastMovedEnum.RIGHT;
    override public Vector3 GetNextMovement()
    {
        // AI Movement        
        Vector3 playPos = player.transform.position;
        Vector3 myPos = transform.position;
        Vector3 dif = playPos - myPos;
        if (!hitWall)
        {
            if (dif.x > 0)
            {
                MoveRight();
                lastMoved = (int)lastMovedEnum.RIGHT;
            }
            else if (dif.x < 0)
            {
                MoveLeft();
                lastMoved = (int)lastMovedEnum.LEFT;
            }
            else if (dif.y > 0)
            {
                MoveUp();
                lastMoved = (int)lastMovedEnum.UP;
            }
            else if (dif.y < 0)
            {
                MoveDown();
                lastMoved = (int)lastMovedEnum.DOWN;
            }
            hitWall = false;
        }
        else if (lastMoved == (int)lastMovedEnum.RIGHT || lastMoved == (int)lastMovedEnum.LEFT)
        {
            if (dif.y > 0)
            {
                MoveUp();
                lastMoved = (int)lastMovedEnum.UP;
            }
            else if (dif.y < 0)
            {
                MoveDown();
                lastMoved = (int)lastMovedEnum.DOWN;
            }
            else
            {
                MoveLeft();
                lastMoved = (int)lastMovedEnum.LEFT;
            }
            
            hitWall = false;
        }
        else
        {
            MoveRight();            
            lastMoved = (int)lastMovedEnum.RIGHT;
        }
        
        return position;
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        this.CancelInvoke();
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        if (collision.gameObject.CompareTag("Wall"))
        {
            hitWall = true;
        }                    
    }
}
