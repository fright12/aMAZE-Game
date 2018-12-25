using UnityEngine;
using System.Collections;

public class cycle : MonoBehaviour {
	public Vector3 targetPosition; //Position to be moved to
	private float smooth = 5f; //Speed the player is moved to the new position

	void Awake(){
		targetPosition = transform.position; //Set target position
	}

	void Update () {
		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime*smooth); //Smoothly move the player from the old position to the new one
	}
}