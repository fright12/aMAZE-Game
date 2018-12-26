using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class dragButton : MonoBehaviour {
	public static menuControl menu;
	public static dragButton openMenu = null; //Which menu is open?
	public static dragButton lastHeld;
	public static Vector2 size;

	public minMax bounds; //The bounds the button can move within

	public Camera[] allCameras;
	public look origin;
	public look destination;
    public int font = 0;

	[HideInInspector] public GameObject controls;
	[HideInInspector] public RectTransform location;

	public float follow {
		get {
			if (enabled)
				return mouse.x - offset.x;
			else if (Vector2.Distance (location.localPosition, forward) < 1f)
				return forward.x;
			else
				return Vector2.Lerp (location.localPosition, forward, lerpSpeed).x;
		}
	}
	[HideInInspector] public float percent;

	private Vector2 mouse {
		get {
			return new Vector2(
				Input.mousePosition.x / Screen.width * controller.canvas.rect.width - controller.canvas.rect.width / 2f,
				Input.mousePosition.y / Screen.height * controller.canvas.rect.height - controller.canvas.rect.height / 2f
				);
		}
	}
	private Vector2 forward;
	private Vector2 offset;
	private float lastPos;
	private float lastDis;
	private float thrown;
	private float lerpSpeed;

	public void Awake(){
		menu = GameObject.FindObjectOfType<menuControl> ();
		controls = GetComponentsInChildren<Transform> (true) [2].gameObject;
		location = GetComponent<RectTransform>();
		size = new Vector2 (controller.canvas.rect.width - 50f, controller.canvas.rect.height - 100f);
		//allCameras = GetComponent<cameraControl> ();
	}

	void Update(){
		lastDis = Mathf.Abs (location.localPosition.x - lastPos);
		lastPos = location.localPosition.x;

		percent = Mathf.Clamp01((follow - origin.transform.x) / (destination.transform.x - origin.transform.x));
		if (origin.position.x < 0){
			float expand = (menuControl.buttonSize.x) / (destination.position.x - origin.position.x);
			percent = Mathf.Clamp01(percent - expand) * (1f / (1f - expand));

			GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = new Vector2(0f, percent * 287.5f);
            //GetComponentInChildren<Text>().fontSize = (int)(font + percent * (75 - font));

            location.sizeDelta = current (origin.size, destination.size);
			controls.transform.localScale = current (origin.scale, destination.scale);
			
			//Change brightness
			//GetComponentInChildren<Text> ().color = setA (GetComponentInChildren<Text> ().color, 1f - percent); //Text color
			GetComponent<Image> ().color = setA (GetComponent<Image> ().color, current (origin.transparence, destination.transparence));
			foreach (MaskableGraphic mg in controls.GetComponentsInChildren<MaskableGraphic>(true))
                if (mg.gameObject.tag != "noFade")
				mg.color = setA (mg.color, percent);
		}

		pushOff ();
		menuControl.menu.GetComponent<RectTransform> ().localPosition = new Vector2 (lastHeld.current (menuControl.bounds.x, menuControl.bounds.y), 0f);

		location.localPosition = new Vector2 (follow, current (origin.position, destination.position).y);

		if (!enabled && follow == forward.x) checkForStop ();
	}

	public void pushOff(){
		if (openMenu != this) {
			if (openMenu.IsInvoking ())
				openMenu.CancelInvoke ();

			RectTransform a = GetComponent<RectTransform> ();
			RectTransform b = openMenu.GetComponent<RectTransform> ();

			if (!enabled && forward == destination.position && Mathf.Abs (location.localPosition.x - lastPos) < lastDis)
				openMenu.location.localPosition = new Vector2(controller.canvas.rect.width / 2f + location.rect.width / 2f, -25f);
			else if (a.localPosition.x + a.rect.width / 2f > 200 - b.rect.width / 2f)
				openMenu.location.localPosition = new Vector2 (a.localPosition.x + a.rect.width / 2f + b.rect.width / 2f, -25f);
		}
	}

	public void checkForStop(){
		if (location.localPosition.x != forward.x) {
			CancelInvoke ();
			Update ();
		} else {
			if (location.localPosition.x < 0f) {
				menu.addToMenu(this);
			} else if (openMenu != this) {
				menu.addToMenu (openMenu);
				openMenu.setAllCameras(false);
				openMenu = this;
                menuControl.firstMenu = this.name;
			}
		}
	}

	void OnEnable(){
		if (lastHeld == this && IsInvoking ()) {
			lerpSpeed = 0.5f;
			this.enabled = false;
			return;
		}
		lastHeld = this;

		menuControl.reverse ();

		destination = new look (new Rect(0, -25f, controller.canvas.rect.width - 50f, controller.canvas.rect.height - 100f), 50f / 255f, 1f);

		if (transform.parent.name == "Canvas") {
			if (location.localPosition.x == 0){
				destination.transform.x = 200f;
			}
		} else {
			transform.SetParent (transform.parent.parent);
		}

		forward = destination.position;
		origin = new look (new Rect(location.localPosition.x, location.localPosition.y, location.rect.width, location.rect.height), GetComponent<Image>().color.a, controls.transform.localScale.x);
        //font = GetComponentInChildren<Text>().fontSize;
        offset = mouse - origin.position;

		lerpSpeed = 0.1f;

		controls.SetActive (true);
		setAllCameras(true);
	}

	void OnDisable(){
		if (origin.position.x < 0f && (Mathf.Sign (location.localPosition.x - lastPos) < 0f || Mathf.Approximately (location.localPosition.x, lastPos) && lastPos < 0f))
			forward = origin.position;

		//thrown = Mathf.Abs (location.localPosition.x - lastPos);
		//allCameras.enabled = false;

		InvokeRepeating ("Update", 0f, Time.deltaTime);
	}

	public void setAllCameras(bool status){
		foreach (cameraControl cc in GetComponentsInChildren<cameraControl>(true))
			cc.enabled = status;
	}

	public float current(float start, float finish){
		return start + percent * (finish - start);
	}

	public Vector2 current(Vector2 start, Vector2 finish){
		return new Vector2 (current (start.x, finish.x), current (start.y, finish.y));
	}

	public Color setA(Color c, float a){
		return new Color (c.r, c.g, c.b, a);
	}
}

