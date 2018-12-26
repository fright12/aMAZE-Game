using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class multiplayer : single {
	protected override System.Type movement2 {
		get { return typeof(playerMovement);}
	}

	protected GameObject player2Cam;
	protected Text[] winText;

	private static int[] wins = new int[]{0, 0};

	protected override void Start () {
		base.Start ();

		player2Cam = addCamera (get.normalCamera, player2);

		mainCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 0.5f, 1f);
		player2Cam.GetComponent<Camera> ().rect = new Rect (0.5f, 0f, 1f, 1f);

		player1.GetComponent<playerMovement> ().input = "Player1";
		player2.GetComponent<playerMovement> ().input = "Player2";

		get.winText.SetActive (true);
		winText = get.winText.GetComponentsInChildren<Text> ();
		for (int i = 0; i < winText.Length; i++)
			winText [i].text = wins[i].ToString ();
	}

	public override void endOfLevelMessage (GameObject winner){
		int winningPlayer = 1;
		if (winner.Equals (player2))
			winningPlayer = 2;

		wins [winningPlayer - 1]++;
		get.diamondFound (new string[]{"Player " + winningPlayer + " Wins!", "Next Level"});
	}
}