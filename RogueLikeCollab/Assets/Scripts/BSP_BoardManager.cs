using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP_BoardManager : MonoBehaviour
{
    //Modifiable Variables 
    public int boardRows = 30, boardColumns = 30; //X,Y Board Size
    public int minRoomSize, maxRoomSize;//The minimum and maximum a dungeon room can be
    public GameObject floorTile;        //FloorTile Sprite Prefab Gameobject
    public GameObject corridorTile;     //CorridorTile Sprite Prefab Gameobject
    public GameObject wallTile;         //WallTile Sprite Prefab Gameobject

    private GameObject[,] boardPositions;  //X,Y array representing the board and containing the locations of the floor tiles

    
    /********************************************
     * Class SubDungeon implements a Binary Space Partitioned (BSP) Random Dungeon Layout. 
     * BSP utilizes a binary tree data structure and recursion to split the game board into non-uniformly sized cells. 
     * These cells contain a randomly generated room which are then all linked together with corridors.
     * 
     ********************************************/
    public class SubDungeon
    {
        public SubDungeon left, right;                  //The child branches for this node of the binary tree
        public Rect rect;                               //
        public Rect room = new Rect(-1, -1, 0, 0);      // i.e null. This is the room contained within this node of the binary tree
        public int debugId;                             //
        public List<Rect> corridors = new List<Rect>(); //

        private static int debugCounter = 0;            //

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
        }

        //Function: If we have reached the end of the branch and our left and right 
        //children are null, we are a leaf and return True. Otherwize, return False. 
        //Input: none
        //Output: the bool stating if we are a leaf or not
        public bool IAmLeaf()
        {
            return left == null && right == null;
        }

        //Function: Split the current branch node into two new child nodes/branches
        //Input: The minimum and maximum size we can split a branch by so the new nodes are not too small or too large
        //Output: A bool stating if the branch has been split or not
        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IAmLeaf())
            {
                return false;
            }

            // choose a vertical or horizontal split depending on the proportions
            // i.e. if too wide split vertically, or too long horizontally,
            // or if nearly square choose vertical or horizontal at random
            bool splitH;
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }
            else if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            }
            else
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }

            //if the branch is too small to split, return false and call it a leaf.
            if (rect.height / 2 < minRoomSize && rect.width / 2 < minRoomSize)
            {
                Debug.Log("Sub-dungeon " + debugId + " will be a leaf");
                return false;
            }

            if (splitH)
            {
                // split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally)
                //the split will be randomly selected at a value that is buffered on both sides by minRoomSize
                int split = Random.Range(minRoomSize, (int)(rect.width - minRoomSize)); 

                //Create new children that have the size of each side of the split
                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(
                  new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else
            {
                int split = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(
                  new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }

        public void CreateRoom()
        {
            if (left != null)
            {
                left.CreateRoom();
            }
            if (right != null)
            {
                right.CreateRoom();
            }
            if (left != null && right != null)
            {
                CreateCorridorBetween(left, right);
            }
            if (IAmLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 2);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 2);

                // room position will be absolute in the board, not relative to the sub-dungeon
                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
                Debug.Log("Created room " + room + " in sub-dungeon " + debugId + " " + rect);
            }
        }

        public void CreateCorridorBetween(SubDungeon left, SubDungeon right)
        {
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();

            Debug.Log("Creating corridor(s) between " + left.debugId + "(" + lroom + ") and " + right.debugId + " (" + rroom + ")");

            // attach the corridor to a random point in each room
            Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

            // always be sure that left point is on the left to simplify the code
            if (lpoint.x > rpoint.x)
            {
                Vector2 temp = lpoint;
                lpoint = rpoint;
                rpoint = temp;
            }

            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);

            Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

            // if the points are not aligned horizontally
            if (w != 0)
            {
                // choose at random to go horizontal then vertical or the opposite
                if (Random.Range(0, 1) > 2)
                {
                    // add a corridor to the right
                    corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                    // if left point is below right point go up
                    // otherwise go down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));
                    }
                }
                else
                {
                    // go up or down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                    }

                    // then go right
                    corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1));
                }
            }
            else
            {
                // if the points are aligned horizontally
                // go up or down depending on the positions
                if (h < 0)
                {
                    corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));
                }
            }

            Debug.Log("Corridors: ");
            foreach (Rect corridor in corridors)
            {
                Debug.Log("corridor: " + corridor);
            }
        }

        //Function: Get the room of the current branch or of it's children?
        //Input: none
        //Output: the Room found by the function
        public Rect GetRoom()
        {
            //If we are a leaf (last node of the branch), return the room inside of the leaf.
            if (IAmLeaf())
            {
                return room;
            }
            //If we are a branch and the left child node is not null, try to return the room inside of it
            if (left != null)
            {
                //grab the left room using its GetRoom() function
                Rect lroom = left.GetRoom();
                //If the room has a define dimension and is not "null", return it.
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            //If we are a branch and the right child node is not null, try to return the room inside of it
            if (right != null)
            {
                //grab the right room using its GetRoom() function
                Rect rroom = right.GetRoom();
                //If the room has a define dimension and is not "null", return it.
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }
            //If no room was found, return "null"
            // workaround non nullable structs
            return new Rect(-1, -1, 0, 0);
        }
    }

    public void CreateBSP(SubDungeon subDungeon)
    {
        Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
        if (subDungeon.IAmLeaf())
        {
            // if the sub-dungeon is too large
            if (subDungeon.rect.width > maxRoomSize
              && subDungeon.rect.height > maxRoomSize
              /* || Random.Range(0.0f, 1.0f) > 0.25 */)
            {

                if (subDungeon.Split(minRoomSize, maxRoomSize))
                {
                    Debug.Log("Splitted sub-dungeon " + subDungeon.debugId + " in "
                      + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
                      + subDungeon.right.debugId + ": " + subDungeon.right.rect);

                    CreateBSP(subDungeon.left);
                    CreateBSP(subDungeon.right);
                }
            }
        }
    }
    public void DrawWalls()
    {
        for(int i = 0; i < boardRows; i++)
        {
            for(int j = 0; j < boardColumns; j++)
            {
                if(boardPositions[i,j] == null)
                {
                    GameObject instance = Instantiate(wallTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    boardPositions[i, j] = instance;
                }
            }
        }
    }
    public void DrawRooms(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }
        if (subDungeon.IAmLeaf())
        {
            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    GameObject instance = Instantiate(floorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(transform);
                    boardPositions[i, j] = instance;
                }
            }
        }
        else
        {
            DrawRooms(subDungeon.left);
            DrawRooms(subDungeon.right);
        }
    }

    void DrawCorridors(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        DrawCorridors(subDungeon.left);
        DrawCorridors(subDungeon.right);

        foreach (Rect corridor in subDungeon.corridors)
        {
            for (int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for (int j = (int)corridor.y; j < corridor.yMax; j++)
                {
                    if (boardPositions[i, j] == null)
                    {
                        GameObject instance = Instantiate(corridorTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(transform);
                        boardPositions[i, j] = instance;
                    }
                }
            }
        }
    }

    void Start()
    {
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        boardPositions = new GameObject[boardRows, boardColumns];
        DrawCorridors(rootSubDungeon);
        DrawRooms(rootSubDungeon);
        DrawWalls();


    }
}

