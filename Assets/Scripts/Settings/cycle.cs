using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System;
//using UnityEngine.EventSystems;

public class cycle : MonoBehaviour {
	[HideInInspector] public Vector3 targetPosition; //Position to be moved to

	public static float center = 50f;

    public UnityEngine.Object selectedObject {
        get { return choices.array[selected].obj; }
    }

    [HideInInspector] public int selected;
    [HideInInspector] public Image[] graphics;

    public Action makeChanges;
	public Choices choices;
	new public Camera camera;
	public Vector2 spacing;
    public string edit;
    public float yOffset;
	public int atATime;

	private Hashtable target = new Hashtable();

    public void Awake() {
        graphics = transform.GetComponentsInChildren<Image>();

        graphics[0].transform.position = new Vector2((camera.rect.x - 0.1f / 4f) * Screen.width, (camera.rect.y + camera.rect.height / 2f) * Screen.height);
        graphics[1].transform.position = new Vector2((camera.rect.x + camera.rect.width + 0.1f / 4f) * Screen.width, (camera.rect.y + camera.rect.height / 2f) * Screen.height);

        graphics[0].GetComponent<Button>().onClick.AddListener(delegate () {
            change(-1);
        });
        graphics[1].GetComponent<Button>().onClick.AddListener(delegate () {
            change(1);
        });

        graphics[2].transform.position = new Vector2((camera.rect.x + camera.rect.width / 2f) * Screen.width, (camera.rect.y + camera.rect.height / 2f) * Screen.height);
        graphics[2].GetComponent<Button>().onClick.AddListener(delegate () {
            if (store.buy(store.findItem(selectedObject.name))) {
                setSelected(selected);
                new settings().GetType().GetField(edit).SetValue(new settings(), selectedObject.name);
                dragButton.menu.updateCoins();
            }
        });
    }

    public void setUp() { 
		for (int i = 0; i < atATime; i++){
			addAt (selected - (int)(atATime / 2f) + i, cycle.center - ((int)(atATime / 2f) - i) * spacing.x);
		}
    }

	public void setSelected(int index){
        selected = index;

		for (int i = 0; i < 2; i++)
            graphics[i].gameObject.GetComponent<Button> ().enabled = true;
		if (selected == 0)
            graphics[0].gameObject.GetComponent<Button> ().enabled = false;
		else if (selected == choices.array.Length - 1)
            graphics[1].gameObject.GetComponent<Button> ().enabled = false;

        settingsController.allSelected = controller.canvas.GetComponentsInChildren<cycle>(true);

        graphics[2].GetComponent<Transform>().gameObject.SetActive(!store.bought.Contains(selectedObject.name));

        if (!store.bought.Contains(selectedObject.name)) {
            //graphics[2].GetComponent<Transform>().gameObject.SetActive(false);
            //settings s = new settings();
        //} else {
            //graphics[2].GetComponent<Transform>().gameObject.SetActive(true);
            graphics[2].GetComponentInChildren<Text>().text = "Buy for " + store.findItem(selectedObject.name).price.ToString() + " coins";
        }
	}

	public void change(int dir){
		setSelected (selected + dir);
		makeChanges ();
		addAt (selected + dir * (int)(atATime / 2f), center + (int)(atATime / 2f + 1) * dir * spacing.x);

		GameObject[] temp = new GameObject[target.Keys.Count];
		target.Keys.CopyTo (temp, 0);
		for (int i = 0; i < temp.Length; i++){
			temp[i].transform.position = getZ ((float)target[temp[i]]);
			target [temp[i]] = (float)target [temp[i]] - spacing.x * dir;
		}

		InvokeRepeating ("move", 0f, Time.deltaTime);
	}

	public void move(){
        //print((int)(atATime - 1f) * spacing.x);
		foreach (GameObject go in target.Keys) {
			if (Mathf.Abs ((float)target[go] - center) >= (int)(atATime / 2f + 1f) * spacing.x && Mathf.Abs (go.transform.position.x -(float)target[go]) < 0.1f) {
				Destroy (go);
				target.Remove (go);
				return;
			} else {
				go.transform.position = Vector3.Lerp (go.transform.position, getZ ((float)target[go]), Time.deltaTime * 5f);
			}
		}
	}

	public void addAt(int index, float x){
		if (index >= 0 && index < choices.array.Length) {
			GameObject temp = makeGameObject (index);
			temp.transform.position = getZ (x);
			target.Add (temp, x);
		}
	}

	public Vector3 getZ (float x){
		return new Vector3 (x, camera.transform.position.y - yOffset, Mathf.Abs ((x - center) / spacing.x * spacing.y));
	}

	public GameObject makeGameObject(int index){
		System.Type type = choices.array [0].obj.GetType ();

		if (type == typeof(GameObject)) { //It's already a GameObject
			GameObject temp = Instantiate (choices.array[index].obj) as GameObject;
			foreach (Component c in temp.GetComponents<Component>())
				if (c.GetType () != typeof(Transform))
					Destroy (c);

			return temp;
		} else if (type == typeof(Material)) {
			GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Plane);
			Material found = (Material)choices.array[index].obj;

			if (found.shader == Shader.Find ("Skybox/6 Sided")) //It's a skybox
				temp.GetComponent<Renderer> ().material.mainTexture = found.GetTexture ("_BackTex");
			else //It's a regular material
				temp.GetComponent<Renderer> ().material = found;

			temp.transform.rotation = Quaternion.Euler (270f, 0f, 0f);
			return temp;
		}

		return new GameObject();
	}
}