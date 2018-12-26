using UnityEngine;
using System.Collections;

public class userMovement : playerMovement {
    private bool released = false;

	public override void Awake(){
		pointInStartingDirection ();

		Rigidbody force = gameObject.AddComponent<Rigidbody> ();
		
		force.useGravity = false;
		force.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		
		BoxCollider bc = gameObject.AddComponent<BoxCollider> ();
        bc.size = new Vector3(1f, 2f, 1f);
	}

	public override void FixedUpdate (){
		Touch[] touchArray = Input.touches; //All touches on the screen during this frame

        if (touchArray.Length == 0)
            released = true;

        if (released && (touchArray.Length > 0 || (!Application.isMobilePlatform && Input.GetKey (KeyCode.Space)))) {
            transform.Translate(new Vector3(0f, 0f, 0.1f), GetComponentInChildren <CardboardHead>().gameObject.transform);
			transform.position = new Vector3(transform.position.x, 1.75f, transform.position.z);
		}
	}
}