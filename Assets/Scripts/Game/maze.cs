using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class maze { //Constructs a maze as data without any physical components. Coordinates are as they would appear in a coordinate system, not a two dimensional array
	public cell upperLeft {
		get { return getCell (new cell (0, (int)bounds.y - 1)); }
	}
	public cell upperRight {
		get { return getCell (new cell((int)bounds.x - 1, (int)bounds.y - 1)); }
	}
	public cell lowerLeft {
		get { return getCell (new cell(0, 0)); }
	}
	public cell lowerRight {
		get { return getCell (new cell((int)bounds.x - 1, 0)); }
	}

	public List<cell> grid = new List<cell> (); //All of the cells in the maze
	public List<wallSpace> walls = new List<wallSpace>(); //All of the walls in the maze
	public dimensions bounds; //The dimensions of the maze

    private float wallSpacing {
        get { return settings.wallSpacing; }
    }

	public maze(int xDim, int yDim){ //Parameters are given x, y as they would appear in the coordinate system
        bounds = new dimensions (xDim, yDim);

		for (int x = 0; x < xDim; x++) { 	//Set up both cells and potential walls
			for (int y = 0; y < yDim; y++){
				cell temp = new cell(x, y);
				grid.Add (temp);

				if (x > 0) //If not in the left most column, add a wallSpace left
					walls.Add (new wallSpace(temp, temp.cellInDirection(270)));
				if (y > 0) //If not in the bottom most row, add a wallSpace below
					walls.Add (new wallSpace(temp, temp.cellInDirection (180)));
			}
		}

		List<cell> cellStack = new List<cell>(); //List of all cells visited so that path can be backtracked
		cell currentCell = grid [Random.Range (0, grid.Count - 1)];
		int visitedCells = 1;

		while (visitedCells < grid.Count){
			List<cell> neighbors = new List<cell>(); //List of possible cells to move to

			for (int i = 0; i < 4; i++){ //Check to see which cells could be moved to around the current cell
				cell temp = currentCell.cellInDirection(i * 90);

				if (!isValid (temp)) //If the cell is not on the grid, skip it
					continue;

				int dir;
				for (dir = 0; dir < 360; dir += 90){ //Make sure it is completely enclosed by walls
					if (isValid(temp.cellInDirection (dir)) && getWall(temp, temp.cellInDirection (dir)) == null) //If there is no wall between any of the adjacent cells, the cell cannot be moved to
						break;
				}

				if (dir == 360) //If a full circle has been checked, the cell can be moved to
					neighbors.Add (temp);
			}

			if (neighbors.Count > 0){ //If there are places to move to
				cellStack.Add (currentCell); //Add this cell to the ones that have been visited
				currentCell = neighbors [Random.Range (0, neighbors.Count)]; //Pick a new cell to move to from the list of possible ones

				walls.Remove (getWall (currentCell, cellStack[cellStack.Count - 1])); //Knock down the wall between the old cell and the new one

				visitedCells++;
			} else { //Can't move anywhere
				currentCell = cellStack[cellStack.Count - 1]; //Go back to the last cell 
				cellStack.RemoveAt (cellStack.Count - 1);
			}
		}
    }

    public void setUp() {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane); //Set up the ground
        //ground.transform.SetParent(container.transform);
        ground.transform.localScale = new Vector3(bounds.x * wallSpacing / 5f, 1f, bounds.y * wallSpacing / 5f);
        //Debug.Log (controller.get +", "+ userPrefs.get.player1.prefab.name +", "+ controller.get.materials.Length + ", " + settings.ground);
        controller.setAndScale(ground, access.ground, new Vector2(ground.transform.localScale.x, ground.transform.localScale.z));

        for (int i = 0; i < 2; i++)
        { //Set up the outermost walls of the maze
            addWall(new Vector2(-0.5f + bounds.x * i, -1f), new Vector2(-0.5f + bounds.x * i, bounds.y));
            addWall(new Vector2(-1f, -0.5f + bounds.y * i), new Vector2(bounds.x, -0.5f + bounds.y * i));
        }

        List<wallSpace> temp = new List<wallSpace>();
        foreach (wallSpace w in walls)
        {
            temp.Add(w);
        }

        while (temp.Count > 0)
        { //While there are still walls that need to be set up
            wallSpace start = temp[0]; //Start with the first one

            wallSpace[] ends = new wallSpace[2]; //The wallSpaces on either end of the wall to be created
            for (int i = 0; i < 2; i++)
            { //Travel in opposite directions
                wallSpace current = new wallSpace(start.cell1, start.cell2);

                do
                {
                    temp.Remove(current); //This wallSpace will be set up
                    current = current.wallSpaceInDirection(i * 180 + current.direction); //Move away from the current wallSpace
                }
                while (temp.IndexOf(current) != -1); //While a wall exists at the current spot

                ends[1 - i] = current;
            }

            addWall(new Vector2((ends[0].cell1.x + ends[0].cell2.x) / 2f, (ends[0].cell1.y + ends[0].cell2.y) / 2f), new Vector2((ends[1].cell1.x + ends[1].cell2.x) / 2f, (ends[1].cell1.y + ends[1].cell2.y) / 2f)); //Instantiate the new wall

            for (int i = 0; i < 2; i++)
            { //Check to see if either end of the wall intersects with another wall
                cell one = ends[i].cell1.cellInDirection(i * 180 + ends[i].direction); //Turn around and travel back one cell
                cell two = ends[i].cell2.cellInDirection(i * 180 + ends[i].direction); //Same for this cell

                if (isValid(ends[i].cell1) && getWall(ends[i].cell1, one) == null && getWall(ends[i].cell2, two) == null)
                { //If the end is on the grid (its not touching the edge) and there are no perpendicular walls to the left or right
                    GameObject wallCap = GameObject.CreatePrimitive(PrimitiveType.Plane); //This will be placed on the end of any exposed walls so the material doesn't look squished
                    //wallCap.transform.SetParent(container.transform);

                    Vector2 pos = ((one.getLocation() + two.getLocation() + ends[i].cell1.getLocation() + ends[i].cell2.getLocation()) / 4f + new Vector2(1, 1) / 2f - bounds.getAsV2() / 2f) * wallSpacing * 2f; //Find the average of the four cells, adjust for maze coordinates, and convert to real coordinates
                    wallCap.transform.position = new Vector3(pos.x + ends[i].direction / 90f * (i * 2f - 1) * 0.4999f / 2f, 2.5f, pos.y + (1 - ends[i].direction / 90f) * (i * 2f - 1) * 0.4999f / 2f); //Add the wall cap - depending on it's direction, add to x or z axis at end of the wall

                    wallCap.transform.localScale = new Vector3(0.05f, 1f, 0.5f);
                    wallCap.transform.rotation = Quaternion.Euler(90f, (1 - i) * 180 + ends[i].direction, 0f); //Change y rotation to face same way as wall, depending on which end it's at
                    controller.setAndScale(wallCap, access.wall, new Vector2(0.25f, 1f));
                }
            }
        }

        movement.inMaze = this;
    }

    private void addWall(Vector2 p1, Vector2 p2) { //Create a new wall given two points (in virtual maze coordinates)
        GameObject tempWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //tempWall.transform.SetParent(container.transform);

        Vector2 pos = ((p1 + p2 + new Vector2(1, 1) - bounds.getAsV2()) / 2f) * wallSpacing * 2f; //The position as it would be in the physical representation - the average of the two points adjusted by the center of the virutal maze
        tempWall.transform.position = new Vector3(pos.x, 2.5f, pos.y);

        Vector3 scale = new Vector3();

        if (p1.x == p2.x)
            scale = new Vector3(0.5f, 5f, (Mathf.Abs(p2.y - p1.y) - 1) * wallSpacing * 2f + 0.499f);
        else
            scale = new Vector3((Mathf.Abs(p2.x - p1.x) - 1) * wallSpacing * 2f + 0.499f, 5f, 0.5f);

        tempWall.transform.localScale = scale;

        controller.setAndScale(tempWall, access.wall, new Vector2(Mathf.Max(tempWall.transform.localScale.x, tempWall.transform.localScale.z) / wallSpacing, 1));
    }

    public GameObject moveToCell(GameObject obj, cell loc) { //Add the given object to the given cell in the maze
        if (!isValid(loc)) //Make sure this is a valid location
            return null;

        obj.transform.position = virtualToPhysical(loc, obj.transform.position.y);

        getCell(loc).isEmpty = false; //This cell now has something in it

        return obj;
    }

    public cell getCell(cell c){ //Compares given x and y values to list of existing cells
		for (int i = 0; i < grid.Count; i++) {
			if (grid[i].Equals (new cell(c.x, c.y))) //(int)bounds.x - 1 - 
				return grid[i];
		}

		return null;
	}

	public cell getCell(Vector3 position){ //Finds the closest cell to the given position
		return getCell (new cell((int)Mathf.Round (position.x / wallSpacing / 2f + bounds.x / 2 - 0.5f), (int)Mathf.Round (position.z / wallSpacing / 2f + bounds.y / 2 - 0.5f)));
	}

	public wallSpace getWall(cell first, cell second){ //Compares given reference cells to those of existing wallSpaces
		for (int i = 0; i < walls.Count; i++) {
			if (walls[i].Equals (new wallSpace(first, second)))
				return walls[i];
		}

		return null;
	}

	public bool isValid(cell c){ //Checks to see if a given cell is on the grid
		return c.x >= 0 && c.y >= 0 && c.x < bounds.x && c.y < bounds.y;
	}
	
	public Vector3 virtualToPhysical(cell loc, float y){ //Convert the cell coordinates to coordinates as they appear in the editor
		return new Vector3(loc.x - bounds.x / 2 + 0.5f, y / wallSpacing / 2f, loc.y - bounds.y / 2 + 0.5f) * wallSpacing * 2; //Adjust by half of maze bounds because center of maze is lower left
	}

	public void print(object o){
		Debug.Log (o);
	}
}