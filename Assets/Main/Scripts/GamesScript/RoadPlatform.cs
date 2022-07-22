using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlatform : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public string type;
    public float speed;

    private bool way;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    private void Update()
    {
        switch (type){
            case "h":
                HorizontalMove();
                break;
            case "v":
                VerticalMove();
                break;
        }
    }

    private void HorizontalMove()
    {
        if(transform.position.x >= endPos.position.x) way = false;
        else if(transform.position.x <= startPos.position.x) way = true;

        if (way) rb.velocity = Vector2.right * speed;
        else rb.velocity = Vector2.left * speed;
    }

    private void VerticalMove()
    {
        if (transform.position.y >= endPos.position.y) way = false;
        else if (transform.position.y <= startPos.position.y) way = true;

        if (way) rb.velocity = Vector2.up * speed;
        else rb.velocity = Vector2.down * speed;
    }
}
