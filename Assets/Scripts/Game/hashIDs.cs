using UnityEngine;
using System.Collections;

public class hashIDs : MonoBehaviour {
	public int locomotionState;
	public int speedFloat;
	
	void Awake (){
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		speedFloat = Animator.StringToHash("Speed");
	}
}
