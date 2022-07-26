using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private bool canGrab;
    public PointEffector2D effector;
    public AudioSource crystal;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            effector.enabled = true;
            crystal.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            effector.enabled = false;
            crystal.Stop();
        }
    }
}
