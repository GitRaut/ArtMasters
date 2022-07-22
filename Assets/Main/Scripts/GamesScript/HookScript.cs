using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private bool canGrab;
    public PointEffector2D effector;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(canGrab);
            effector.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            effector.enabled = false;
        }
    }
}
