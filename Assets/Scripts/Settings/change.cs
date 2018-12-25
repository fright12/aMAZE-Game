using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Options{
	public GameObject selectPlayer, selectWall, selectGround, selectSkybox;
}

public class change : MonoBehaviour {
	public Options options;

	public GameObject samplePlayer;
	public RectTransform right;

	public Text coins;

	void Start () {
		changePlayer (options.selectPlayer);
		changeWallTexture (options.selectWall);
		changeGroundTexture (options.selectGround);
		changeSkybox (options.selectSkybox);

		for (int i = Mathf.Min (0, settings.current.player[0]-1)*-1; i<9-Mathf.Max (6, settings.current.player[0]+2); i++) makeGameObject (controller.get.players[settings.current.player[0]-1+i], new Vector3(48f+i*2f, 0f, Mathf.Abs (i-1)*0.5f)).transform.parent = GameObject.Find ("Cameras/Player").transform;
		for (int i = Mathf.Min (0, settings.current.wall-2)*-1; i<25-Mathf.Max (20, settings.current.wall+3); i++) makeGameObject (controller.get.materials[settings.current.wall-2+i], new Vector3(38f+i*6f, 20f, Mathf.Abs (i-2)*2f)).transform.parent = GameObject.Find ("Cameras/Wall").transform;
		for (int i = Mathf.Min (0, settings.current.ground-2)*-1; i<25-Mathf.Max (20, settings.current.ground+3); i++) makeGameObject (controller.get.materials[settings.current.ground-2+i], new Vector3(38f+i*6f, -20f, Mathf.Abs (i-2)*2f)).transform.parent = GameObject.Find ("Cameras/Ground").transform;
		for (int i = Mathf.Min (0, settings.current.skybox-2)*-1; i<12-Mathf.Max (7, settings.current.skybox+3); i++) makeGameObject (controller.get.skyboxes[settings.current.skybox-2+i], new Vector3(38f+i*6f, -40f, Mathf.Abs (i-2)*2f)).transform.parent = GameObject.Find ("Cameras/Sky").transform;

		foreach (Camera cam in GameObject.Find ("Cameras").GetComponentsInChildren<Camera>()) {
			//print (cam.ScreenToWorldPoint());
			//cam.camera.rect = new Rect((Screen.width-Screen.width*0.4f)/Screen.width, cam.camera.rect.y, cam.camera.rect.width, cam.camera.rect.height);
			if (cam.gameObject.name != "Player") cam.gameObject.SetActive (false);
		}
	}

	public void changePlayer(GameObject child){
		makeChanges (child, GameObject.Find ("Cameras/Player"), controller.get.players, new Vector3(2f, 0f, 0.5f), ref settings.current.player[0], 3);

		Destroy (samplePlayer);
		samplePlayer = Instantiate (controller.get.players [settings.current.player[0]], new Vector3 (0f, -20f, -4f), Quaternion.Euler (0f, 180f, 0f)) as GameObject;
		//settings.player [0] = players [player];
	}

	public void changeWallTexture(GameObject child){
		makeChanges (child, GameObject.Find("Cameras/Wall"), controller.get.materials, new Vector3 (6f, 20f, 2f), ref settings.current.wall, 5);

		foreach (Renderer rend in GameObject.Find ("SampleGame/Walls").GetComponentsInChildren<MeshRenderer>()) 
			controller.setAndScale(rend.transform.gameObject, controller.get.materials [settings.current.wall], new Vector2 (Mathf.Max(rend.transform.localScale.x,rend.transform.localScale.z)/settings.current.wallSpacing,1));

		//settings.wall = materials [wall];
	}

	public void changeGroundTexture(GameObject child){
		makeChanges (child, GameObject.Find ("Cameras/Ground"), controller.get.materials, new Vector3 (6f, -20f, 2f), ref settings.current.ground, 5);

		controller.setAndScale (GameObject.Find ("SampleGame/Ground"), controller.get.materials [settings.current.ground], new Vector2 (2, 2));
		//settings.ground = materials [ground];
	}

	public void changeSkybox(GameObject child){
		makeChanges (child, GameObject.Find ("Cameras/Sky"), controller.get.skyboxes, new Vector3 (6f, -40f, 2f), ref settings.current.skybox, 5);

		RenderSettings.skybox = controller.get.skyboxes [settings.current.skybox];
		//settings.skybox = skyboxes [skybox];
	}

