using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

	//This class is attached to the camera of the unity game. It is ran once to start/load the gameManager which then starts/loads everything else
	void Awake () {
		
        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
	}
}
