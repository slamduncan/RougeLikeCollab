using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 2f;
    public bool isPlayerCharacter = false;
    protected Vector3 position;
    protected Vector3 lastPosition;
    private Transform tr;
    private Rigidbody2D rb2d;
    protected GameManager gm;
    protected GameObject player;
    private bool changeDir;
    protected ContactFilter2D filter = new ContactFilter2D();
    protected RaycastHit2D[] hits = new RaycastHit2D[20];


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>(); 
        position = tr.position;
        lastPosition = tr.position;
        gm = GameManager.instance;
        player = gm.boardScript.playerChar;        
    }

    // Update is called once per frame
    void Update()
    {
        if (tr.position == position)
        {
            lastPosition = position;
            // Player Movement
            GetNextMovement();
        }

        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * speed);
        
    }

    virtual public Vector3 GetNextMovement()
    {
        return position;
    }

    void OnCollisionEnter(Collision collision) // Built into Unity, looks for this function if defined, calls on collision
    {
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        Debug.Log("Collided with something");
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        this.CancelInvoke();
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, Time.deltaTime * speed);
        position = lastPosition;
        Debug.Log("COLLIDED!!!!!!");
    }

    protected void MoveRight()
    {
        position += Vector3.right;
        Debug.Log("Right");
    }
    protected void MoveLeft()
    {
        position += Vector3.left;
        Debug.Log("Left");
    }
    protected void MoveDown()
    {
        position += Vector3.down;
        Debug.Log("Down");
    }
    protected void MoveUp()
    {
        position += Vector3.up;
        Debug.Log("Up");
    }

    protected int CheckHits(Vector2 direction)
    {
        return Physics2D.Raycast(this.transform.position, direction, filter, hits, 1.0f);
    }

    protected int CheckHits(Vector2 direction, Vector2 start)
    {
        return Physics2D.Raycast(start, direction, filter, hits, 1.0f);
    }
}
