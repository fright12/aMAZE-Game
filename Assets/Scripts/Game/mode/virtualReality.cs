using UnityEngine;
using System.Collections;

public class virtualReality : single {
	protected override void Start (){
		mainCamera = MonoBehaviour.Instantiate (get.vrCamera);

        //get.VRLevelOver = Instantiate(get.VRLevelOver) as GameObject;
        //get.VRLevelOver.SetActive(false);
        //get.VRLevelOver.transform.SetParent(temp.GetComponentInChildren<CardboardHead>().transform);

        mainCamera.AddComponent<userMovement> ();
        mainCamera.transform.position = game.currentMaze.virtualToPhysical (game.currentMaze.upperLeft, 1.75f);
	}
}