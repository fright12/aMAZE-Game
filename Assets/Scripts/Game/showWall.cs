using UnityEngine;
using System.Collections;

public class showWall : MonoBehaviour {
	private GameObject player;

	void Start(){
		gameObject.GetComponent<BoxCollider> ().isTrigger = true;
		gameObject.GetComponent<BoxCollider> ().center = new Vector3 (0f, 12f, 0f);
		gameObject.GetComponent<BoxCollider> ().size += new Vector3 ((settings.wallSpacing * 4 - 1) * xOrY(gameObject.transform.localScale.z), 0f, (settings.wallSpacing * 4 - 1)* xOrY(gameObject.transform.localScale.x));
	}

	void OnTriggerEnter(){
		gameObject.layer = 12;
		Destroy (gameObject.GetComponent<MonoBehaviour> ());
		Destroy (gameObject.GetComponent<MeshFilter> ());
		Destroy (gameObject.GetComponent<BoxCollider> ());
	}

	public int xOrY(float x){
		if (x<1f) return 0;
		return 1;
	}
}