public class look {
	public Rect transform;
	public float transparence;
	public float scaleRatio;

	public Vector2 position {
		get { return new Vector2 (transform.x, transform.y); }
	}
	public Vector2 size {
		get { return new Vector2 (transform.width, transform.height); }
	}
	public Vector2 scale {
		get { return new Vector2 (scaleRatio, scaleRatio); }
	}

	public look (Rect trnsfrm, float trnsprnc, float sclRt){
		transform = trnsfrm;
		transparence = trnsprnc;
		scaleRatio = sclRt;
	}
	
	public override string ToString () {
		return "(" + transform.x + ", " + transform.y + ")" + ", " + "(" + transform.width + ", " + transform.height + ")";
	}
}

public class olddragButton : MonoBehaviour { //Moves the button and transition between button graphic and new menu graphic
	/*public static dragButton openMenu = null; //Which menu is open?
	
	public bool isNewScene = false; //Is the thing being switched to a new scene?
	public List<Camera> cameras; //Any cameras that are in the new menu
	public GameObject switchTo = null; //The new menu to transition to
	public minMax bounds; //The bounds the button can move within

	private minMax button; //The bounds for width and height of button
	private minMax scale; //The bounds for scale of switchTo
	private minMax[] cameraRect; //The bounds for the rect components of all the cameras

	private static string cameraPriority; //Which button should the cameras move with?
	private static float max; //The highest any button can be
	private bool isMouseDown;
	private bool bounce;
	private float offset; //The distance between the mouse click and the center of the button
	private Vector2 pos; //The position of the button
	private Vector2[] camPos;

	public void clicked(){
		if (transform.position.x < bounds.xMin + button.xMin && openMenu == null)
			bounce = true;
	}

	void Start(){
		if (isNewScene) {
			bounds.xMax = Screen.width / 2f;
			bounds.yMax = Screen.height / 2f;
		}

		//Change bounds based on the size of the screen
		bounds.xMin *= (Screen.width / 450f); bounds.xMax *= (Screen.width / 450f);
		bounds.yMin *= (Screen.height / 281f); bounds.yMax *= (Screen.height / 281f);

		if (bounds.yMin > max) max = bounds.yMin;

		//Define boundaries for button width/height and switchTo scale
		button = new minMax (100f, gameObject.GetComponent<RectTransform> ().rect.width, 100f / 3f, gameObject.GetComponent<RectTransform> ().rect.height);
		scale = new minMax (button.xMin / button.xMax, 1f, button.yMin / button.yMax, 1f);

		//Define boundaries for camera rect properties
		cameraRect = new minMax[cameras.Count];
		camPos = new Vector2[cameras.Count];
		for (int i = 0; i<cameras.Count; i++) {
			camPos[i] = new Vector2(transform.position.x / Screen.width - cameras[i].rect.x, transform.position.y / Screen.height - cameras[i].rect.y);
			cameraRect [i] = new minMax(cameras[i].rect.width * scale.xMin, cameras[i].rect.width, cameras[i].rect.height * scale.yMin, cameras[i].rect.height); //width and height bounds
		}

		transform.position = new Vector2 (bounds.xMin, bounds.yMin); //Set button position to top right corner
	}

	void Update(){
		if (!Input.GetMouseButton(0)) isMouseDown = false; //If mouse no longer clicked, no drag

		if (isMouseDown) { //Is the button being dragged?
			pos = new Vector2 (Mathf.Clamp (Input.mousePosition.x - GetComponent<RectTransform> ().rect.width * offset, bounds.xMin, bounds.xMax) - decelerate (Input.mousePosition.x - GetComponent<RectTransform> ().rect.width * offset), transform.position.y); //Change pos based on mouse input
			transform.position = new Vector2 (pos.x, transform.position.y);
		} else if (bounce) {
			pos = Vector2.Lerp (pos, new Vector2(bounds.xMin + button.xMin / 2f * Screen.width / 450f, pos.y), 0.1f);

			if (pos.x > Mathf.Round (bounds.xMin + button.xMin / 5f * Screen.width / 450f))
				bounce = false;
		} else if (!isMouseDown) {
			transform.position = Vector3.Lerp (transform.position, new Vector3 (closest (bounds.xMin, bounds.xMax), pos.y), 0.1f);
			pos = transform.position; //Change pos based on button's position
		}

		/*if (openMenu && transform.position.x > openMenu.bounds.xMin + openMenu.button.xMin * (Screen.width / 450f) && cameraPriority != name) {
			bounds = new minMax(bounds.xMin, Screen.width + button.xMin);
		}

		transform.position = new Vector2 (pos.x, expand (bounds).y); //Change y position of objects
		GetComponent<RectTransform> ().sizeDelta = expand (button); //Change button size

		//Fix cameras
		if (cameraPriority == name) { //If cameras are moving with this button
			for (int i = 0; i<cameras.Count; i++) { //For each camera that needs to be fixed
				cameras [i].rect = new Rect ( //Change rect values with position of button
					transform.position.x / Screen.width - camPos [i].x * GetComponent<RectTransform> ().rect.width / button.xMax,
					transform.position.y / Screen.height - camPos [i].y * GetComponent<RectTransform> ().rect.height / button.yMax,
					expand (cameraRect [i]).x,
					expand (cameraRect [i]).y);
			}
		}

		dragButton[] all = GameObject.Find ("Canvas").GetComponentsInChildren<dragButton>(); //Collection of all menu buttons

		//If a menu is open and its not the top of the list and it is no longer in the stack of buttons
		if (openMenu && all[all.Length - 1] != openMenu && openMenu.transform.position.x > openMenu.bounds.xMin + openMenu.button.xMin * (Screen.width / 450f)) { //If the first one in the list is not the open menu
			openMenu.transform.SetAsLastSibling (); //Move to top of list in hierarchy
			isMouseDown = false;
		}

		for (int i = 0; i < all.Length - 1; i++) //Make sure all buttons are evenly spaced
			all[i].bounds.yMin = Mathf.Lerp (all[i].bounds.yMin, max - (all.Length - 1 - i) * 29f * (Screen.height / 281f), Time.deltaTime); //Move the upper bound until the button is at the correct distance

		//Change brightness
		MaskableGraphic tempColor = GetComponentInChildren<Text>(); //Text color
		GetComponentInChildren<Text>().color = new Color (tempColor.color.r, tempColor.color.g, tempColor.color.b, 1f-(tempColor.gameObject.transform.position.x-bounds.xMin)/((bounds.xMax - bounds.xMin) * 3f / 4f));
		
		tempColor = GetComponent<Image>(); //Actual button color (fades to 50/255f)
		GetComponentInChildren<Image>().color = new Color (tempColor.color.r, tempColor.color.g, tempColor.color.b, 1f-(tempColor.gameObject.transform.position.x-bounds.xMin)/((bounds.xMax - bounds.xMin) * 3f / 4f) * 150f/255f);

		//Make menu to switch to follow button
		if (openMenu == this && !switchTo && !Application.isLoadingLevel) {
			//foreach (Camera cam in GameObject.Find ("Package").GetComponentsInChildren<Camera>())
			//	print (cam.name);
			cameras.Add(GameObject.Find ("Main Camera").GetComponent<Camera>());
			this.Start ();
			GameObject.Find ("Main Camera").GetComponent<Camera>().rect = new Rect(0.2f, 0.2f, 0.2f, 0.2f);

			//GameObject.Find ("Package").transform.localScale = new Vector2(scale.xMin, scale.yMin);

			//cameras = new Camera[]{GameObject.Find("Main Camera").GetComponent<Camera>()};
		} else if (switchTo) {
			switchTo.transform.position = transform.position;
			switchTo.transform.localScale = expand (scale) * 0.5f;
			foreach (MaskableGraphic graphic in switchTo.GetComponentsInChildren<MaskableGraphic> ()) //All text and images in the switchTo
				if (graphic.tag != "noFade")
					graphic.color = new Color (graphic.color.r, graphic.color.g, graphic.color.b, (transform.position.x - bounds.xMin - button.xMin) / ((bounds.xMax - bounds.xMin - button.xMin) * 3f / 4f));
		}
	}
	
	public Vector2 expand(minMax boundaries){ //Change everything as button moves between lower bound and upper bound
		return new Vector2 (
			Mathf.Clamp (boundaries.xMin + (boundaries.xMax - boundaries.xMin) * Mathf.Clamp01((pos.x - bounds.xMin - button.xMin * (Screen.width / 450f)) / (bounds.xMax - bounds.xMin - button.xMin * (Screen.width / 450f))), Mathf.Min (boundaries.xMin, boundaries.xMax), Mathf.Max(boundaries.xMin, boundaries.xMax)),
			Mathf.Clamp (boundaries.yMin + (boundaries.yMax - boundaries.yMin) * Mathf.Clamp01((pos.x - bounds.xMin - button.xMin * (Screen.width / 450f)) / (bounds.xMax - bounds.xMin - button.xMin * (Screen.width / 450f))), Mathf.Min (boundaries.yMin, boundaries.yMax), Mathf.Max(boundaries.yMin, boundaries.yMax)));
	}
	
	public float decelerate(float value){ //Make the button slow down as it passes over its bounds
		int max = 50;

		if (value < bounds.xMin)
			return -max * Mathf.Exp (-1f / max * (bounds.xMin - value)) + max;
		else if (value > bounds.xMax)
			return max * Mathf.Exp (-1f / max * (value - bounds.xMax)) - max;
		else
			return 0f;
	}

	public float closest(float x, float y){ //Find if the button is closer to the lower bound or the upper bound
		if (transform.position.x > bounds.xMin + button.xMin * (Screen.width / 450f))
			return y;
		else
			return x;
	}

	public void startDrag(){ //Start the drag operation
		//if (openMenu != null && openMenu != this) return; //If there's a menu in the space, no drag
		if (openMenu == this) openMenu.bounds.yMin = max; //If this menu is open, it should go back to the top of the stack

		isMouseDown = true; //Mouse is now down
		openMenu = this; //This button is the open menu
		offset = (Input.mousePosition.x - transform.position.x) / GetComponent<RectTransform>().rect.width; //Set the offset
		cameraPriority = name; //Cameras should move with this button

		if (isNewScene) {
			GameObject oldStuff = new GameObject();
			oldStuff.name = "oldStuff";

			foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
				if (obj.transform.parent == null)
					obj.transform.SetParent(oldStuff.transform);

			Application.LoadLevelAdditive (0);
			return;
		}

		//Fade all graphical components down all the way
		foreach(UnityEngine.UI.MaskableGraphic graphic in switchTo.GetComponentsInChildren<UnityEngine.UI.MaskableGraphic> ())
			graphic.color = new Color (graphic.color.r, graphic.color.g, graphic.color.b, 0f);

		//Turn on everything
		switchTo.SetActive (true);
		foreach (Camera cam in cameras)
			cam.gameObject.SetActive (true);
	}

	public void stopDrag(){ //Stop drag operation
		if (openMenu != null && openMenu != this) return; //If there's a menu in the space, no drag

		isMouseDown = false; //Mouse is no longer down

		//If the menu is not open, turn everything off
		if (openMenu.transform.position.x < openMenu.bounds.xMin + openMenu.button.xMin * (Screen.width / 450f)){
			switchTo.SetActive (false);
			foreach (Camera cam in cameras)
				cam.gameObject.SetActive (false);
		}

		openMenu = null;
		foreach (dragButton temp in GameObject.FindObjectsOfType<dragButton>())
			if (temp.transform.position.x > temp.bounds.xMin + temp.button.xMin * (Screen.width / 450f))
				openMenu = temp;
	}*/
}

