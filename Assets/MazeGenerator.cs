using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class MazeGenerator : MonoBehaviour
{
    //1 = North, 2 = South, 3 = East, 4 = West, 5 = StaticPoint
    public int width, height, scalingMultiplier;
    public int[,] maze;
    public Vector2 startingPoint, endingPoint, currentPoint;
    public List<Vector2> startingPoints, endingPoints;
    public GameObject floorPrefab, floorNoWallPrefab, mainFloor, playerPrefab;
    public List<GameObject> floor;
    public float mazeDifferenceMultiplier, MDMstart, MDMmin, MDMdecrease;
    public float waitTimeCalc, waitTimeSpawn;
    public List<Vector4> excludingPoints;
    public Material mat;
    GameObject mainPaths;
    GameObject SidePaths;
    Stopwatch timer = new Stopwatch();

    private void Start()
    {
        timer.Start();
        mainPaths = new GameObject("Main Paths");
        SidePaths = new GameObject("Side Paths");
        width++;
        height++;
        startingPoint = startingPoints[0];
        endingPoint = endingPoints[0];
        maze = new int[height + 1, width + 1];
        maze[(int)startingPoint.x - 1, (int)startingPoint.y - 1] = 5;
        currentPoint = startingPoint;
        mazeDifferenceMultiplier = MDMstart;
        ExcludePoints();
    }

    void ExcludePoints()
    {
        for (int i = 0; i < excludingPoints.Count; i++)
        {
            for (int x = 0; x < excludingPoints[i].z; x++)
            {
                for (int y = 0; y < excludingPoints[i].w; y++)
                {
                    maze[(int)excludingPoints[i].x - (int)excludingPoints[i].z / 2 + x, (int)excludingPoints[i].y - (int)excludingPoints[i].w / 2 + y] = 6;
                }
            }
        }
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        while (currentPoint != endingPoint)
        {
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

            if (newPoint != currentPoint && maze[(int)newPoint.x, (int)newPoint.y] != 6)
            {
                maze[(int)currentPoint.x, (int)currentPoint.y] = direction;

                currentPoint = newPoint;

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
        }

        startingPoint = startingPoints[0];
        endingPoint = endingPoints[0];
        currentPoint = startingPoint;
        GenerateMainPaths();
    }

    public void GenerateMainPaths()
    {
        Vector2 newPoint = startingPoint;
        GameObject oldObj = null;
        while (newPoint != endingPoint)
        {
            if (newPoint == startingPoint)
            {
                GameObject obj = Instantiate(floorPrefab, new Vector3((int)startingPoint.x, 0, (int)startingPoint.y), Quaternion.identity);
                obj.transform.localScale *= scalingMultiplier;
                obj.transform.SetParent(mainPaths.transform);
                floor.Add(obj);
                oldObj = obj;
                Ray ray = new Ray(new Vector3((width - 1) / 2, 1, (height - 1) / 2), new Vector3(startingPoint.x, 1, startingPoint.y) - new Vector3((width - 1) / 2, 1, (height - 1) / 2));
                UnityEngine.Debug.DrawRay(new Vector3((width - 1) / 2, 1, (height - 1) / 2), new Vector3(startingPoint.x, 1, startingPoint.y) - new Vector3((width - 1) / 2, 1, (height - 1) / 2), Color.red);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Destroy(hit.transform.gameObject);
                }
                currentPoint = startingPoint;
            }

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
            newObj.transform.SetParent(mainPaths.transform);
            floor.Add(newObj);
            if (maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
            {
                Destroy(newObj.transform.Find("South Wall").gameObject);
            }
            else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
            {
                Destroy(newObj.transform.Find("North Wall").gameObject);
            }
            else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
            {
                Destroy(newObj.transform.Find("West Wall").gameObject);
            }
            else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
            {
                Destroy(newObj.transform.Find("East Wall").gameObject);
            }
            if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
            {
                Destroy(oldObj.transform.Find("North Wall").gameObject);
            }
            else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
            {
                Destroy(oldObj.transform.Find("South Wall").gameObject);
            }
            else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
            {
                Destroy(oldObj.transform.Find("East Wall").gameObject);
            }
            else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
            {
                Destroy(oldObj.transform.Find("West Wall").gameObject);
            }

            if (newPoint == endingPoint)
            {
                if (endingPoints.IndexOf(endingPoint) == 0)
                {
                    Destroy(newObj.transform.Find("North Wall").gameObject);
                }
                else if (endingPoints.IndexOf(endingPoint) == 1)
                {
                    Destroy(newObj.transform.Find("South Wall").gameObject);
                }
                else if (endingPoints.IndexOf(endingPoint) == 2)
                {
                    Destroy(newObj.transform.Find("East Wall").gameObject);
                }
                else if (endingPoints.IndexOf(endingPoint) == 3)
                {
                    Destroy(newObj.transform.Find("West Wall").gameObject);
                }

                if (startingPoints.IndexOf(startingPoint) != startingPoints.Count - 1)
                {
                    startingPoint = startingPoints[startingPoints.IndexOf(startingPoint) + 1];
                    endingPoint = endingPoints[endingPoints.IndexOf(endingPoint) + 1];
                    newPoint = startingPoint;
                }
            }
            currentPoint = newPoint;
            oldObj = newObj;
        }

        for (int i = 0; i < floor.Count; i++)
        {
            maze[(int)floor[i].transform.position.x, (int)floor[i].transform.position.z] = 5;
        }
        GenerateSidePaths();
    }

    void GenerateSidePaths()
    {
        int spotsUsed = 0;
        while (spotsUsed < 10201)
        {
            int newX = Mathf.RoundToInt(Random.Range(0, width) / scalingMultiplier) * scalingMultiplier;
            int newY = Mathf.RoundToInt(Random.Range(0, height) / scalingMultiplier) * scalingMultiplier;

            while (maze[newX, newY] == 5 || maze[newX, newY] == 6)
            {
                newX = Mathf.RoundToInt(Random.Range(0, width) / scalingMultiplier) * scalingMultiplier;
                newY = Mathf.RoundToInt(Random.Range(0, height) / scalingMultiplier) * scalingMultiplier;
            }

            Vector2 startPoint = new Vector2(newX, newY);
            currentPoint = startPoint;
            while (maze[(int)currentPoint.x, (int)currentPoint.y] != 5)
            {
                Vector2 newPointCalc = currentPoint;
                int direction = Random.Range(1, 5);

                if (direction == 1 && newPointCalc.y + scalingMultiplier < height - 1)
                {
                    newPointCalc = new Vector2(newPointCalc.x, newPointCalc.y + scalingMultiplier);
                }
                else if (direction == 2 && newPointCalc.y - scalingMultiplier > 0)
                {
                    newPointCalc = new Vector2(newPointCalc.x, newPointCalc.y - scalingMultiplier);
                }
                else if (direction == 3 && newPointCalc.x + scalingMultiplier < width - 1)
                {
                    newPointCalc = new Vector2(newPointCalc.x + scalingMultiplier, newPointCalc.y);
                }
                else if (direction == 4 && newPointCalc.x - scalingMultiplier > 0)
                {
                    newPointCalc = new Vector2(newPointCalc.x - scalingMultiplier, newPointCalc.y);
                }

                if (maze[(int)newPointCalc.x, (int)newPointCalc.y] != 6)
                {
                    maze[(int)currentPoint.x, (int)currentPoint.y] = direction;
                    currentPoint = newPointCalc;
                }
            }

            Vector2 endPoint = new Vector2((int)currentPoint.x, (int)currentPoint.y);
            Vector2 newPointPath = startPoint;
            GameObject obj = Instantiate(floorPrefab, new Vector3((int)startPoint.x, 0, (int)startPoint.y), Quaternion.identity);
            obj.transform.localScale *= scalingMultiplier;
            obj.transform.SetParent(SidePaths.transform);
            floor.Add(obj);
            GameObject oldObj = obj;
            while (newPointPath != endPoint)
            {
                currentPoint = newPointPath;
                if (maze[(int)newPointPath.x, (int)newPointPath.y] == 1)
                {
                    newPointPath = new Vector2(newPointPath.x, newPointPath.y + scalingMultiplier);
                }
                else if (maze[(int)newPointPath.x, (int)newPointPath.y] == 2)
                {
                    newPointPath = new Vector2(newPointPath.x, newPointPath.y - scalingMultiplier);
                }
                else if (maze[(int)newPointPath.x, (int)newPointPath.y] == 3)
                {
                    newPointPath = new Vector2(newPointPath.x + scalingMultiplier, newPointPath.y);
                }
                else if (maze[(int)newPointPath.x, (int)newPointPath.y] == 4)
                {
                    newPointPath = new Vector2(newPointPath.x - scalingMultiplier, newPointPath.y);
                }

                if (newPointPath != endPoint)
                {
                    GameObject newObj = Instantiate(floorPrefab, new Vector3(newPointPath.x, 0, newPointPath.y), Quaternion.identity);
                    newObj.transform.localScale *= scalingMultiplier;
                    newObj.transform.SetParent(SidePaths.transform);
                    floor.Add(newObj);

                    if (maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
                    {
                        Destroy(newObj.transform.Find("South Wall").gameObject);
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
                    {
                        Destroy(newObj.transform.Find("North Wall").gameObject);
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
                    {
                        Destroy(newObj.transform.Find("West Wall").gameObject);
                    }
                    else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
                    {
                        Destroy(newObj.transform.Find("East Wall").gameObject);
                    }

                    if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
                    {
                        Destroy(oldObj.transform.Find("North Wall").gameObject);
                    }
                    else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
                    {
                        Destroy(oldObj.transform.Find("South Wall").gameObject);
                    }
                    else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
                    {
                        Destroy(oldObj.transform.Find("East Wall").gameObject);
                    }
                    else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
                    {
                        Destroy(oldObj.transform.Find("West Wall").gameObject);
                    }
                    oldObj = newObj;
                }
                else
                {
                    Ray ray = new Ray(new Vector3(newPointPath.x, 1, newPointPath.y), Vector3.down);
                    UnityEngine.Debug.DrawRay(new Vector3(currentPoint.x, 1, currentPoint.y), new Vector3(newPointPath.x, 1, newPointPath.y) - new Vector3(currentPoint.x, 1, currentPoint.y), Color.red);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject newObj = hit.transform.gameObject;

                        if (maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
                        {
                            Destroy(newObj.transform.Find("South Wall").gameObject);
                        }
                        else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
                        {
                            Destroy(newObj.transform.Find("North Wall").gameObject);
                        }
                        else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
                        {
                            Destroy(newObj.transform.Find("West Wall").gameObject);
                        }
                        else if (maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
                        {
                            Destroy(newObj.transform.Find("East Wall").gameObject);
                        }

                        if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 1)
                        {
                            Destroy(oldObj.transform.Find("North Wall").gameObject);
                        }
                        else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 2)
                        {
                            Destroy(oldObj.transform.Find("South Wall").gameObject);
                        }
                        else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 3)
                        {
                            Destroy(oldObj.transform.Find("East Wall").gameObject);
                        }
                        else if (oldObj && maze[(int)currentPoint.x, (int)currentPoint.y] == 4)
                        {
                            Destroy(oldObj.transform.Find("West Wall").gameObject);
                        }
                    }
                }
            }

            for (int i = 0; i < floor.Count; i++)
            {
                maze[(int)floor[i].transform.position.x, (int)floor[i].transform.position.z] = 5;
            }
            spotsUsed = 0;
            for (int x = 0; x < width - 1 / scalingMultiplier; x += scalingMultiplier)
            {
                for (int y = 0; y < height - 1 / scalingMultiplier; y += scalingMultiplier)
                {
                    if (maze[x, y] == 5 || maze[x, y] == 6)
                    {
                        spotsUsed++;
                    }
                }
            }
            UnityEngine.Debug.Log($"Spots used: {spotsUsed}");
        }
        for (int i = 0; i < floor.Count; i++)
        {
            MeshRenderer[] renderes = floor[i].GetComponentsInChildren<MeshRenderer>();

            for (int r = 0; r < renderes.Length; r++)
            {
                renderes[r].enabled = false;
            }
        }
        Instantiate(playerPrefab, new Vector3((width - 1) / 2, 10, (height - 1) / 2), Quaternion.identity);
        Instantiate(mainFloor, new Vector3((width - 1) / 2, 0, (height - 1) / 2), Quaternion.identity);
        timer.Stop();
        UnityEngine.Debug.Log($"Done - {timer.Elapsed}");
    }
}