using UnityEngine;
using System.Collections;

[System.Serializable]
public class player {
	public GameObject location {
		get { return GameObject.Find ("GameController").GetComponent<gameController> ().location;}
	}

	public GameObject prefab;
	public Color color;

	public player (GameObject prfb, Color clr){
        prefab = prfb;
		color = clr;
	}

	public GameObject addPlayer (System.Type movementType, cell position) {
		GameObject temp = game.currentMaze.moveToCell (GameObject.Instantiate (prefab), position);
        temp.AddComponent (movementType);

		try {
			GameObject tempLoc = MonoBehaviour.Instantiate (location) as GameObject;
			tempLoc.transform.SetParent (temp.transform);
			tempLoc.transform.localPosition = new Vector3 (0f, -5f, 0f);
			tempLoc.GetComponentInChildren<pulse> ().color = color;
		} 
		catch {}

		return temp;
	}
}