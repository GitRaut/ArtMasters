using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformU : MonoBehaviour
{
    public enum MovementType
    {
        simple,
        jerk
    }

    public MovementType type = MovementType.simple;
    public MovementPath path;
    public float speed = 1;
    public float maxDistance = .1f;
    public bool isActive = false;

    private IEnumerator<Transform> pointInPath;

    private void Start()
    {
        if(path == null)
        {
            return;
        }

        pointInPath = path.GetNextPathPoint();

        pointInPath.MoveNext();
        
        if (pointInPath.Current == null)
        {
            Debug.Log("Нужны точки");
            return;
        }

        transform.position = pointInPath.Current.position;
    }

    private void Update()
    {
        if (isActive)
        {
            if (pointInPath == null || pointInPath.Current == null)
            {
                return;
            }

            if (type == MovementType.simple)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
            }
            else if (type == MovementType.jerk)
            {
                transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
            }

            var distanceSquare = (transform.position - pointInPath.Current.position).sqrMagnitude;
            if (distanceSquare < maxDistance * maxDistance)
            {
                pointInPath.MoveNext();
            }
        }
    }
}

