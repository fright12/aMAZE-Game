using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class transition : MonoBehaviour {
	public GameObject cam;
    public GameObject VRcam;
	public float speed;

	public Light directional;
	public Light fill;

    private Vector2 mapPosition;

	private float ratio;
	private float step;

	private int stage = 0;
	private Vector3[][] checkPoint = new Vector3[][]{
		new Vector3[] {new Vector3 (48f, 10f, -27f), new Vector3 (360f - Mathf.Atan (5f / Mathf.Sqrt (Mathf.Pow (92f, 2f) + Mathf.Pow (54f, 2f))) * Mathf.Rad2Deg, 360f - Mathf.Atan (92f / 54f) * Mathf.Rad2Deg, 0f)},
		new Vector3[] {new Vector3 (-46f, 5f, 25f), new Vector3 (0f, 315f, 0f)},
		new Vector3[] {Vector3.zero, Vector3.zero}
	};

	//Stage 0 - moving camera from above looking down to corner looking across
	//Stage 1 - moving camera from corner looking across to opposite corner
	//Stage 2 - moving camera to normal game view

	void Start(){
		DontDestroyOnLoad (gameObject);
	}

	public void begin(){
		if (!settings.playTransition || settings.mode == typeof(multiplayer)) {
            Application.LoadLevel(1);
            Destroy(gameObject);
            return;
        }

		if (stage == 0) {
			GameObject.Find ("Canvas/Play").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas/Settings").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas/Exit").GetComponent<Button> ().interactable = false;
		}

        RenderSettings.skybox = access.skybox;

        if (Application.isEditor && Application.loadedLevel == 10) { //Skip intro if editing
			speed = 2;
			stage = 2;
			Application.LoadLevel (1);
			return;
		}

		step = Vector3.Distance (cam.transform.position, checkPoint [stage] [0]) / speed;
		ratio = Vector3.Distance (cam.transform.position, checkPoint[stage][0]) / Quaternion.Angle (cam.transform.rotation, Quaternion.Euler (checkPoint[stage][1]));

        if (settings.mode == typeof(virtualReality) && Application.loadedLevel == 0 && stage == 0)
        {
            GameObject temp = Instantiate(VRcam) as GameObject;
            temp.transform.position = cam.transform.position;
            Destroy(cam);
            cam = temp;
        }

        InvokeRepeating ("move", 0f, Time.deltaTime);
	}

	void move(){
		cam.transform.position = Vector3.MoveTowards (cam.transform.position, checkPoint[stage][0], step);
        if (settings.mode != typeof(virtualReality))
		    cam.transform.rotation = Quaternion.RotateTowards (cam.transform.rotation, Quaternion.Euler (checkPoint[stage][1]), step / ratio);

		if (cam.transform.position == checkPoint [stage] [0]) {
			CancelInvoke("move");
			stage++;

			Invoke ("begin", 0.5f);

			if (stage == 1) {
				GameObject.Find ("Canvas").SetActive (false);
				CancelInvoke ("fade");
				InvokeRepeating ("changeLights", 0.5f, Time.deltaTime);
            } else if (stage == 2) {
				Application.LoadLevel (1);
				CancelInvoke ("begin");
				CancelInvoke ("changeLights");
                InvokeRepeating("waitForLoad", 0f, Time.deltaTime);
			} else if (stage == 3) {
				cam.transform.SetParent (GameObject.FindObjectOfType<playerMovement>().gameObject.transform);
                GameObject.Find("Canvas/Pause Button").GetComponent<Button>().enabled = true;
                Destroy(gameObject);
                GameObject.FindObjectOfType<gameController>().GetComponent<gameController>().moving(true);
            }

            if (stage > 1)
				return;
		}

		if (stage == 0) {
			fade (-1f / speed);
		} else if (stage == 2) {
			fade (1f / speed);

			Camera temp = GameObject.Find ("Map Camera").GetComponent<Camera>();
			temp.rect = new Rect(temp.rect.x - (1f - mapPosition.x) * 1f / speed, temp.rect.y - (1f - mapPosition.y) * 1f / speed, 1f, 1f);
		}
	}

    public void waitForLoad() {
        if (Application.loadedLevel == 1) {
            cam = GameObject.FindObjectOfType<single>().mainCamera;
            cam.transform.position = game.currentMaze.virtualToPhysical(game.currentMaze.upperLeft, 5f) + new Vector3(-1f, 0f, 1f);
            if (settings.mode != typeof(virtualReality))
                cam.transform.rotation = Quaternion.Euler(0f, 315f, 0f);

            GameObject player = GameObject.FindObjectOfType<playerMovement>().gameObject;
            GameObject.FindObjectOfType<gameController>().GetComponent<gameController>().moving(false);

            Camera temp = GameObject.Find("Map Camera").GetComponent<Camera>();
            mapPosition = new Vector2(temp.rect.x, temp.rect.y);
            temp.rect = new Rect(1f, 1f, 1f, 1f);

            GameObject.Find("Canvas/Pause Button").GetComponent<Button>().enabled = false;

            fade(-1f);

            checkPoint[2][0] = new Vector3(player.transform.position.x - Mathf.Sin(player.transform.eulerAngles.y * Mathf.Deg2Rad), 1.75f, player.transform.position.z - Mathf.Cos(player.transform.eulerAngles.y * Mathf.Deg2Rad));
            checkPoint[2][1] = new Vector3(10f, player.transform.eulerAngles.y, 0f);

            CancelInvoke("waitForLoad");
            begin();
        }
    }

	public void fade(float percent){
		foreach (Graphic g in GameObject.FindObjectsOfType<Graphic>()) 
			g.color = new Color (g.color.r, g.color.g, g.color.b, g.color.a + percent);
	}

	void changeLights(){
		directional.transform.rotation = Quaternion.RotateTowards (directional.transform.rotation, Quaternion.Euler (60f, 60f, 0f), step * 5f / ratio);
		fill.transform.rotation = Quaternion.RotateTowards (fill.transform.rotation, Quaternion.Euler (300f, 300f, 0f), step * 10f / ratio);
	}

	private void pause(float seconds){
		CancelInvoke ("move");
		InvokeRepeating ("move", seconds, Time.deltaTime);
	}
}
