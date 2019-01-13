using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyMovement : CharacterMovement
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

        RaycastHit hitInfo;
        bool movementFound = false;

        Vector3 posMov = myPos;
        int num_hits = 0;


        if (dif.x > 0)
        {
            num_hits = CheckHits(Vector3.right);
            if (num_hits > 1)
            {
                for (int i = 1; i < num_hits; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.CompareTag("Player"))
                    {
                        MoveRight();
                        lastMoved = (int)lastMovedEnum.RIGHT;
                        movementFound = true;
                    }
                    
                }
            }
            else
            {
                MoveRight();
                lastMoved = (int)lastMovedEnum.RIGHT;
                movementFound = true;
            }

        }
        if (dif.x < 0 && !movementFound)
        {
            num_hits = CheckHits(Vector3.left);
            if (num_hits > 1)
            {
                for (int i = 1; i < num_hits; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.CompareTag("Player")) // Move if colliding with player
                    {
                        MoveLeft();
                        lastMoved = (int)lastMovedEnum.LEFT;
                        movementFound = true;
                    }                    
                }
            }
            else
            {
                MoveLeft();
                lastMoved = (int)lastMovedEnum.LEFT;
                movementFound = true;
            }
        }
        if (dif.y > 0 && !movementFound)
        {
            num_hits = CheckHits(Vector3.up);
            if (num_hits > 1)
            {
                for (int i = 1; i < num_hits; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.CompareTag("Player")) // Move if colliding with player
                    {
                        MoveUp();
                        lastMoved = (int)lastMovedEnum.UP;
                        movementFound = true;
                    }
                }
            }
            else
            {
                MoveUp();
                lastMoved = (int)lastMovedEnum.UP;
                movementFound = true;
            }

        }
        else if (dif.y < 0 && !movementFound)
        {
            num_hits = CheckHits(Vector3.down);
            if (num_hits > 1)
            {
                for (int i = 1; i < num_hits; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.CompareTag("Player")) // Move if colliding with player
                    {
                        MoveDown();
                        lastMoved = (int)lastMovedEnum.DOWN;
                        movementFound = true;
                    }
                }
            }
            else
            {
                MoveDown();
                lastMoved = (int)lastMovedEnum.DOWN;
                movementFound = true;
            }
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
