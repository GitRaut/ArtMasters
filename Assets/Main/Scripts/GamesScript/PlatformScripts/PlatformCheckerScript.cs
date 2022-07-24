using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheckerScript : MonoBehaviour
{
    private MovingPlatformU platform;
    private Transform grabObject;

    private void Start()
    {
        platform = transform.parent.GetComponent<MovingPlatformU>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "GrabObject":
                grabObject = collision.transform.parent;
                grabObject.gameObject.SetActive(false);
                platform.isActive = true;
                break;
        }
    }
}
