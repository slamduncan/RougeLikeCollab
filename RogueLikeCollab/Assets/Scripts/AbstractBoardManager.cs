using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoardManager : MonoBehaviour
{
    //GameObjects that hold the sprite prefabs for their respective tiles. We drag and drop the prefabs in unity to specify which ones are to be used    
    public GameObject playerChar;      // Player
    public GameObject enemy;           // Easy
    public GameObject enemy2;          // Med
    public GameObject enemy3;          // Hard
    public GameObject floorTile;        //FloorTile Sprite Prefab Gameobject
    public GameObject corridorTile;     //CorridorTile Sprite Prefab Gameobject
    public GameObject wallTile;         //WallTile Sprite Prefab Gameobject
    public int boardRows;
    public int boardColumns; //X,Y Board Size

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
