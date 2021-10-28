using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width, height;
    public int[,] maze;
    public Vector2 startingPoint, endingPoint, currentPoint;

    //1 = North, 2 = South, 3 = East, 4 = West, 5 = StaticPoint

    private void Start()
    {
        GenerateMaze(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.Log($"Space: {x}, {y} - " + maze[x, y]);
            }
        }
    }

    public int[,] GenerateMaze(int height, int width)
    {
        maze = new int[height, width];
        maze[(int)startingPoint.x - 1, (int)startingPoint.y - 1] = 5;
        currentPoint = startingPoint;

        for (int i = 0; i < 500; i++)
        {
            if (currentPoint != endingPoint)
            {
                int newDir = findDirection(currentPoint);
                maze[(int)currentPoint.x - 1, (int)currentPoint.y - 1] = newDir;
            }
            else
            {
                return maze;
            }
        }

        return maze;
    }

    public int findDirection(Vector2 point)
    {
        Vector2 newPoint = point; 
        int direction = Random.Range(1, 5);

        if (direction == 1 && newPoint.y < height - 1)
        {
            Debug.Log("North");
            newPoint = new Vector2(newPoint.x, newPoint.y++);
        }
        else if (direction == 2 && newPoint.y > 0)
        {
            Debug.Log("South");
            newPoint = new Vector2(newPoint.x, newPoint.y--);
        }
        else if (direction == 3 && newPoint.x < width - 1)
        {
            Debug.Log("East");
            newPoint = new Vector2(newPoint.x++, newPoint.y);
        }
        else if (direction == 4 && newPoint.y < 0)
        {
            Debug.Log("West");
            newPoint = new Vector2(newPoint.x--, newPoint.y);
        }
        else
        {
            return findDirection(currentPoint);
        }

        if (maze[(int)newPoint.x, (int)newPoint.y] == 5)
        {
            return findDirection(currentPoint);
        }
        else
        {
            Debug.Log("Currentpoint = " + currentPoint + " - And point = " + newPoint);
            currentPoint = newPoint;
            return direction;
        }
    }
}
