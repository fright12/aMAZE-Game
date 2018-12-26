using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameController : MonoBehaviour {
	public Camera mapCamera;
	public GameObject normalCamera;
	public GameObject vrCamera;

	public GameObject diamond;
	public GameObject coin;
	public GameObject location;
	public GameObject levelOver;
        
	[HideInInspector]
	public Text[] menu;

	public Text timer;
    public Text coins;
	public GameObject winText;

    private int coinsCollected = 0;
    private bool startChecking = false;

    void Start(){
        menu = levelOver.GetComponentsInChildren<Text> (true);

        game.currentMaze.setUp();

        gameObject.AddComponent(settings.mode);
        diamond = game.currentMaze.moveToCell(GameObject.Instantiate(diamond), new cell((int)(game.currentMaze.bounds.x / 2f), (int)(game.currentMaze.bounds.y / 2f)));

        for (int i = 0; i < game.currentMaze.grid.Count; i++) {
            if (game.currentMaze.grid[i].isEmpty && Random.Range(0, 3) < 2 && settings.mode != typeof(multiplayer)) {
                game.currentMaze.moveToCell(Instantiate(coin), game.currentMaze.grid[i]);
            }
        }

		RenderSettings.skybox = access.skybox;

		mapCamera.orthographicSize = game.currentMaze.bounds.y * settings.wallSpacing;
		float size = game.currentMaze.bounds.y; //Mathf.Max (allMazes.current.use.GetUpperBound (0), allMazes.current.use.GetUpperBound (1)) * gameData.wallSpacing / 2f;
		float s1 = game.currentMaze.bounds.x; // * gameData.wallSpacing / 2f;
		mapCamera.rect = new Rect (1f - s1 / size * 0.25f, 0.6f, 1f, 1f);
	}

    void OnDisable() {
        foreach (cell c in game.currentMaze.grid)
            c.isEmpty = true;
    }

	public void diamondFound(string[] message){
		for (int i = 0; i < message.Length; i++) {
			if (message [i].Length > 10)
				menu [i].fontSize = 100;
			menu [i].text = message [i];
		}

		if (settings.mode == typeof(timed))
			FindObjectOfType<single> ().enabled = false;
		
        if (settings.mode == typeof(virtualReality))
        {
            FindObjectOfType<single>().mainCamera.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
            InvokeRepeating("checkForScreenPress", 0f, Time.deltaTime);
        }
        else
            levelOver.SetActive(true);

        moving(false);

        game.level = ++game.level % 5;
        if (message[1] == "Next Level")
        {
            new game();
            store.money += coinsCollected;
        }
	}

    public void checkForScreenPress() {
        if (Input.touches.Length == 0)
            startChecking = true;

        if (startChecking && Input.touches.Length > 0)
            Application.LoadLevel(1);
    }

    public void addCoin()
    {
        coinsCollected++;
        coins.text = "Coins: " + coinsCollected.ToString();
    }

	public void action(){
		if (menu [0].text == "Paused") {
			gamePlay (true);
			return;
		}

		Application.LoadLevel (1);
	}

	public void pause(){
		if (settings.showTutorial) return;

		menu [0].text = "Paused";
		menu [1].text = "Resume";
		gamePlay (false);
	}
	
	public void gamePlay(bool play){
		levelOver.SetActive (!play);
		moving (play);
		GetComponent<single> ().enabled = play;
	}

	public void moving(bool isMoving){
		foreach (movement move in GameObject.FindObjectsOfType<movement>()) {
			move.enabled = isMoving;
		}
	}

    public void loadMainMenu() {
        Application.LoadLevel(0);
    }
}