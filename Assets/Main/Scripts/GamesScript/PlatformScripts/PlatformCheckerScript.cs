using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheckerScript : MonoBehaviour
{
    private MovingPlatformU platform;

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
                Transform obj = collision.transform.parent;
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                obj.SetParent(transform.parent);
                obj.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                obj.localScale = new Vector2(1, 1);

                platform.isActive = true;
                break;
        }
    }
}
