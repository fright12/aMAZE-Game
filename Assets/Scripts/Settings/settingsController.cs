using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class settingsController : MonoBehaviour {
    public static cycle[] allSelected;

    private float wallSpacing {
        get { return settings.wallSpacing; }
    }

    public void mainMenu() {
        Application.LoadLevel(0);
    }

	public void showTutorial(Toggle theTutorial){
        settings.showTutorial = theTutorial.isOn;
	}

	public void deleteData(){
		controller.delete = true;
	}

    void OnDisable() {
        foreach (cycle c in allSelected)
            if (store.bought.Contains(c.selectedObject.name))
                new settings().GetType().GetField(c.edit).SetValue(new settings(), c.selectedObject.name);

        dataManager.data[0].save();
    }
}