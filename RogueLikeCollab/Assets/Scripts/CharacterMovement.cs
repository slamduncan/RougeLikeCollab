using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 position;
    private Vector3 lastPosition;
    private Transform tr;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        position = tr.position;
        lastPosition = tr.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = position;
        if (Input.GetKeyDown(KeyCode.D) && tr.position == position)
        {
            position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && tr.position == position)
        {
            position += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.W) && tr.position == position)
        {
            position += Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && tr.position == position)
        {
            position += Vector3.down;
        }

        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision collision) // Built into Unity, looks for this function if defined, calls on collision
    {
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        Debug.Log("Collided with something");
    }
}
