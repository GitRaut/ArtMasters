using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;//скорость
    public float jumpPower;//сила прыжка
    public float hookSpeed;//скорость крючка
    public float climbSpeed;//скорость забирания на лестницу
    public GameObject hook;//объект крючок удочки
    public Text textField;//текстовое поле для подсказок
    public Camera playerCamera;
    public Camera hookCamera;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isPulling;
    private bool isClimbing;
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

        textField.transform.position 
            = new Vector2(transform.position.x, transform.position.y) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isPulling) PullMove();
        else if (isClimbing) LadderMove();
        else if (!isClimbing && !isPulling) Move();
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
            float moveX = Input.GetAxisRaw("Horizontal") * hookSpeed;
            float moveY = Input.GetAxisRaw("Vertical") * hookSpeed;
            hookRB.velocity = new Vector2(moveX, moveY);
        }
        if (Input.GetKey(KeyCode.E))
        {
            hook.SetActive(false);
            ChangeView(hookCamera, playerCamera);
            ChangeMove("simple");
        }
    }

    private void LadderMove()
    {
        float move = Input.GetAxis("Vertical") * climbSpeed;
        rb.velocity = new Vector2(rb.velocity.x, move);
        if(Input.GetAxis("Horizontal") != 0)
        {
            ChangeMove("simple");
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
                    hook.transform.position 
                        = new Vector2(startPos.x, startPos.y - 0.5f);
                    hook.SetActive(true);
                    ChangeView(playerCamera, hookCamera);
                    ChangeMove("pull");
                }
                break;
            case "Ladder":
                if(Input.GetAxis("Vertical") != 0)
                {
                    float startX = collision.transform.position.x;
                    transform.position 
                        = new Vector2(startX, transform.position.y);
                    ChangeMove("ladder");
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
            case "Ladder":
                textField.text = "'W', 'S'";
                break;
            case "MovingPlatform":
                
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
            case "Ladder":
                textField.text = null;
                ChangeMove("simple");
                break;
        }
    }

    private void ChangeView(Camera oldCamera, Camera newCamera)
    {
        oldCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
    }

    private void ChangeMove(string name)
    {
        switch (name)
        {
            case "simple":
                isPulling = false;
                isClimbing = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "pull":
                isClimbing = false;
                isPulling = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "ladder":
                isClimbing = true;
                isPulling = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
                break;
        }
    }
}
