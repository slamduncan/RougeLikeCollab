using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 position;
    private Vector3 lastPosition;
    private Transform tr;
    private Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>(); 
        position = tr.position;
        lastPosition = tr.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (tr.position == position)
        {
            lastPosition = position;
            if (Input.GetKeyDown(KeyCode.D))
            {
                position += Vector3.right;
                Debug.Log("Right");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                position += Vector3.left;
                Debug.Log("Left");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                position += Vector3.up;
                Debug.Log("Up");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                position += Vector3.down;
                Debug.Log("Down");
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
        
    }

    void OnCollisionEnter(Collision collision) // Built into Unity, looks for this function if defined, calls on collision
    {
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        Debug.Log("Collided with something");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.CancelInvoke();
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        Debug.Log("COLLIDED!!!!!!");
        Debug.Log(transform.position);
        Debug.Log(lastPosition);
    }
}
