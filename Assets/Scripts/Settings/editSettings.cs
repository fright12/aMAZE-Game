using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;

public class editSettings : MonoBehaviour {
    public GameObject modes;

    public GameObject colorSwatch;
    public Selectable currentColor;
    public Toggle transitionState;

    public Text directions;

    void Start() {
        osChanges();

        currentColor.image.color = access.player1.color;
        transitionState.isOn = settings.playTransition;

        foreach (Button b in modes.GetComponentsInChildren<Button>())
            if (b.name.ToLower() == settings.mode.ToString().ToLower())
                b.interactable = false;
    }

    private void osChanges() {
        if (!Application.isMobilePlatform) {
            modes.GetComponentsInChildren<Button>(true)[3].gameObject.SetActive(true);

            directions.text = directions.text.Substring(0, directions.text.IndexOf("Controls") + 9) +
                "\nUse the arrow keys to control player movement \n \n";
		} else {
            modes.GetComponentsInChildren<Button>(true)[4].gameObject.SetActive(true);

            directions.text = directions.text.Substring(0, directions.text.IndexOf("Controls") + 9) +
                "\nTilt forward to run\n" +
                "Press the right side of the screen to turn right, and the left side to turn left";
        }
    }

    public void changeMode(Selectable button) {
        foreach (Button b in modes.GetComponentsInChildren<Button>())
            b.interactable = true;
        button.interactable = false;

        settings.mode = System.Type.GetType(button.name);
    }

    public void changeColor(Image button) {
        settings.color1 = new float[] { button.color.r, button.color.g, button.color.b, 1f };

        currentColor.image.color = button.color;
        colorSwatch.SetActive(false);
    }

    public void changeTransition(Toggle t) {
        settings.playTransition = t.isOn;
    }

    void LateUpdate() {
        if (colorSwatch.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0))
            Invoke("closeColorSwatch", 0.2f);
    }

    private void closeColorSwatch() {
        colorSwatch.SetActive(false);
    }

    public void openWebsiteURL() {
        Application.OpenURL("http://productionsefd.wix.com/efd-productions");
    }
}
