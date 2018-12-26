using UnityEngine;
using System.Collections;

public class computer : single {
	protected override System.Type movement2 {
		get { return typeof(computerMovement);}
	}

	public override void endOfLevelMessage (GameObject winner) {
		if (winner.Equals (player1))
			get.diamondFound (winMessage);
		else
			get.diamondFound (loseMessage);
	}
}