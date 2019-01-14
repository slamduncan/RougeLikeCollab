using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemyMovement : CharacterMovement
{
    private bool hitWall = false;
    private enum moveEnum
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }

    private List<PosMove> openList = new List<PosMove>();
    private List<PosMove> closedList = new List<PosMove>();

    private class PosMove : IComparable<PosMove>
    {
        public int Cost { get; set; }        
        public Vector3 Position { get; set; }
        public PosMove LastMove { get; set; }
        public Vector3 Destination { get; set; }

        public PosMove(int cost,  Vector3 position, Vector3 destination,  PosMove parent = null)
        {
            this.Cost = cost;
            this.Position = position;            
            LastMove = parent;
            this.Destination = destination;
        }
        
        public int GetValue()
        {
            Vector3 diff = this.Destination - this.Position;
            return Cost + Mathf.RoundToInt(Mathf.Abs(diff.x) + (int)Mathf.Abs(diff.y));            
        }


        public int CompareTo(PosMove obj)
        {
            try
            {
                return this.GetValue() - obj.GetValue();
            }
            catch
            {
                return 0;
            }
        }
    }

    override public Vector3 GetNextMovement()
    {
        Vector3 playPos = player.transform.position;
        Vector3 myPos = transform.position;
        Vector3 dif = playPos - myPos;
        openList.Clear();
        closedList.Clear();
        int num_hits = 0;

        // Shortcut for ensuring last movement goes through
        if (position != transform.position)
        {
            return position;
        }
        
        // Add each direction from the start square
        Vector3[] moveList = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        PosMove start = new PosMove(0, myPos, playPos);
        foreach (Vector3 m in moveList)
        {
            num_hits = CheckHits(m);
            if (num_hits > 1)
            {
                for (int i = 1; i < num_hits; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.collider.CompareTag("Player"))
                    {
                        openList.Add(new PosMove(1, myPos + m, playPos, start)); // Add if it's the player
                    }
                }
            }
            else
                openList.Add(new PosMove(1, myPos + m, playPos, start));
        }

        // Pathfinding alg
        while (openList.Count > 0)
        {
            openList.Sort(); // Sort list so first object is best (lowest) value            
            PosMove eval = (PosMove)openList[0];
            closedList.Add(eval);
            openList.RemoveAt(0);
            Vector3 playerDiff = eval.Position - playPos;
            if (playerDiff.x > -1.0f && playerDiff.x < 1.0f && playerDiff.y < 1.0f && playerDiff.y > -1.0f) // Found player
            {
                PosMove m1 = eval, m2 = start;
                // Track path back
                while (m1.LastMove != null)
                {
                    m2 = m1;
                    m1 = m1.LastMove;
                }
                position = m2.Position;
                return m2.Position;
            }
            // Check for walls
            bool skip = false;        
            foreach (Vector3 m in moveList)
            {
                
                PosMove nextMove = new PosMove(eval.Cost + 1, eval.Position + m, playPos, eval);
                num_hits = CheckHits(m);
                if (num_hits > 1)
                {
                    for (int i = 1; i < num_hits; i++)
                    {
                        RaycastHit2D hit = hits[i];
                        if (!hit.collider.CompareTag("Player"))
                        {
                            skip = true;
                        }
                    }
                }
                // Make sure we're playing inside the game
                if (nextMove.Position.x > gm.boardScript.columns || nextMove.Position.x < gm.boardScript.columns ||
                    nextMove.Position.y > gm.boardScript.rows || nextMove.Position.y < gm.boardScript.rows)
                    skip = true;
                if (skip)
                {
                    skip = false;
                    continue;                    
                }                
                // Check if we already have this square in our list                
                foreach(PosMove move in closedList)
                {
                    if (move.Position == nextMove.Position)
                    {
                        skip = true;
                    }
                }
                if (skip)
                {
                    skip = false;
                    continue;
                }
                
                // Add to evaluation list if not there already
                for (int i = 0; i < openList.Count; i++)
                {
                    PosMove move = (PosMove)openList[i];
                    if (move.Position == nextMove.Position)
                    {
                        // check if current value is better 
                        if (eval.GetValue() > nextMove.GetValue())
                        {
                            openList[i] = nextMove;
                        }
                        skip = true;
                    }
                        
                }                
                if (!skip)
                    openList.Add(nextMove);   
                    
            }
        }
        return myPos;
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
