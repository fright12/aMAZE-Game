using UnityEngine;
using System.Collections;

public class settings {
    public static string player1 = "vincent";
    public static float[] color1 = new float[] { 0f, 0f, 1f, 1f };
    public static string player2 = "joan";
    public static float[] color2 = new float[] { 1f, 0f, 0f, 1f };

    public static string wall = "pavement pattern 14";
    public static string ground = "pavement pattern 16";
    public static string skybox = "sunny1 skybox";

    public static System.Type mode = typeof(single);
    public static float wallSpacing = 3f; //Space between walls
	public static bool playTransition = true;
    public static bool showTutorial = false;

    /*public static int[][] store = new int[3][]{ //Prices for things that can be bought
		new int[]{0, 100, 100, 200, 200, 500}, //Players
		new int[]{0, 0, 100, 100, 150, 150, 200, 200, 200, 200, 250, 250, 500, 500, 500, 500, 1000, 1000, 1000, 1000}, //Walls and grounds
		new int[]{0, 100, 150, 200, 250, 500, 1000}}; //Skyboxes
	*/
}