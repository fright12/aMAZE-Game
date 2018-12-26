using UnityEngine;
using System.Collections;

public class sampleGame : MonoBehaviour {
	public cycle player;
	public cycle wall;
	public cycle ground;
	public cycle skybox;

	public GameObject walls;
	public GameObject sampleGround;
	public UnityEngine.UI.Slider wallSpacing;

	private GameObject samplePlayer;

    void Start () {
        player.setSelected (player.choices.getIndex(settings.player1));
        wall.setSelected (wall.choices.getIndex(settings.wall));
        ground.setSelected (ground.choices.getIndex(settings.ground));
        skybox.setSelected(skybox.choices.getIndex(settings.skybox));

        player.makeChanges = () => {
            //settings.player1 = player.selectedObject.name;

            Destroy(samplePlayer);
            samplePlayer = Instantiate((GameObject)player.selectedObject, new Vector3(0f, -20f, -4f), Quaternion.identity) as GameObject;
        };

        wall.makeChanges = () => {
            //settings.wall = wall.selectedObject.name;

            foreach (MeshRenderer mr in walls.GetComponentsInChildren<MeshRenderer>())
                controller.setAndScale(mr.gameObject, (Material)wall.selectedObject, new Vector2(Mathf.Max(mr.gameObject.transform.localScale.x, mr.gameObject.transform.localScale.z) / settings.wallSpacing, 1f));
        };

        wallSpacing.value = settings.wallSpacing;
        adjustWalls(wallSpacing);

        ground.makeChanges = () => {
            //settings.ground = ground.selectedObject.name;
            controller.setAndScale(sampleGround, (Material)ground.selectedObject, new Vector2(2, 2));
        };

        skybox.makeChanges = () => {
            //settings.skybox = skybox.selectedObject.name;
            RenderSettings.skybox = (Material)skybox.selectedObject;
		};

		foreach (cycle c in GameObject.Find("Canvas").GetComponentsInChildren<cycle>(true)) {
            c.setUp();
			c.makeChanges ();
        }
	}

	public void adjustWalls(UnityEngine.UI.Slider s){
        settings.wallSpacing = s.value;

		walls.GetComponentsInChildren<Transform> () [1].transform.position = new Vector3 (0f, -17.5f, 7f + s.value);
		walls.GetComponentsInChildren<Transform> () [2].transform.position = new Vector3 (-s.value, -17.5f, 0f);
		walls.GetComponentsInChildren<Transform> () [3].transform.position = new Vector3 (s.value, -17.5f, -3f - s.value);
	}
}
