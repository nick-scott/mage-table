using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeIdentifier : MonoBehaviour
{
    public static Shape getShape(List<GameObject> pointsInOrder)
    {
        Debug.Log("Getting shape for: " + pointsInOrder.Count + " points");
        if (pointsInOrder.Count == 0)
        {
            return Shape.CIRCLE;
        }
        int edges = countEdges(pointsInOrder);
        Debug.Log("Edges: " + edges);
        if (edges == 3)
        {
            return Shape.TRIANGLE;
        }
        if (edges == 4)
        {
            return Shape.SQUARE;
        }
        if (edges == 5)
        {
            return Shape.STAR;
        }
        return Shape.CIRCLE;
    }


    private static int countEdges(List<GameObject> pointsInOrder)
    {
        List<Vector3> currentEdgeList = new List<Vector3>();
        Vector3 averageNormalVector = Vector3.zero;
        Vector3 firstNormalVector = Vector3.zero;
        int edgeThresholdCount = 0;
        int VECTORS_FOR_EDGE = 4;
        double ANGLE_THRESHOLD = 30;
        int foundEdges = 0;

        bool edgeFound = false;
        GameObject[] gameObjects = pointsInOrder.ToArray();

        if (pointsInOrder.ToArray()[0] != null)
        {
            int index;
            for (index = 0; index < gameObjects.Length - 1; index++)
            {
                Vector3 currentNormalVector = Vector3.Cross(gameObjects[index].transform.position,
                    gameObjects[index + 1].transform.position).normalized;
                if (!edgeFound)
                {
                    currentEdgeList.Add(currentNormalVector);
                    averageNormalVector = averageVector(currentEdgeList);
                    if (Vector3.Angle(averageNormalVector, currentNormalVector) < ANGLE_THRESHOLD)
                    {
                        ++edgeThresholdCount;
                        if (edgeThresholdCount > VECTORS_FOR_EDGE)
                        {
                            if (foundEdges == 0)
                            {
                                firstNormalVector = averageNormalVector;
                            }
                            edgeFound = true;
                            foundEdges++;
                        }
                    }
                    else
                    {
                        currentEdgeList.Clear();
                        edgeThresholdCount = 0;
                    }
                }
                else if (Vector3.Angle(averageNormalVector, currentNormalVector) > ANGLE_THRESHOLD)
                {
                    Debug.Log("Vector outside edge. Staring new edge calculation");
                    currentEdgeList.Clear();
                    edgeFound = false;
                    edgeThresholdCount = 0;
                }
                else
                {
                    currentEdgeList.Add(currentNormalVector);
                    averageNormalVector = averageVector(currentEdgeList);
                }
            }
            if (Vector3.Angle(averageNormalVector, firstNormalVector) < ANGLE_THRESHOLD && edgeFound)
            {
                Debug.Log("Final edge aligns with original, removing duplicate");
                foundEdges--;
            }
        }


        return foundEdges;
    }


    public static Vector3 averageVector(List<Vector3> vectorList)
    {
        Vector3 bigVector = Vector3.zero;
        foreach (Vector3 vector in vectorList)
        {
            bigVector += vector;
        }
        return bigVector.normalized;
    }

    public static double angleBetween3Points(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = Vector3.Cross(a, b);
        Vector3 bc = Vector3.Cross(b, c);

        float res = Vector3.Dot(ab.normalized, bc.normalized);

        double between3Points = Mathf.Acos(res) * 180.0 / Mathf.PI;

        return between3Points;
    }
}