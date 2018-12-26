using UnityEngine;
using System.Collections;

public class playerMovement : movement {
	public string input = "";
	private float runPoint = 2f / 3f;

	public virtual void Awake(){
		setUp ();

        if (Application.isMobilePlatform)
            rotationSpeed = 2f;
	}
	
	public virtual void FixedUpdate(){
		float h = 0f, v = 1f; //h = horizontal movement (rotating), v = vertical movement (forward)
		
		if (Application.isMobilePlatform) { //Is it a mobile device?
            Touch[] touchArray = Input.touches; //All touches on the screen during this frame
			
			foreach (Touch touch in touchArray) { //Loop through all touches
				if (touch.position.x < Screen.width / 2f) //If touch is on left half, rotate left
					h += -1f;
				else //Otherwise, rotate right
					h += 1f;
			}

            //if (touchArray.Length == 2 && h == 0f)
            //	v = 0f; //If there are two touches, stop running

            v = Mathf.Clamp01(-Input.acceleration.z);
            if (Input.acceleration.y > 0)
                v += Input.acceleration.y;
            v = playerSpeed * Mathf.Clamp01(v - runPoint) / runPoint;
		} else {
            h = Input.GetAxis (input + "Horizontal"); //Get rotation input from left and right arrow keys
            v = Input.GetAxis(input + "Vertical"); //Get forward movement from up arrow key
            if (v > 0 && Input.GetKey(KeyCode.LeftControl))
                v = 0.25f;
        }
		
		if (v >= 0) movementManagement (h, v); //Move player
        h = 0f;
	}
	
	void movementManagement(float horizontal, float vertical){
		if (horizontal != 0f) Rotating(horizontal); //If it needs to rotate

        anim.SetFloat (hash.speedFloat, playerSpeed * vertical, speedDampTime, Time.deltaTime); //Set speed
	}
	
	public void Rotating(float rotate){
		rotation += rotate * rotationSpeed; //Change rotation variable
		
		Quaternion targetRotation = Quaternion.Euler (0f, rotation, 0f); //Set rotation to rotate to
		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime); //Smoothly change old rotation to new rotation
		
		GetComponent<Rigidbody>().MoveRotation (newRotation); //Finalize the rotation to correct value
	}
}