using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class computerMovement : movement {
	private cell next;
	private float min = Mathf.Infinity;
	private float computerSpeed = 3f;
	
	private float horizontal, vertical;
    private bool deadEnd = false;
	private maze gameMaze;
		
	private Vector3 pTransform;
	
	void Awake() {
		setUp ();

        gameMaze = new maze((int)inMaze.bounds.x, (int)inMaze.bounds.y);
        for (int i = 0; i < inMaze.grid.Count; i++)
            gameMaze.grid[i] = inMaze.grid[i];
        for (int i = 0; i < inMaze.walls.Count; i++)
            gameMaze.walls[i] = inMaze.walls[i];

        next = gameMaze.getCell (transform.position).cellInDirection (getRotation ());

		if (Application.loadedLevel == 0)
			computerSpeed = 10f;
	}
	
	void FixedUpdate(){
		anim.SetFloat (hash.speedFloat, computerSpeed, speedDampTime, Time.deltaTime);
		
		transform.rotation = Quaternion.Euler (0f, getRotation (), 0f);
		
		float distance = Vector3.Distance (gameMaze.virtualToPhysical (next, 0f), transform.position);
		
		if (distance > min && gameMaze.getCell (transform.position).Equals (next)) {
			List<int> moves = getMoveDirections (transform.position);
			
			if (moves.Count == 1){ //Dead end
				deadEnd = true;
			} else {
				if (moves.Count > 2 && deadEnd){
					cell current = gameMaze.getCell (transform.position);

                    if (Application.loadedLevel != 0)
                    {
                        gameMaze.walls.Add(new wallSpace(current, current.cellInDirection((getRotation() + 180) % 360)));
                        //gameMaze.setUp();
                    }

                    deadEnd = false;
				}
				
				moves.Remove ((getRotation () + 180) % 360);
			}
			
			setRotation (moves [Random.Range (0, moves.Count)]);
			transform.position = gameMaze.virtualToPhysical (next, 0f);
			
			min = Mathf.Infinity;
			next = gameMaze.getCell (transform.position).cellInDirection (getRotation ());
		} else {
			min = distance;
		}
	}
	
	public int getRotation(){
		return (int)(Mathf.Round (transform.rotation.eulerAngles.y / 90f) % 4 * 90f);
	}
}