[System.Serializable]
public class minMax { //Defines bounds of a space to move around in
	public float xMin, xMax, yMin, yMax;
	
	public minMax () {}
	public minMax (float x1, float x2) {xMin = x1; xMax = x2;}
	public minMax (float x1, float x2, float y1, float y2) {xMin = x1; xMax = x2; yMin = y1; yMax = y2;}
	
	public override string ToString(){
		return "xMin: " + xMin + ", xMax: " + xMax + ", yMin: " + yMin + ", yMax: " + yMax;
	}
}

[System.Serializable]
public class camInfo {
	public Camera cam;
	public Vector2 position;
	public minMax rect;

	public camInfo(){}
	public camInfo(Camera c, Vector2 pos, minMax r){
		cam = c;
		pos = position;
		rect = r;
	}
}

	/*void Update(){
		if (isMouseDown)
			gameObject.transform.position = new Vector2 (Mathf.Clamp (Input.mousePosition.x - offset, bounds.x, bounds.y) - decelerate (), gameObject.transform.position.y);
		else if (!isMouseDown) 
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, new Vector3(closest(bounds.x, bounds.y), gameObject.transform.position.y, gameObject.transform.position.z), 0.1f);

		float frac = 1f / 2f;
		//Change y position
		gameObject.transform.position = new Vector2 (gameObject.transform.position.x,
		                                             bounds.z - (bounds.z - bounds.w) * (gameObject.transform.position.x - bounds.x - (bounds.y - bounds.x) * frac) / (bounds.y - bounds.x - (bounds.y - bounds.x) * frac) * Mathf.Clamp01(Mathf.Round((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) * 1f )));
		switchTo.transform.position = gameObject.transform.position;

		//Change button size              75, 25 -> 340, 185
		//float exponential = Mathf.Exp (Mathf.Log (2) / 210f * (gameObject.transform.position.x - bounds.x))-1;
		//print (exponential+", "+((gameObject.transform.position.x - bounds.x))+", "+gameObject.transform.position.x);
		//print (original.y - gameObject.transform.position.y);

		//print ((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) + ", " + Mathf.Clamp01(Mathf.Round ((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) * 4f)));
		Rect temp = gameObject.GetComponent<RectTransform> ().rect;
		//if ((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) > 0.25f) 
		//print ((bounds.y - bounds.x)/
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(
			75f + (342.5f - 75f) * (gameObject.transform.position.x - bounds.x - (bounds.y - bounds.x) * frac) / (bounds.y - bounds.x - (bounds.y - bounds.x) * frac) * Mathf.Clamp01(Mathf.Round((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) * 1f )),
			25f + (187.5f - 25f) * (gameObject.transform.position.x - bounds.x - (bounds.y - bounds.x) * frac) / (bounds.y - bounds.x - (bounds.y - bounds.x) * frac) * Mathf.Clamp01(Mathf.Round((gameObject.transform.position.x - bounds.x) / (bounds.y - bounds.x) * 1f )));

		//Make thing bigger
		switchTo.transform.localScale = new Vector3 (
			0.2f + (switchTo.transform.position.x - bounds.x) / (bounds.y - bounds.x) * (1f - 0.2f),
			0.125f + (switchTo.transform.position.x - bounds.x) / (bounds.y - bounds.x) * (1f - 0.125f),
			switchTo.transform.localScale.z);

		//Change brightness
		float newRight = (bounds.y - bounds.x) * 3f / 4f;

		MaskableGraphic tempColor = gameObject.GetComponentInChildren<Text>();
		gameObject.GetComponentInChildren<Text>().color = new Color (tempColor.color.r, tempColor.color.g, tempColor.color.b, 1f-(tempColor.gameObject.transform.position.x-bounds.x)/newRight);

		tempColor = gameObject.GetComponent<Image>();
		gameObject.GetComponentInChildren<Image>().color = new Color (tempColor.color.r, tempColor.color.g, tempColor.color.b, 1f-(tempColor.gameObject.transform.position.x-bounds.x)/newRight * 150f/255f);

		foreach (UnityEngine.UI.MaskableGraphic graphic in switchTo.GetComponentsInChildren<UnityEngine.UI.MaskableGraphic> ())
			graphic.color = new Color (graphic.color.r, graphic.color.g, graphic.color.b, (gameObject.transform.position.x - bounds.x) / newRight);
	}*/