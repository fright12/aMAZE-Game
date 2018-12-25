using UnityEngine;
using System.Collections;

public class gameController : controller {
	public static bool playing = false;
	public static int coinsCollected;

	public GameObject paused;
	public GameObject resume;
	public GameObject loseMenu;

	public UnityEngine.UI.Text timer;
	public UnityEngine.UI.Text coins;
	public UnityEngine.EventSystems.EventSystem input;
	public UnityEngine.UI.Selectable pauseButton;

	private float t;

	void Start() {
		//allMazes.set (1);
		gameController.coinsCollected = 0;

		playing = true;

		if (settings.current.mode == "time") 
			t = (allMazes.current.use.GetUpperBound (0)+allMazes.current.use.GetUpperBound (1));
	}

	void Update(){
		if (settings.current.mode == "time") {
			if (Mathf.RoundToInt (t) == 0){
				playing = false;
				loseMenu.SetActive (true);
			}else if (playing){
				t -= Time.deltaTime;
				timer.text = "Time: "+t.ToString("0");
			}
		}

		if (!playing){
			if (!paused.activeInHierarchy&&pauseButton.enabled) input.SetSelectedGameObject(GameObject.Find ("Canvas/Level Complete Menu/Next Level"));

			pauseButton.enabled = false;

			//if (GameObject.Find ("Canvas/Level Complete Menu").activeInHierarchy) input.SetSelectedGameObject(GameObject.Find ("Canvas/Level Complete Menu/Next Level"));
			//print (GameObject.Find ("Canvas/LevelCompleteMenu").name);
		}

		if (Input.GetKeyDown(KeyCode.P)) pause ();

		coins.text = "Coins: " + coinsCollected;
	}

	public void pause(){
		playing = false;
		paused.SetActive(true);
		input.SetSelectedGameObject (resume);
	}

	public void play(){
		paused.SetActive (false);
		pauseButton.enabled = true;
		playing = true;
	}
}
