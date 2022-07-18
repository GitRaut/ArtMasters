using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smooth;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 pos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, pos, smooth * Time.fixedDeltaTime);
    }
}
