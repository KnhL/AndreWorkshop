using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //1 = North, 2 = South, 3 = East, 4 = West, 5 = StaticPoint
    public int width, height, scalingMultiplier;
    public int[,] maze;
    public Vector2 startingPoint, endingPoint, currentPoint;
    public List<Vector2> startingPoints, endingPoints;
    public GameObject floorPrefab;
    public List<GameObject> floor;
    public float mazeDifferenceMultiplier, MDMstart, MDMmin, MDMdecrease;
    public float waitTimeCalc, waitTimeSpawn;
    public List<Vector4> excludingPoints;
    public Material mat, mat1, mat2, mat3, mat4, mat5;

    private void Start()
    {
        for (int i = 0; i < startingPoints.Count; i++)
        {
            startingPoints[i] = new Vector2(Mathf.Round(startingPoints[i].x / scalingMultiplier) * scalingMultiplier, Mathf.Round(startingPoints[i].y / scalingMultiplier) * scalingMultiplier);
            endingPoints[i] = new Vector2(Mathf.Round(endingPoints[i].x / scalingMultiplier) * scalingMultiplier, Mathf.Round(endingPoints[i].y / scalingMultiplier) * scalingMultiplier);
        }

        width++;
        height++;
        startingPoint = startingPoints[0];
        endingPoint = endingPoints[0];
        maze = new int[height + 1, width + 1];
        maze[(int)startingPoint.x - 1, (int)startingPoint.y - 1] = 5;
        currentPoint = startingPoint;
        mazeDifferenceMultiplier = MDMstart;
        StartCoroutine(ExcludePoints());
    }

    IEnumerator ExcludePoints()
    {
        for (int i = 0; i < excludingPoints.Count; i++)
        {
            for (int x = 0; x < excludingPoints[i].z; x++)
            {
                for (int y = 0; y < excludingPoints[i].w; y++)
                {
                    Debug.Log($"X - {(int)excludingPoints[i].x - (int)excludingPoints[i].z / 2 + x} and Y - {(int)excludingPoints[i].y - (int)excludingPoints[i].w / 2 + y}");
                    maze[(int)excludingPoints[i].x - (int)excludingPoints[i].z / 2 + x, (int)excludingPoints[i].y - (int)excludingPoints[i].w / 2 + y] = 5;
                    GameObject obj = Instantiate(floorPrefab, new Vector3((int)excludingPoints[i].x - (int)excludingPoints[i].z / 2 + x, -0.25f, (int)excludingPoints[i].y - (int)excludingPoints[i].w / 2 + y), Quaternion.identity);
                    obj.GetComponent<MeshRenderer>().material = mat;
                    yield return new WaitForSeconds(waitTimeCalc); maze[(int)excludingPoints[i].x - (int)excludingPoints[i].z / 2 + x, (int)excludingPoints[i].y - (int)excludingPoints[i].w / 2 + y] = 5;
                }
            }
        }
        StartCoroutine(GenerateMaze());
    }

    public IEnumerator createPath()
    {
        Vector2 newPoint = startingPoint;
        GameObject obj = Instantiate(floorPrefab, new Vector3(currentPoint.x, 0, currentPoint.y), Quaternion.identity);
        obj.transform.localScale *= scalingMultiplier;
        while (newPoint != endingPoint)
        {
            Debug.Log($"Floor Posistion: {newPoint} - Path: {startingPoints.IndexOf(startingPoint) + 1} - Current Start: {startingPoints.IndexOf(startingPoint)} - Current End: {endingPoints.IndexOf(endingPoint)}");
            if (maze[(int)newPoint.x, (int)newPoint.y] == 1)
            {
                newPoint = new Vector2(newPoint.x, newPoint.y + scalingMultiplier);
            }
            else if (maze[(int)newPoint.x, (int)newPoint.y] == 2)
            {
                newPoint = new Vector2(newPoint.x, newPoint.y - scalingMultiplier);
            }
            else if (maze[(int)newPoint.x, (int)newPoint.y] == 3)
            {
                newPoint = new Vector2(newPoint.x + scalingMultiplier, newPoint.y);
            }
            else if (maze[(int)newPoint.x, (int)newPoint.y] == 4)
            {
                newPoint = new Vector2(newPoint.x - scalingMultiplier, newPoint.y);
            }
            GameObject newObj = Instantiate(floorPrefab, new Vector3(newPoint.x, 0, newPoint.y), Quaternion.identity);
            newObj.transform.localScale *= scalingMultiplier;
            floor.Add(newObj);
            yield return new WaitForSeconds(waitTimeSpawn);
            Debug.Log($"CurrentPoint: {currentPoint}");
            Debug.Log($"NewPoint: {newPoint}");
            Debug.Log($"Endingpoint: {endingPoint}");
            if (newPoint == endingPoint && startingPoints.IndexOf(startingPoint) != startingPoints.Count - 1)
            {
                startingPoint = startingPoints[startingPoints.IndexOf(startingPoint) + 1];
                endingPoint = endingPoints[endingPoints.IndexOf(endingPoint) + 1];
                newPoint = startingPoint;
            }
        }

        for (int i = 0; i < floor.Count; i++)
        {
            maze[(int)floor[i].transform.position.x, (int)floor[i].transform.position.y] = 5;
        }
    }

    public IEnumerator GenerateMaze()
    {
        GameObject parrent = new GameObject("Debug");
        while (currentPoint != endingPoint)
        {
            Debug.Log($"Point: {currentPoint} - Path: {startingPoints.IndexOf(startingPoint) + 1} - Current Start: {startingPoints.IndexOf(startingPoint)} - Current End: {endingPoints.IndexOf(endingPoint)}");
            Vector2 newPoint = currentPoint;
            int direction = Random.Range(1, 5);

            if (direction == 1 && newPoint.y + scalingMultiplier < height - 1 && newPoint.y < Mathf.RoundToInt(endingPoint.y + mazeDifferenceMultiplier * height))
            {
                newPoint = new Vector2(newPoint.x, newPoint.y + scalingMultiplier);
            }
            else if (direction == 2 && newPoint.y - scalingMultiplier > 0 && newPoint.y > Mathf.RoundToInt(endingPoint.y - mazeDifferenceMultiplier * height))
            {
                newPoint = new Vector2(newPoint.x, newPoint.y - scalingMultiplier);
            }
            else if (direction == 3 && newPoint.x + scalingMultiplier < width - 1 && newPoint.x < Mathf.RoundToInt(endingPoint.x + mazeDifferenceMultiplier * width))
            {
                newPoint = new Vector2(newPoint.x + scalingMultiplier, newPoint.y);
            }
            else if (direction == 4 && newPoint.x - scalingMultiplier > 0 && newPoint.x > Mathf.RoundToInt(endingPoint.x - mazeDifferenceMultiplier * width))
            {
                newPoint = new Vector2(newPoint.x - scalingMultiplier, newPoint.y);
            }

            if (newPoint != currentPoint && maze[(int)newPoint.x, (int)newPoint.y] != 5)
            {
                maze[(int)currentPoint.x, (int)currentPoint.y] = direction;

                if (maze[(int)newPoint.x, (int)newPoint.y] != 5)
                {
                    GameObject obj = Instantiate(floorPrefab, new Vector3((int)newPoint.x, 0, (int)newPoint.y), Quaternion.identity);
                    Debug.Log(new Vector3((int)newPoint.x, 0, (int)newPoint.y));
                    obj.transform.localScale *= scalingMultiplier;
                    obj.transform.parent = parrent.transform;

                    if (maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
                    {
                        obj.GetComponent<MeshRenderer>().material = mat1;
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
                    {
                        obj.GetComponent<MeshRenderer>().material = mat2;
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
                    {
                        obj.GetComponent<MeshRenderer>().material = mat3;
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
                    {
                        obj.GetComponent<MeshRenderer>().material = mat4;
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 5)
                    {
                        obj.GetComponent<MeshRenderer>().material = mat5;
                    }
                }

                currentPoint = newPoint;

                yield return new WaitForSeconds(waitTimeCalc);

                if (mazeDifferenceMultiplier > MDMmin)
                {
                    mazeDifferenceMultiplier -= MDMdecrease;
                }
                else
                {
                    mazeDifferenceMultiplier = MDMmin;
                }

                if (newPoint == endingPoint && startingPoints.IndexOf(startingPoint) != startingPoints.Count - 1)
                {
                    mazeDifferenceMultiplier = MDMstart;
                    startingPoint = startingPoints[startingPoints.IndexOf(startingPoint) + 1];
                    endingPoint = endingPoints[endingPoints.IndexOf(endingPoint) + 1];
                    currentPoint = startingPoint;
                }
            }
            else
            {
                Debug.Log("Retry");
            }
        }

        startingPoint = startingPoints[0];
        endingPoint = endingPoints[0];
        StartCoroutine(createPath());
    }
}
