using UnityEngine;
using System.Collections;

public class end : MonoBehaviour {
	public GameObject nextLevel;
	public GameObject loseMenu;

	void Update(){
		transform.Rotate (new Vector3 (0, 0, 30) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		gameController.playing = false;

		gameObject.SetActive (false);

		if (settings.current.mode == "computer" && other.name.Substring (0, other.name.IndexOf ("(Clone)")).Equals (controller.get.players [settings.current.player [1]].name)) {
			loseMenu.SetActive (true);
		}else{
			nextLevel.SetActive (true);
			allMazes.current.level = ++allMazes.current.level%5;
			allMazes.createMaze ();
		}
		//allMazes.current.level = 1;
		//print (allMazes.current.level);

		//if (allMazes.current.level == 0) Application.LoadLevel (0);

		settings.current.coins += gameController.coinsCollected;
	}
}
