using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class settingsController : controller {
	public GameObject colorSwatch;
	public GameObject multiMode;
	public GameObject modeMenu;
	public GameObject wallSpacingSlider;

	public Selectable currentColor;
	public Text directions;
	public Text coins;

	private static GameObject menu;
	//rivate Selectable mode;

	void Start() {
		if (!menu) menu = GameObject.Find ("Canvas/Player Settings");
		directions.text = "How to Play \n" +
			"Move the character around the maze looking for the exit to get to the next level \n \n" +
			"Controls: " +
			"Modes \n Normal - Simply find the exit to continue to the next level \n" +
			"Time - Race against the clock to get to the exit. If time runs out you will lose and have to restart the entire maze until you can finish in time.\n" +
			"Computer - Race a player controlled by the computer to the end of the maze. If the computer finishes first, you lose. \n";
				
		if (Application.isMobilePlatform) {
			multiMode.SetActive (false);

			directions.text = directions.text.Substring (0, directions.text.IndexOf ("Controls")+10) + 
				"Use the joystick in the bottom right to control player movement \n \n" +
				directions.text.Substring (directions.text.IndexOf ("Modes"));

		}else{
			directions.text = directions.text.Substring (0, directions.text.IndexOf ("Controls")+10) + 
				"Use the arrow keys to control player movement \n \n" +
				directions.text.Substring (directions.text.IndexOf ("Modes")) +
				"Multiplayer - Race another human player on the same maze. Whoever gets to the exit first wins! Note: the other player uses “wasd” to move around.";
		}

		//directions.gameObject.SetActive (false);
		setAllFalse ();
		//menu.GetComponent<Button> ().onClick();

		currentColor.image.color = settings.color[0];
		//menu.GetComponent<Button>().interactable = false;

		wallSpacingSlider.GetComponent<Slider> ().value = settings.current.wallSpacing;
		changeWallSpacing (wallSpacingSlider);

		foreach (Button button in modeMenu.GetComponentsInChildren<Button>(true)) 
			if (button.name.ToLower() == settings.current.mode) button.interactable = false;
		//modeMenu.SetActive (false);
		//setMode (mode);

		coins.text = "Coins: " + settings.current.coins;

		GameObject.Find ("Canvas/Back").transform.position = new Vector2(60f * Screen.width / 450f, 30f * Screen.height / 281f);
		//GameObject.Find ("Canvas/Scrollbar").GetComponent<Scrollbar> ().value = .5f;
	}

	void LateUpdate(){
		if (colorSwatch.activeInHierarchy&&Input.GetKeyDown(KeyCode.Mouse0)){
			StartCoroutine(closeColorSwatch ());
		}
	}
	
	IEnumerator closeColorSwatch(){
		yield return new WaitForSeconds (0.1f);
		colorSwatch.SetActive(false);
	}

	public void buy(GameObject priceTag){
		//settings.current.coins = 800;

		if (settings.current.coins >= findPrice (priceTag)) {
			if (priceTag.transform.parent.gameObject.name == "Select Player") settings.current.bought.Add (controller.get.players[settings.current.player[0]].name);
			if (priceTag.transform.parent.gameObject.name == "Select Wall Texture") settings.current.bought.Add (controller.get.materials[settings.current.wall].name);
			if (priceTag.transform.parent.gameObject.name == "Select Ground Texture") settings.current.bought.Add (controller.get.materials[settings.current.ground].name);
			if (priceTag.transform.parent.gameObject.name == "Select Skybox") settings.current.bought.Add (controller.get.skyboxes[settings.current.skybox].name);

			settings.current.coins -= findPrice (priceTag);
			coins.text = "Coins: " + settings.current.coins;
			priceTag.SetActive (false);
		}
	}

	public static int findPrice(GameObject obj){
		if (obj.transform.parent.gameObject.name == "Select Player") return settings.store[0][settings.current.player[0]];
		if (obj.transform.parent.gameObject.name == "Select Wall Texture") return settings.store[1][settings.current.wall];
		if (obj.transform.parent.gameObject.name == "Select Ground Texture") return settings.store[1][settings.current.ground];
		if (obj.transform.parent.gameObject.name == "Select Skybox") return settings.store[2][settings.current.skybox];
		return 0;
	}

	public void changeColor(Selectable color){
		settings.color[0] = color.image.color;
		currentColor.image.color = color.image.color;
		colorSwatch.SetActive (false);
	}

	public void scroll(GameObject menus){
		RectTransform[] menu = menus.GetComponentsInChildren<RectTransform> ();
		//print (menus.name+", "+(-57.5f-GameObject.Find ("Canvas/Scrollbar").GetComponent<Scrollbar> ().value));
		//menus.transform.position = new Vector3(0f, -57.5f-GameObject.Find ("Canvas/Scrollbar").GetComponent<Scrollbar> ().value, 0f);

		//GameObject.Find ("Canvas/Scrollbar").GetComponent<Scrollbar> ().value = .9f;
		//for (int i = 0; i<menu.Length/2-2; i++)
			//print (menu [i + 1].transform.position);
			//print (menu [i * 2 + 4].name);
			//menu [i*2+4].transform.position = new Vector3(menu[i*2+4].transform.position.x, 204.4f-i*23.2f-GameObject.Find ("Canvas/Menus/Scrollbar").GetComponent<Scrollbar> ().value * 92.8f, 0f);
		//foreach (RectTransform up in menus.GetComponentsInChildren<RectTransform>())
			//up.transform.position += new Vector3(0f, 50f, 0f);
			//print (up.transform.position);
	}

	public void focusMenu(GameObject scrollbar){
		print (scrollbar.name);
		//scrollbar.GetComponentInChildren<Scrollbar> ().value += .1f;
	}

	public void changeWallSpacing(GameObject slider){
		settings.current.wallSpacing = slider.GetComponent<Slider> ().value;

		GameObject.Find ("SampleGame/Walls").GetComponentsInChildren<Transform> () [1].transform.position = new Vector3 (0f, -17.5f, 7f + settings.current.wallSpacing);
		GameObject.Find ("SampleGame/Walls").GetComponentsInChildren<Transform> () [2].transform.position = new Vector3 (-settings.current.wallSpacing, -17.5f, 0f);
		GameObject.Find ("SampleGame/Walls").GetComponentsInChildren<Transform> () [3].transform.position = new Vector3 (settings.current.wallSpacing, -17.5f, -3f-settings.current.wallSpacing);
	}

	public void setMode(GameObject mode){
		settings.current.mode = mode.name.ToLower ();

		foreach(Button button in mode.transform.parent.gameObject.GetComponentsInChildren<Button>(true))
			button.interactable = true;

		mode.GetComponent<Button>().interactable = false;
	}

	public void website(){
		Application.OpenURL ("http://productionsefd.wix.com/efd-productions");
	}

	public void deleteData(){
		controller.delete = true;
	}

	public void setAllFalse(){
		//foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Menu")) obj.GetComponent<Button>().interactable = true;
		
		foreach (Object obj in GameObject.FindGameObjectsWithTag("Deactivate")) ((GameObject)obj).SetActive (false);

		//GameObject.Find ("SampleGame/GameCamera").SetActive (false);
		//foreach (Transform trans in GameObject.Find("Cameras").GetComponentsInChildren<Transform>()) trans.gameObject.SetActive (false);
		//foreach (Transform trans in GameObject.Find("Canvas/Menus").GetComponentsInChildren<Transform>()) trans.gameObject.SetActive (false);
	}
}