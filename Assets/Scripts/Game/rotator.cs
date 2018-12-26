using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {
	public int speed;

	void Update () {
		transform.Rotate (new Vector3 (0f, 0f, speed) * Time.deltaTime);
	}
}
