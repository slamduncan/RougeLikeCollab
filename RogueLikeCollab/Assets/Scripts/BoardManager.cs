using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//There are two random classes and we've specified using the Unity Engine one
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    //The length and width of the "dungeon"
    public int columns = 8; //X
    public int rows = 8; //Y

    //GameObjects that hold the sprite prefabs for their respective tiles. We drag and drop the prefabs in unity to specify which ones are to be used
    public GameObject wall;
    public GameObject floor;
    public GameObject playerChar;

    //BoardHolder is a "container" used to keep the Heirarchy view for the Scene clean and contained. It will hold all of the walls and floors as "child" GameObjects
    private Transform boardHolder;

    //gridPositions is a master list that specifies all of the tile locations in the center of the "dungeon" for random walls to spawn. From (1,1) to (7,7).
    //This leaves a 1x1 path between outer wall and the randomly generated walls so no paths are blocked.
    private List<Vector3> gridPositions = new List<Vector3>();

    //We initialize the list with nested for loops, specifying the coordinates of each 
    private void InitializeList()
    {
        //Clear the List in case we are reinitializing for another dungeon map
        gridPositions.Clear();

        //Init the list from coordinates (1,1) to (columns-1, rows-1)
        for(int x = 1; x < columns - 1; x++)
        {
            for(int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //BoardSetup is where we fill the board with actual tiles. In this method, the outer walls are created and the middle of the map is populated with floor tiles
    private void BoardSetup()
    {
        //Initialize the boardHolder object to keep the Heirarchy view clean
        boardHolder = new GameObject("Board").transform;
        
        //Fill the Dungeon
        for(int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                //Default instantiation is a floor tile
                GameObject toInstantiate = floor;

                //If the for loop is on a coordinate that is included in the outer wall, use a wall tile
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = wall;
                }

                //Instantiate the selected coordinate with the determined tile. Use the x and y coordinate, 0f for z axis, and Quaternion.identity for no rotation. 
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                //Add the game object as a child of boardHolder
                instance.transform.SetParent(boardHolder);
            }
        }

        //Place the character

        GameObject player = Instantiate(playerChar, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
    }

    //RandomPosition determines a random coordinate to select out of the gridPositions List and will be used to place a "random" wall tile
    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    //Takes an argument of the tile prefab you would like to place on the board and randomly places it.
    private void LayoutObjectAtRandom(GameObject tile)
    {
        int objectCount = Random.Range(5, 9);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }
    }

    //This method is called by the GameManager and creates the board by setting up the empty dongeon and then filling it with random locations of wall tiles
    public void SetupScene()
    {
        InitializeList();
        BoardSetup();
        LayoutObjectAtRandom(wall);
    }
}
