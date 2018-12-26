using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuControl : MonoBehaviour {
	public static GameObject menu {
		get { return GameObject.FindObjectOfType<menuControl>().gameObject;}
	}

	public static string firstMenu = "Environment";
	public static Vector2 bounds;
	public static Vector2 buttonSize = new Vector2 (200, 100);

    public Text coins;

	private float top = 150f;
	private dragButton[] all = new dragButton[0];
	private int buttonCount;

	void Start () {
		bounds = new Vector2 (controller.canvas.rect.width / -2f, controller.canvas.rect.width / -2f - buttonSize.x);
		buttonCount = GameObject.Find ("Canvas").GetComponentsInChildren<dragButton> (true).Length;

        updateCoins();

        foreach (dragButton db in GameObject.Find ("Canvas").GetComponentsInChildren<dragButton>(true)) {
			db.gameObject.SetActive (true);

			if (db.name == firstMenu) {
				dragButton.openMenu = db;
				reverse();
				db.gameObject.transform.position += new Vector3(10f, 0f, 0f);
				openMenu ();
                db.location.sizeDelta = new Vector2(controller.canvas.rect.width - 50f, controller.canvas.rect.height - 100f);
                db.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
                db.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = new Vector2(0f, 287.5f);
            }
            else
				addToMenu (db);
		}
	}

	public void addToMenu(dragButton button){
		button.location.sizeDelta = new Vector2(200, 100);
		button.location.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
		button.GetComponentsInChildren<Text> (true)[0].color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
		button.GetComponentsInChildren<Text> (true)[0].gameObject.SetActive (true);
		button.GetComponentInChildren<Text> ().GetComponent<RectTransform> ().localPosition = Vector2.zero;

        button.controls.transform.localScale = new Vector2 (200f / (controller.canvas.rect.width - 50f), 100f / (controller.canvas.rect.height - 50f));
		button.controls.SetActive (false);

		button.enabled = false;

		button.transform.SetParent (transform);

		if (button.transform.position.x >= Screen.width / 2f){
			button.GetComponent<RectTransform>().localPosition = new Vector2 (100f, top - (buttonCount - 2) * buttonSize.y);
			
			all = transform.GetComponentsInChildren<dragButton> (true);

			all [0].location.localPosition = new Vector2 (100f, top);
			for (int i = 1; i < all.Length; i++){
				if (all[i - 1].location.localPosition.y - all[i].location.localPosition.y > buttonSize.y)
					all[i].GetComponent<RectTransform> ().localPosition += new Vector3 (0f, buttonSize.y);
			}
		}
	}

	public void openMenu(){
		dragButton.openMenu.enabled = true;
		dragButton.openMenu.enabled = false;
	}

    public void updateCoins() {
        coins.text = "Coins: " + store.money;
    }

    public static void reverse(){
		if (bounds.x != menu.GetComponent<RectTransform>().localPosition.x)
			bounds = new Vector2 (bounds.y, bounds.x);
	}
}