using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoint;
    public float moveSpeed = 2;

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f) {
            if (goalPoint == points.Count - 1)
            {
                goalPoint = 0;
            }
            else goalPoint++;
        }
    }
}
