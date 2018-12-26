using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class controller : MonoBehaviour {
    public static RectTransform canvas {
		get { return GameObject.Find ("Canvas").GetComponent<RectTransform> (); }
	}
    public static bool delete = false; //Should the data be deleted?

	void Start(){
		if (settings.showTutorial)
            Application.LoadLevelAdditive (3);

        maze m = new maze(16, 9);
        m.setUp();
        m.moveToCell(Instantiate(access.player2.prefab), m.upperLeft).AddComponent<computerMovement>();

    }

    public void play(){
		Destroy (GameObject.Find ("EventSystem"));
		Application.LoadLevelAdditive (1);
	}

	public void loadLevel(int level){ //Load a new scene
		Application.LoadLevel (level);

		if (settings.showTutorial && level != 0)
            Application.LoadLevelAdditive (3);
	}

	public void exit() { //Quit the game
		Application.Quit ();
	}

	public static void setAndScale(GameObject obj, Material mat, Vector2 size){ //Set and scale a new texture
		obj.GetComponent<Renderer>().material = mat; //Set materials
		obj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", size); //Resize main texture
		obj.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", size); //Resize bump texture
	}
}
