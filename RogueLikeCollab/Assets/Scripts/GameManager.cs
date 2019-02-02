using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public AbstractBoardManager boardScript;

	//When the Game Starts
	void Awake () {

        //If the instance of GameManager has not been created yet
        if (instance == null)
            instance = this;
        //If an instance of GameManager already exists destroy the newly created game object
        else if (instance != this)
            Destroy(gameObject);
        
        //When the gameManager loads in, it will not destroy itself
        DontDestroyOnLoad(gameObject);

        //Create an Object of boardManager so we may setup and run it
        //boardScript = GetComponent<AbstractBoardManager>();
        
        //initialize the game
        InitGame();
	}

    void InitGame()
    {
        //Call the SetupScene method of the boardManager class
        boardScript.Start(); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
