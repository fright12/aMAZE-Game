using UnityEngine;
using System.Collections;

public class pulse : MonoBehaviour {
	public Color color;

	private float maxR;
	private float maxG;
	private float maxB;

	private bool black;

	void Start (){
		maxR = color.r;
		maxG = color.g;
		maxB = color.b;

		GetComponent<Renderer>().material.color = color;

		transform.localScale = new Vector3 (settings.wallSpacing, 0.1f, settings.wallSpacing);
	}

	void Update() {
		if (black){
			toBlack();
		}else{
			toColor();
		}
		//color = Color.Lerp(Color.white, Color.black, Time.time);
		//else if (!upOrDown){renderer.material.color = Color.Lerp(Color.black, renderer.material.color, Time.deltaTime);}
		//if (renderer.material.color == Color.black||renderer.material.color == color){upOrDown = !upOrDown;}
		//print (upOrDown);
	}

	public void toBlack(){
		GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.black, Time.deltaTime);
		if (GetComponent<Renderer>().material.color == Color.black){black = false;}
	}

	public void toColor(){
		GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, color, Time.deltaTime);
		if (GetComponent<Renderer>().material.color == color){black = true;}
	}

	void FixedUpdate (){
		color.r -= maxR/50f;
		color.g -= maxG/50f;
		color.b -= maxB/50f;

		GetComponent<Renderer>().material.color = new Color (Mathf.PingPong(color.r,maxR), Mathf.PingPong(color.g,maxG), Mathf.PingPong(color.b,maxB));
	}
}
