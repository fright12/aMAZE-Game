using UnityEngine;
using System.Collections;

[System.Serializable]
public class wallSpace { //Defined as the space between two cells of the maze
    public cell cell1; //First reference cell
    public cell cell2; //Second reference cell
    public int direction; //The direction the wall faces

    public wallSpace(cell c1, cell c2)
    {
        cell1 = c1;
        cell2 = c2;

        if (c1.x == c2.x)
            direction = 90;
        else
            direction = 0;
    }

    public wallSpace wallSpaceInDirection(int dir)
    { //Returns the wallSpace in the given direction
        return new wallSpace(cell1.cellInDirection(dir), cell2.cellInDirection(dir));
    }

    public override bool Equals(object other)
    {
        return cell1.Equals(((wallSpace)other).cell1) && cell2.Equals(((wallSpace)other).cell2) || cell1.Equals(((wallSpace)other).cell2) && cell2.Equals(((wallSpace)other).cell1);
    }

    public override string ToString()
    {
        return "Between cells " + cell1.ToString() + " and " + cell2.ToString();
    }
}

[System.Serializable]
public class dimensions
{
    public float x;
    public float y;

    public dimensions(float xValue, float yValue)
    {
        x = xValue;
        y = yValue;
    }

    public Vector2 getAsV2()
    {
        return new Vector2(x, y);
    }
}