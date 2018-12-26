using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class single : MonoBehaviour {
	protected gameController get {
		get { return GetComponent<gameController> (); }
	}
	
	public GameObject mainCamera;
	
	protected GameObject player1;
	protected GameObject player2;
	protected virtual System.Type movement2 {
		get { return null;}
	}

	protected string[] winMessage = new string[]{"Level Complete!", "Next Level"};
	protected string[] loseMessage = new string[]{"Game Over", "Try Again"};

	protected virtual void Start(){
        player1 = access.player1.addPlayer (typeof(playerMovement), game.currentMaze.upperLeft);
        
		mainCamera = addCamera (get.normalCamera, player1);
		
		if (movement2 != null) 
			player2 = access.player2.addPlayer (movement2, game.currentMaze.lowerRight);
	}

	protected GameObject addCamera(GameObject cam, GameObject parent){
		GameObject temp = MonoBehaviour.Instantiate (cam);
		
		temp.transform.SetParent (parent.transform);
		temp.transform.localRotation = Quaternion.Euler (temp.transform.rotation.eulerAngles.x, parent.transform.localRotation.y, temp.transform.rotation.eulerAngles.z);
		temp.transform.localPosition = new Vector3 (0f, 1.75f, -1f);

		return temp;
	}

	public virtual void endOfLevelMessage(GameObject winner){
		get.diamondFound (winMessage);
    }
}