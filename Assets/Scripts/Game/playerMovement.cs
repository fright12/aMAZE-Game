using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;

	private Animator anim;
	private hashIDs hash;
	private float rotation; //Direction player is pointing
	private string input = "";

	private float minSwipeDist=0.4f, maxSwipeTime=0.3f; 
	private bool couldBeSwipe;
	private Vector2 startPos;
	private float swipeStartTime;

	void Awake(){
		anim = GetComponent<Animator> ();
		hash = GameObject.Find ("GameController").GetComponent<hashIDs> ();

		rotation = gameObject.transform.rotation.y*180f; //Set rotation to 180 in y axis

		//If multi mode, read from different axis for primary (up, left, right), and secondary (a, d, w) keys
		if (settings.current.mode == "multi"){
			if (gameObject.name == controller.get.players[settings.current.player[0]].name+"(Clone)") input = "Player1";
			if (gameObject.name == controller.get.players[settings.current.player[1]].name+"(Clone)") input = "Player2";
		}

		//StartCoroutine(checkHorizontalSwipes ());
	}

	//When the player collides with another object
	void OnTriggerEnter(Collider other){
		if (other.tag == "Coin"){ //If the object is a coin
			Destroy (other.gameObject); //Get rid of the coin
			gameController.coinsCollected++; //Add 1 to the number of coins collected this level
		}
	}

	void FixedUpdate(){
		if (gameController.playing){ //If not paused
			float h = 0f, v = 1f; //h = horizontal movement (rotating), v = vertical movement (forward)

			//movementManagement(0f, 1f);
			//return;

			if (Application.isMobilePlatform) { //Is it a mobile device?
				Touch[] touchArray = Input.touches; //All touches on the screen during this frame

				foreach(Touch touch in touchArray) { //Loop through all touches
					if (touch.position.x < Screen.width/2f) h += -1f; //If touch is on left half, rotate left
					if (touch.position.x > Screen.width/2f) h += 1f; //If touch is on right half, rotate right
				}

				//anim.SetFloat (hash.speedFloat, 3.5f, speedDampTime, Time.deltaTime);
				//movementManagement(h, v); //Move player  
				if (touchArray.Length == 2 && h == 0f) v = 0f; //If there are two touches, stop running
			} else {
				h = -Input.GetAxis (input+"Horizontal"); //Get rotation input from left and right arrow keys
				v = Input.GetAxis (input+"Vertical"); //Get forward movement from up arrow key
				//movementManagement(h, v); //Move player
			}

			if (v >= 0) movementManagement(h, v); //Move player
		}else{ //If game is paused
			anim.SetFloat (hash.speedFloat, 0); //Set speed to 0
		}
	}

	public float[] getInput(){
		return new float[]{-Input.GetAxis (input+"Horizontal"), Input.GetAxis (input+"Vertical")};
	}

	void movementManagement(float horizontal, float vertical){
		if (horizontal != 0f) Rotating(horizontal); //If it needs to rotate

		if (vertical == 0f) //If not moving forward
			anim.SetFloat (hash.speedFloat, 0); //Set speed to 0
		else //Otherwise, if moving forward
			anim.SetFloat (hash.speedFloat, settings.current.playerSpeed, speedDampTime, Time.deltaTime); //Set speed to max speed
	}

	void Rotating(float rotate){
		rotation += rotate * settings.current.rotationSpeed; //Change rotation variable

		Quaternion targetRotation = Quaternion.Euler (0f, rotation, 0f); //Set rotation to rotate to
		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing*Time.deltaTime); //Smoothly change old rotation to new rotation

		GetComponent<Rigidbody>().MoveRotation (newRotation); //Finalize the rotation to correct value
	}

	IEnumerator checkHorizontalSwipes () { //Coroutine, which gets Started in "Start()" and runs over the whole game to check for swipes
		while (true) { //Loop. Otherwise we wouldnt check continoulsy ;-)
			foreach (Touch touch in Input.touches) { //For every touch in the Input.touches - array...
				switch (touch.phase) {
					case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
							couldBeSwipe = true;

						   startPos = touch.position;  //Position where the touch started
						   swipeStartTime = Time.time; //The time it started
						break;

					case TouchPhase.Stationary: //Is the touch stationary? --> No swipe then!
						couldBeSwipe = false;
						break;
				}

				float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
				float swipeDist = Mathf.Abs (touch.position.x - startPos.x); //Swipedistance

				if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist) { // It's a swiiiiiiiiiiiipe!
					couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.

					/*if (Mathf.Sign (touch.position.x - startPos.x) == 1f) { //Swipe-direction, either 1 or -1.
						//Rotating (Mathf.Sign (touch.position.x - startPos.x));
						transform.Rotate (Vector3.right * Time.deltaTime);//Right-swipe
					} else {
						//Rotating (Mathf.Sign (touch.position.x - startPos.x));
						transform.Rotate (Vector3.left * Time.deltaTime);//Left-swipe
					}*/

					rotation += Mathf.Sign (touch.position.x - startPos.x)*90;
					gameObject.transform.rotation = Quaternion.Euler (0f, rotation, 0f);
					//Rotating (Mathf.Sign (touch.position.x - startPos.x));
				} 
			}
			//transform.Translate(Vector3.forward * Time.deltaTime);
			//for (int i = 0; i<10; i++)
			//movementManagement (80f, 0f);
			//transform.Rotate (Vector3.left * Time.deltaTime);//Left-swipe

			yield return null;
		}
	}
}