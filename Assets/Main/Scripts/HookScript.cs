using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private bool canGrab;
    private Rigidbody2D grabRB;
    private Rigidbody2D rb;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.Log(canGrab);
        if (canGrab)
        {
            if (Input.GetMouseButtonDown(0))
            {
                grabRB.velocity = rb.velocity;
            }
            if (Input.GetMouseButtonUp(0))
            {
                grabRB.velocity = Vector2.zero;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "GrabObject":
                canGrab = true;
                grabRB = collision.GetComponentInParent<Rigidbody2D>();
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "GrabObject":
                canGrab = false;
                grabRB = null;
                break;
        }
    }
}
