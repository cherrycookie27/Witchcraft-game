using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RuneDrawing : MonoBehaviour
{
    public List<GameObject> checkPoints;
    public List<int> correctLenghts;
    public List<GameObject> drawn;
    public List<int> drawnLenghts;

    GameObject previousHit;

    public LineRenderer linePrefab;
    int pointCount;
    private int currentCheckpointIndex = 0;
    public List<LineRenderer> activeLines = new List<LineRenderer>();
    Vector3 mousePos;
    bool drawing;

    private void Start()
    {
        drawn = new List<GameObject>();
        drawnLenghts = new List<int>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LineRenderer g = Instantiate(linePrefab);
            activeLines.Add(g);
            pointCount = 0;
            drawnLenghts.Add(0);
            drawing = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            drawing = false;
        }
        if (drawing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePos = ray.origin;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject != null && hitObject != previousHit)
                {
                    previousHit = hitObject;
                    drawn.Add(hitObject);
                    drawnLenghts[activeLines.Count -1]++;
                    if (pointCount == 0)
                    {
                        Vector3[] points = new Vector3[1];
                        activeLines[activeLines.Count - 1].positionCount = points.Length;
                        activeLines[activeLines.Count - 1].SetPosition(0, hitObject.transform.position);
                        pointCount += 1;
                    }
                    else
                    {

                        Vector3[] points = new Vector3[activeLines[activeLines.Count - 1].positionCount + 1];
                        for(int i = 0; i < activeLines[activeLines.Count - 1].positionCount; i++)
                        {
                            points[i] = activeLines[activeLines.Count - 1].GetPosition(i);
                        }
                        activeLines[activeLines.Count - 1].positionCount = points.Length;
                        activeLines[activeLines.Count - 1].SetPositions(points);
                        activeLines[activeLines.Count - 1].SetPosition(pointCount, hitObject.transform.position);
                        pointCount++;
                    }

                    if (drawn.Count == checkPoints.Count)
                    {
                        if(correctLenghts.Count == drawnLenghts.Count)
                        {
                            for (int i = 0; i < checkPoints.Count; i++)
                            {
                                if (checkPoints[i] != drawn[i])
                                {
                                    Debug.Log("dumb");
                                    ResetLines();
                                    return;
                                }
                            }
                            for (int x = 0; x < correctLenghts.Count; x++)
                            {
                                if (correctLenghts[x] != drawnLenghts[x])
                                {
                                    Debug.Log("dumb2");
                                    ResetLines();
                                    return;
                                }
                            }

                            Debug.Log("Winner!!!!");
                            drawing = false;
                            return;
                        }
                       

                        Debug.Log("dumb3");
                        ResetLines();
                    }
                }
            }
        }
    }

    private void HandleCheckpointClick(GameObject clickedCheckpoint)
    {
        if (clickedCheckpoint == checkPoints[currentCheckpointIndex])
        {
            if (currentCheckpointIndex > 0)
            {
                DrawLine(checkPoints[currentCheckpointIndex - 1].transform.position, clickedCheckpoint.transform.position);
            }

            currentCheckpointIndex++;
        }
        else
        {
            Debug.Log("Incorrect order! Resetting lines.");
            ResetLines();
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        LineRenderer line = Instantiate(linePrefab);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        activeLines.Add(line);
    }

    private void ResetLines()
    {
        drawing = false;
        foreach (LineRenderer line in activeLines)
        {
            Destroy(line.gameObject);
        }
        activeLines.Clear();

        currentCheckpointIndex = 0;
        drawn = new List<GameObject>();
        drawnLenghts = new List<int>();
        pointCount = 0;
        previousHit = null;

    }
}