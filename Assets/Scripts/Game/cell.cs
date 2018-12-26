using UnityEngine;
using System.Collections;

[System.Serializable]
public class cell { //Defined as the smallest unit enclosed by four walls that make up the maze
    public int x;
    public int y;
    public dimensions location; //A vector representation of the coordinates
    public bool isEmpty;

    public cell(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
        location = new dimensions(x, y);
        isEmpty = true;
    }

    public cell cellInDirection(int dir)
    { //Returns the cell in the given direction. Directions are the same as they appear in the unity editor, corresponding to directions on a compass
        return new cell((int)(x - ((dir - 180) / 90) * ((dir / 90f) % 2)), (int)(y + ((dir - 90) / 90) * ((dir / 90f) % 2 - 1)));
    }

    public Vector2 getLocation()
    {
        return location.getAsV2();
    }

    public override bool Equals(object other)
    {
        return x == ((cell)other).x && y == ((cell)other).y;
    }

    public override string ToString()
    {
        return "Location: " + "(" + x + ", " + y + ")";
    }
}