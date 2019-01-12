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
}
