using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;//скорость
    public float jumpPower;//сила прыжка
    public GameObject hook;//объект крючок удочки
    public Text textField;//текстовое поле для подсказок
    public Camera playerCamera;
    public Camera hookCamera;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isPulling;
    private Rigidbody2D hookRB;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hookRB = hook.GetComponent<Rigidbody2D>();
        isPulling = false;
    }

    private void Update()
    {
        if(!isPulling)Jump();
    }

    private void FixedUpdate()
    {
        if (!isPulling) Move();
        else if(isPulling) PullMove();
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    private void PullMove()
    {
        if (isPulling)
        {
            float moveX = Input.GetAxisRaw("Horizontal") * 2;
            float moveY = Input.GetAxisRaw("Vertical") * 2;
            hookRB.velocity = new Vector2(moveX, moveY);
        }
        if (Input.GetKey(KeyCode.E))
        {
            hook.SetActive(false);
            ChangeView(hookCamera, playerCamera);
            isPulling = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                break;
            case "PullPlace":
                if (Input.GetKey(KeyCode.E))
                {
                    textField.text = null;
                    Vector2 startPos = collision.transform.position;
                    hook.transform.position = new Vector2(startPos.x, startPos.y - 0.5f);
                    hook.SetActive(true);

                    ChangeView(playerCamera, hookCamera);
                    isPulling = true;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PullPlace":
                textField.text = "Нажмите 'Е'";
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = false;
                break;
            case "PullPlace":
                textField.text = null;
                break;
        }
    }

    private void ChangeView(Camera oldCamera, Camera newCamera)
    {
        oldCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
    }
}
