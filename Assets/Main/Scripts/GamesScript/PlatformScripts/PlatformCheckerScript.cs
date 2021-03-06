using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheckerScript : MonoBehaviour
{
    public Sprite activePlatform;
    private MovingPlatformU platform;
    private Transform grabObject;
    public SpriteRenderer sprite;

    private void Start()
    {
        if(sprite == null)
            sprite = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        platform = transform.parent.GetComponent<MovingPlatformU>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "GrabObject":
                grabObject = collision.transform.parent;
                grabObject.gameObject.SetActive(false);
                sprite.sprite = activePlatform;
                platform.isActive = true;
                transform.gameObject.SetActive(false);
                break;
        }
    }
}