	public void makeChanges(GameObject child, GameObject cam, Object[] array, Vector3 move, ref int index, int atATime){
		GameObject parent = child.transform.parent.gameObject;
		cycle[] options = cam.GetComponentsInChildren<cycle> ();

		RectTransform[] temp = parent.GetComponentsInChildren<RectTransform> (true);
		cam.GetComponent<Camera>().rect = new Rect (temp [2].transform.position.x / Screen.width + temp [2].rect.width * (Screen.width / 450f) / Screen.width / 2f,
		                            cam.GetComponent<Camera>().rect.y, //temp[1].transform.position.y / Screen.height - temp[1].rect.height * (Screen.height / 281f) / Screen.height * 1.75f / 2f,
		                            temp [1].transform.position.x / Screen.width - temp [2].transform.position.x / Screen.width - temp [2].rect.width * (Screen.width / 450f) / Screen.width,
		                            cam.GetComponent<Camera>().rect.height); //temp[1].rect.height * (Screen.height / 281f) / Screen.height * 1.75f);

		move *= Mathf.Sign (child.transform.position.x-parent.transform.position.x-50);
		index += (int)Mathf.Sign (move.x)*Mathf.Min (1, options.Length);

		if (settings.current.bought.Contains (array[index].name)) {
			parent.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive (false);
		}else if (child.activeInHierarchy) {
			//print (parent.GetComponentsInChildren<Text>(true)[1].transform.parent.gameObject.name);
			parent.GetComponentsInChildren<Button>(true)[2].gameObject.SetActive (true);
			//GameObject temp = Instantiate (priceTag) as GameObject;
			parent.GetComponentsInChildren<Text>(true)[1].text = "Buy For " + settingsController.findPrice (child) + " Coins";
			
			//temp.transform.position = parent.GetComponentsInChildren<Transform>(true)[1].position - new Vector3((parent.GetComponentsInChildren<Transform>(true)[1].position.x-parent.GetComponentsInChildren<Transform>(true)[0].position.x)/2f, 0f, 0f);
		}

		foreach (Button button in parent.GetComponentsInChildren<Button>(true)) button.interactable = true;

		if (index == array.Length-1) parent.GetComponentsInChildren<Button>(true)[0].interactable = false;
		if (index == 0) parent.GetComponentsInChildren<Button>(true)[1].interactable = false;
		if (options.Length == 0) return;

		if (Mathf.Clamp (index, atATime/2-1-Mathf.Sign (move.x), array.Length-atATime/2-Mathf.Sign (move.x)) == index)
			makeGameObject(array [index+atATime/2*(int)Mathf.Sign (move.x)], new Vector3 (50f + move.x * (atATime/2+1), move.y*Mathf.Sign (move.x), Mathf.Abs(move.z) * (atATime/2+1))).transform.parent = cam.transform;

		foreach (cycle o in cam.GetComponentsInChildren<cycle>()){
			o.transform.position = o.targetPosition;
			o.targetPosition -= new Vector3(move.x, 0f, move.z*Mathf.Sign(o.targetPosition.x-move.x-50+move.z));

			if (Mathf.Abs (o.targetPosition.x-50)>Mathf.Abs (move.x)*(atATime/2)) Destroy (o.transform.gameObject, 0.5f);
		}
	}

	public GameObject makeGameObject(Object obj, Vector3 position){
		GameObject temp = null;

		if (obj.GetType () == typeof(Material)){
			temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
			temp.transform.position = position;
			temp.transform.rotation = Quaternion.Euler (270f, 0f, 0f);

			//print (obj.name +", "+ ((Material)obj).shader);
			if (((Material)obj).shader == Shader.Find ("Skybox/6 Sided"))
				temp.GetComponent<Renderer>().material.mainTexture = ((Material)obj).GetTexture ("_BackTex");
				//print (((Material)obj).GetTexture ("_BackTex").name);
			else
				controller.setAndScale(temp, (Material)obj, new Vector2(1,1));
		}else if (obj.GetType () == typeof(GameObject)){
			temp = Instantiate (obj, position, Quaternion.Euler (0f, 180f, 0f)) as GameObject;
			if (temp.GetComponent<SpriteRenderer>()) temp.GetComponent<SpriteRenderer>().enabled = true;
		}

		foreach(Component comp in temp.GetComponents(typeof(Component))){
			if (comp.GetType () != typeof(Transform) && comp.GetType () != typeof(MeshRenderer) && comp.GetType () != typeof(SpriteRenderer)) 
				Destroy (comp);
		}
		temp.AddComponent<cycle>();

		return temp;
	}
}
