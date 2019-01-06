using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 2f;
    public bool isPlayerCharacter = false;
    private Vector3 position;
    private Vector3 lastPosition;
    private Transform tr;
    private Rigidbody2D rb2d;
    private GameManager gm;
    private GameObject player;


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
            if (this.tag.Contains("Player"))
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
            }
            // AI Movement
            else if (this.tag.Contains("Enemy"))
            {
                Vector3 playPos = player.transform.position;
                Vector3 myPos = transform.position;
                Vector3 dif = playPos - myPos;
                if (dif.x > 0)
                    MoveRight();
                else if (dif.x < 0)
                    MoveLeft();
                else if (dif.y > 0)
                    MoveUp();
                else if (dif.y < 0)
                    MoveDown();
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
    }

    private void MoveRight()
    {
        position += Vector3.right;
        Debug.Log("Right");
    }
    private void MoveLeft()
    {
        position += Vector3.left;
        Debug.Log("Left");
    }
    private void MoveDown()
    {
        position += Vector3.down;
        Debug.Log("Down");
    }
    private void MoveUp()
    {
        position += Vector3.up;
        Debug.Log("Up");
    }
}
