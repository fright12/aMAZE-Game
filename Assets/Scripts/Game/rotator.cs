using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {
	void Update () {
		transform.Rotate (new Vector3 (25, 0, 0) * Time.deltaTime);
	}
}
