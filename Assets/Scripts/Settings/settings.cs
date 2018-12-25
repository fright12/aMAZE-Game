using UnityEngine;
using System.Collections;

[System.Serializable]
public class settings {
	public static settings current; //Instance of all currently selected options that can be easily accessed
	public static bool tutorial = true;

	public static Color[] color = {Color.blue, Color.red}; //Colors for players
	public static int[][] store = new int[3][]{ //Prices for things that can be bought
		new int[]{0, 100, 100, 200, 200, 500}, //Players
		new int[]{0, 0, 100, 100, 150, 150, 200, 200, 200, 200, 250, 250, 500, 500, 500, 500, 1000, 1000, 1000, 1000}, //Walls and grounds
		new int[]{0, 100, 150, 200, 250, 500, 1000}}; //Skyboxes

	public float[,] colorValues; //RGBW values for colors (so that colors can be saved)

	public ArrayList bought = new ArrayList(){"Vincent", "pavement pattern 14", "pavement pattern 16", "Eerie Skybox"}; //List of everything user has already bought
	public int[] player = {0,4}; //Indexes of players to be used
	public int wall = 0; //Index of wall texture to be used
	public int ground = 1; //Index of ground texture to be used
	public int skybox = 0; //Index of skybox to be used

	public string mode = "single"; //Mode the user is playing
	public float wallSpacing = 3f; //Space between walls
	public float playerSpeed = 4.5f; //Speed of player
	public float rotationSpeed = 3f; //How fast the player rotates
	public int coins = 0; //Total number of coins user has collected
}
