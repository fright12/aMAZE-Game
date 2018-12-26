using UnityEngine;
using System.Collections;

public class timed : single {
	private UnityEngine.UI.Text timer;
	private float timeLeft;

	protected override void Start (){
		base.Start ();

		timer = get.timer;
		timer.gameObject.SetActive (true);

		timeLeft = (game.currentMaze.bounds.x + game.currentMaze.bounds.y) * 2f;
	}
	
	void FixedUpdate(){
		if (Mathf.RoundToInt (timeLeft) == 0){
			get.diamondFound (loseMessage);
		} else {
			timeLeft -= Time.deltaTime;
			timer.text = "Time: " + timeLeft.ToString("0");
		}
	}
}