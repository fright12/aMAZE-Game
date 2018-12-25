using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADVERTISEMENTS : MonoBehaviour {
	void Awake() {
		if (Advertisement.isSupported) {
			Advertisement.allowPrecache = true;
			Advertisement.Initialize ("131621742",true);
		} else {
			Debug.Log("Platform not supported");
		}
	}
	
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 150, 50), Advertisement.isReady() ? "Show Ad" : "Waiting...")) {
			// Show with default zone, pause engine and print result to debug log
			Advertisement.Show(null, new ShowOptions {
				pause = true,
				resultCallback = result => {
					Debug.Log(result.ToString());
				}
			});
		}
	}
}

