using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {
	public static controller get; //Used to access the arrays of options
	public static bool delete = false; //Should the data be deleted?

	public GameObject[] players; //Collection of all players
	public Material[] materials; //Collection of all wall/ground textures
	public Material[] skyboxes; //Collectionf of all skies

	void Start() {
		if (settings.current == null) { //If there are currently no settings loaded, load them from file
			settings.current = (settings)saveLoad.Load (new settings(), "/userOptions.opt"); //Load settings options

			if (settings.current.colorValues != null) //As long as there was a color chosen, load colors with the values in colorValues
			settings.color = new Color[]{new Color(settings.current.colorValues[0,0],settings.current.colorValues[0,1],settings.current.colorValues[0,2],settings.current.colorValues[0,3]),
				 new Color(settings.current.colorValues[1,0],settings.current.colorValues[1,1],settings.current.colorValues[1,2],settings.current.colorValues[1,3])};

			allMazes.current = (allMazes)saveLoad.Load (new allMazes(), "/game.gm"); //Load in the last used maze
			if (allMazes.current.use == null) allMazes.createMaze (); //If there was no maze, make a new one
		}

		//if (Application.isMobilePlatform) settings.current.playerSpeed *= 3 / 4f;

		get = GetComponent<controller> (); //Assign get to the current instance of controller
	}

	void OnApplicationQuit(){ //When the game is exited
		//Save RGBW values of color into color values so they can be saved
		settings.current.colorValues = new float[,]{{settings.color[0].r,settings.color[0].g,settings.color[0].b,settings.color[0].a},
			{settings.color[1].r,settings.color[1].g,settings.color[1].b,settings.color[1].a}};

		checkSettings (); //Check and make sure all of the selected options have been bought
		saveLoad.Save (settings.current, "/userOptions.opt"); //Save current settings options
		saveLoad.Save (allMazes.current, "/game.gm"); //Save current maze

		if (delete){ //If the user has clicked the erase data button
			System.IO.File.Delete (saveLoad.directory + "/useroptions.opt"); //Delete file with settings options
			System.IO.File.Delete (saveLoad.directory + "/game.gm"); //Delete file with maze data

			if (!System.IO.Directory.Exists("D:/")){ //If the game is not being run from a disc
				string path = saveLoad.directory;  //Get path name from saveLoad script
				System.IO.Directory.Delete (path); //Delete the first folder the files were in

				//Get rid of the last file name in the directory path
				int psn = 0;
				while (path.IndexOf ("/", psn+1) != -1)
					if (path.IndexOf ("/", psn+1) != -1)
						psn = path.IndexOf ("/", psn+1);

				System.IO.Directory.Delete (path.Substring (0,psn)); //Delete the second folder the files were in
			}
		}
	}
	
	public void checkSettings(){ //Check to make sure the user has bought everything they have selected
		settings temp = (settings)saveLoad.Load (new settings(), "/userOptions.opt"); //Load old settings options in a temporary variable

		//Check players, walls, grounds, and skies to make sure they have been bought; if not, restore the current one to what it was at last save
		if (!settings.current.bought.Contains (get.players[settings.current.player[0]].name))
			settings.current.player[0] = temp.player[0];
		if (!settings.current.bought.Contains (get.materials[settings.current.wall].name))
			settings.current.wall = temp.wall;
		if (!settings.current.bought.Contains (get.materials[settings.current.ground].name))
			settings.current.ground = temp.ground;
		if (!settings.current.bought.Contains (get.skyboxes[settings.current.skybox].name))
			settings.current.skybox = temp.skybox;
	}

	public void loadLevel(int level){ //Load a new scene
		Application.LoadLevel (level);
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
