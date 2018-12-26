using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class game {
	public static maze currentMaze;
	public static int level = 0;

    public game() {
        currentMaze = new maze(4 + level + Random.Range(0, 2), 4 + level + Random.Range(0, 2));
    }
}
