using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class movement : MonoBehaviour {
    public static maze inMaze;

	public Animator anim;
	public hashIDs hash;

	public float playerSpeed = 4.5f;
	public float rotationSpeed = 4f;

	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	public float rotation;

	public void setUp(){
		anim = GetComponent<Animator> ();
        hash = gameObject.AddComponent<hashIDs> ();

		pointInStartingDirection ();
	}

	public void pointInStartingDirection(){
		List<int> moves = new List<int> ();
		moves = getMoveDirections (transform.position);
		setRotation (moves[Random.Range (0, moves.Count)]);
	}

	public List<int> getMoveDirections(Vector3 position){ //Find the directions a character could move in
		cell current = inMaze.getCell (position);
		
		List<int> moves = new List<int> ();
		for (int i = 0; i < 360; i += 90) { //Check all directions
			cell temp = current.cellInDirection (i);
			
			if (inMaze.isValid (temp) && inMaze.getWall (current, temp) == null) //If the cell being checked is on the grid and there's no wall between it and the current cell, it could be moved to
				moves.Add (i);
		}
		
		return moves;
	}

	public void setRotation(int r){
		rotation = r;
		transform.rotation = Quaternion.Euler (0f, r, 0f);
	}

	void OnDisable(){
		try{
			anim.SetFloat (hash.speedFloat, 0f);
		} 
		catch{}
	}

	void movementManagement(float horizontal, float vertical){		
		if (vertical == 0f) //If not moving forward
			anim.SetFloat (hash.speedFloat, 0); //Set speed to 0
		else //Otherwise
			anim.SetFloat (hash.speedFloat, playerSpeed, speedDampTime, Time.deltaTime); //Set speed to max speed
	}

	void OnTriggerEnter(Collider other){
        if (other.tag == "Coin" && GetComponent<playerMovement>() != null) { //If the object is a coin
			Destroy (other.gameObject); //Get rid of the coin
			GameObject.Find("GameController").GetComponent<gameController>().addCoin(); //Add 1 to the number of coins collected this level
		} else if (other.name == "Diamond(Clone)") {
			Destroy (GameObject.Find("GameController").GetComponent<gameController>().diamond);
			GameObject.Find ("GameController").GetComponent<single>().endOfLevelMessage (gameObject);
		}
	}
}