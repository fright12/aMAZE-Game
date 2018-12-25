using UnityEngine;
using System.Collections;

public class tutorial : MonoBehaviour {
	void Start () {
	}

	void Update () {
		gameObject.transform.position = new Vector3 (0f, 0f, 0f);

		if (Input.GetAxis ("Vertical") > 0)
			GetComponent<Animator>().SetFloat (GetComponent<hashIDs> ().speedFloat, 5.5f);
		else
			GetComponent<Animator>().SetFloat (GetComponent<hashIDs> ().speedFloat, 0f);
	}
}