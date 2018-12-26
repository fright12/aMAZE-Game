using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tutorial : controller {
	public Canvas myCanvas;
	public Text main;
	public RectTransform mapBorder;
	public RectTransform pauseBorder;
	public GameObject background;

	private Camera mapCamera;

	private static int stage = 0;

	private float lineTime = 0.1f; //Pause between showing lines
	private float letterTime = 0.05f; //Pause between showing letters

	void Start (){
		actions ();
	}

	void Update(){
		if ((stage == 8 && GameObject.FindObjectOfType<gameController> () == null) ||
		    (stage == 13 && dragButton.openMenu && !Input.GetMouseButton (0) && dragButton.openMenu.transform.position.x >= dragButton.openMenu.bounds.xMax - 1) ||
		    (stage == 15 && dragButton.openMenu == null && GameObject.Find ("Canvas/Player Settings Button").transform.position.x < GameObject.Find ("Canvas/Player Settings Button").GetComponent<dragButton>().bounds.xMin + 1)){
			stage++;
			actions ();
		}
	}

	public void actions(){
		if (stage == 0) { //Welcome
			StartCoroutine (changeText (new string[]{
				"Welcome to aMAZE Game",
				"\n\nTo get started, press the play button|",
			}));
		} else if (stage == 1) { //Turn on play button
			GameObject.Find ("Canvas/Play").transform.SetParent (myCanvas.transform);
			GameObject.Find (myCanvas.name + "/Play").transform.SetSiblingIndex (1);

            //vars.maze = new physicalMaze(new virtualMaze(1, 1));//controller.createMaze ();
            //maze.current = new maze();

			stage++;
			this.enabled = false;
		} else if (stage == 2) { //Set up game scene
			//gameController.playing = false;
			mapCamera = GameObject.Find ("Map Camera").GetComponent<Camera> ();

			foreach (MaskableGraphic g in GameObject.Find ("Canvas/Pause Button").GetComponentsInChildren<MaskableGraphic>()) {
				float avg = (g.color.r + g.color.g + g.color.b) / 3f;
				g.color = new Color (avg, avg, avg, g.color.a);
			}

			mapCamera.gameObject.SetActive (false);
			mapCamera.cullingMask = -1;

			string[] nav = new string[4];
			if (Application.isMobilePlatform){
				nav = new string[]{
					"Your player will run continuously",
					"\n\nPress the left half of the screen to turn left, and the right half to turn right",
					"\n\nPress both halves at the same time to stop running",
					""
				};
			} else {
				nav = new string[]{"Use the arrow keys to navigate", "\a", "\a", "\n\n"};
			}

			StartCoroutine (changeText (new string[]{
				"You are stuck in a maze",
				"\n\nTo get out, you will need to find the diamond",
				"",
				"|This is a bird's eye view of the maze",
				"\n\nExcept it will look like this|",
				"",
				"\n\n|This is you",
				"\n\n|And this is where you need to be",
				"",
				nav[0], nav[1], nav[2], nav[3],
				"|Collect coins to buy things later in the shop",
				"\n\nSee if you can find the diamond!",
				"|",""
			}));
		} else if (stage == 3) { //Point to map
			mapCamera.gameObject.SetActive (true);
			mapBorder.gameObject.SetActive (true);
		} else if (stage == 4) { //Change to real map
			mapBorder.gameObject.SetActive (false);
			mapCamera.GetComponent<Camera> ().cullingMask = 4096;
		} else if (stage == 5) { //Point to your position
		} else if (stage == 6) { //Point to diamond's position
		} else if (stage == 7) { //Turn on coins
			GameObject.Find ("Canvas/Coins").transform.SetParent (myCanvas.transform);
			GameObject.Find (myCanvas.name + "/Coins").transform.SetSiblingIndex (1);
		} else if (stage == 8) { //Let the user find the diamond
			background.SetActive (false);
			//gameController.playing = true;
		} else if (stage == 9) { //User has found diamond
			GameObject.Find ("Tutorial/Coins").transform.SetAsFirstSibling ();
			background.SetActive (true);

			string pressP = "";
			if (!Application.isMobilePlatform) pressP = " or pressing 'P'";

			StartCoroutine (changeText (new string[]{
				"Good job!",
				"\n\nYou found the diamond!",
				"",
				"From here, you can go on to the next level or return to the main menu",
				"\n\nA similar menu can be accessed during the game by clicking |the pause button" + pressP,
				"\n\nFirst, let's take a look at the settings menu",
				"|"
			}));
		} else if (stage == 10) { //Point to pause button
			pauseBorder.gameObject.SetActive (true);
			//border.rect.Set(-353f, 203f, 70f, 70f);
			//border.rect.Set (12f, 12f, 70f, 70f);
			//border.rect.Set (0f, 10f, 70f, 70f);
		} else if (stage == 11) { //Go to settings menu
			loadLevel (2);
			stage++;
		} else if (stage == 12){ //Explain settings menu
			StartCoroutine (changeText (new string[]{
				"This is the settings menu",
				"\n\nHere you can change certain aspects of the game such as the player, the environment, and the game mode",
				"",
				"To open a menu in settings, slide the button to the right",
				"\n\nTry it!",
				"|"
			}));
		} else if (stage == 13) { //Turn on player settings button
			main.text = "";
			background.transform.SetParent (GameObject.Find ("Canvas").transform);
			background.transform.SetSiblingIndex (GameObject.Find ("Canvas/Player Settings Button").transform.GetSiblingIndex ());
		} else if (stage == 14) { //User has dragged open player settings
			background.transform.SetAsLastSibling();

			StartCoroutine (changeText (new string[]{
				"Good job!",
				"\n\nWhen you're done making the changes you want, you will have to slide the button back to the stack before opening another menu",
				"\n\nDo this now to exit the tutorial and return to the main menu|"
			}));
		} else if (stage == 15){ //Turn off player settings so button can't be dragged back
			background.transform.SetSiblingIndex (GameObject.Find ("Canvas/Player Settings Button").transform.GetSiblingIndex ());
		} else if (stage == 16){ //End tutorial and go to main menu
            settings.showTutorial = false;
			loadLevel (0);

			stage = 0;
		}
	}
	
	IEnumerator changeText(string[] allText){
		for (int i = 0; i < allText.Length; i++) {
			if (allText[i] == "") {
				main.text = "";
				continue;
			}

			for (int j = 0; j < allText[i].Length; j++) {
				if (allText[i].Substring (j, 1) == "|"){
					allText[i] = allText[i].Substring (0, j) + allText[i].Substring (j + 1);
					j--;

					stage++;
					actions();
				}else{
					main.text += allText[i].Substring (j, 1);
					yield return new WaitForSeconds (letterTime);
				}
			}

			yield return new WaitForSeconds(allText[i].Length * lineTime - allText[i].Length * letterTime);
		}
	}

	/*private static int[,] tutorialMaze = new int[,]{
		{1,1,1,1,1,1,1,1,1,1,1,1,1},
		{1,2,0,0,0,0,0,0,0,0,1,0,1},
		{1,0,1,1,1,1,1,0,1,0,1,0,1},
		{1,0,0,0,1,0,0,4,1,0,1,0,1},
		{1,1,1,0,1,0,1,1,1,0,1,0,1},
		{1,3,1,0,1,0,1,0,0,0,1,0,1},
		{1,0,1,0,1,0,1,0,1,0,1,0,1},
		{1,0,1,0,1,0,1,0,1,0,0,0,1},
		{1,0,1,1,1,0,1,0,1,0,1,0,1},
		{1,0,0,0,0,0,1,0,1,0,1,0,1},
		{1,0,1,1,1,1,1,0,1,1,1,0,1},
		{1,0,0,0,0,0,0,0,1,0,0,0,1},
		{1,1,1,1,1,1,1,1,1,1,1,1,1},
	};*/
}