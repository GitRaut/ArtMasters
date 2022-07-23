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
                Debug.Log("asdkahwd");
                grabObject = collision.transform.parent;
                Rigidbody2D rb = grabObject.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                grabObject.GetComponent<CircleCollider2D>().enabled = false;
                grabObject.gameObject.SetActive(false);

                platform.isActive = true;
                break;
        }
    }
}
